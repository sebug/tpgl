using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using tpgl.Models;
using System.Linq;
using Polly;
using Xamarin.Forms;
using Polly.Timeout;
using System.Threading;

namespace tpgl.Services
{
    public class TPGService : ITPGService
    {
        private readonly string apiKey;
        private readonly HttpClient client;
        private readonly IManifestResourceService _manifestResourceService;

        public TPGService()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(Helpers.Secrets.APIEndpoint);
            this.client.Timeout = TimeSpan.FromSeconds(20);
            this.apiKey = Helpers.Secrets.APIKey;
            this._manifestResourceService = DependencyService.Resolve<IManifestResourceService>();
        }

        public async Task<GetNextDeparturesResponse> GetNextDepartures(Stop stop)
        {
            if (stop == null || stop.StopCode == null)
            {
                return null;
            }
            var nextDepartures =
                await Policy.Handle<HttpRequestException>(ex => !ex.Message.Contains("404"))
                .WaitAndRetryAsync(retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (ex, time) =>
                {
                    Console.WriteLine($"Something went wrong: {ex.Message}, retrying...");
                })
                .ExecuteAsync(async () =>
                {
                    Console.WriteLine("Trying to fetch remote data...");
                    string nextDeparturesString = await this.client.GetStringAsync("GetNextDepartures?key=" +
                        this.apiKey + "&stopCode=" + stop.StopCode);
                    return JsonConvert.DeserializeObject<GetNextDeparturesResponse>(nextDeparturesString);
                });
            
            return nextDepartures;
        }

        public async Task<StopsResponse> GetStops()
        {
            var timeoutPolicy = Policy.TimeoutAsync(2, TimeoutStrategy.Optimistic);
            try
            {
                return await timeoutPolicy.ExecuteAsync<StopsResponse>(async ct =>
                {
                    var response = await this.client.GetAsync("GetStops?key=" + this.apiKey, ct);
                    string stopsResponse = await response.Content.ReadAsStringAsync();
                    var stops = JsonConvert.DeserializeObject<StopsResponse>(stopsResponse);
                    stops.Stops = stops.Stops.OrderBy(s => s.StopName).ToList();
                    return stops;
                }, cancellationToken: CancellationToken.None);
            }
            catch
            {
                string stopsFromManifest = this._manifestResourceService.GetManifestResource("tpgl.Resources.stops.json");
                var stops = JsonConvert.DeserializeObject<StopsResponse>(stopsFromManifest);
                stops.Stops = stops.Stops.OrderBy(s => s.StopName).ToList();
                return stops;
            }
        }
    }
}
