using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using tpgl.Models;

namespace tpgl.Services
{
    public class TPGService : ITPGService
    {
        private readonly string apiKey;
        private readonly HttpClient client;

        public TPGService()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(Helpers.Secrets.APIEndpoint);
            this.client.Timeout = TimeSpan.FromSeconds(20);
            this.apiKey = Helpers.Secrets.APIKey;
        }

        public async Task<GetNextDeparturesResponse> GetNextDepartures(Stop stop)
        {
            if (stop == null || stop.StopCode == null)
            {
                return null;
            }
            string nextDeparturesString = await this.client.GetStringAsync("GetNextDepartures?key=" +
                this.apiKey + "&stopCode=" + stop.StopCode);
            var nextDepartures = JsonConvert.DeserializeObject<GetNextDeparturesResponse>(nextDeparturesString);
            return nextDepartures;
        }

        public async Task<StopsResponse> GetStops()
        {
            string stopsResponse = await this.client.GetStringAsync("GetStops?key=" + this.apiKey);
            var stops = JsonConvert.DeserializeObject<StopsResponse>(stopsResponse);
            return stops;
        }
    }
}
