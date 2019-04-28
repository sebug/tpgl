using System;
using System.ComponentModel;
using tpgl.Helpers;
using tpgl.Services;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using tpgl.Models;

namespace tpgl.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string message;

        public string Message
        {
            get { return this.message; }
            set
            {
                if (this.message != value)
                {
                    this.message = value;

                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Message"));
                    }
                }
            }
        }

        private ObservableCollection<Stop> stops = new ObservableCollection<Stop>();

        public ObservableCollection<Stop> Stops
        {
            get { return this.stops; }
            set
            {
                if (this.stops != value)
                {
                    this.stops = value;

                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Stops"));
                    }
                }
            }
        }

        private Stop selectedStop;
        public Stop SelectedStop
        {
            get { return this.selectedStop; }
            set
            {
                if (this.selectedStop != value)
                {
                    this.selectedStop = value;
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("SelectedStop"));
                    }
                    if (this.selectedStop != null)
                    {
                        this.Message = this.selectedStop.StopCode;
                    }
                    this.SelectedStopChanged(this.selectedStop);
                }
            }
        }

        private ObservableCollection<Departure> departures = new ObservableCollection<Departure>();
        public ObservableCollection<Departure> Departures
        {
            get { return this.departures; }
            set
            {
                if (this.departures != value)
                {
                    this.departures = value;
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Departures"));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ITPGService tpgService;

        public MainViewModel()
        {
            this.Message = String.Empty;

            this.tpgService = DependencyService.Resolve<ITPGService>();
        }

        private void SelectedStopChanged(Stop s)
        {
            Task.Run(async () =>
            {
                var response = await this.tpgService.GetNextDepartures(s);
                if (response != null && response.Departures != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.Message = "Found " + response.Departures.Count + " departures";
                        this.Departures = new ObservableCollection<Departure>(response.Departures);
                    });
                }
            });
        }

        public async Task LoadStops()
        {
            try
            {
                var stopsResponse = await this.tpgService.GetStops();

                if (stopsResponse != null && stopsResponse.Stops != null && stopsResponse.Stops.Any())
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.Stops = new ObservableCollection<Stop>(stopsResponse.Stops);
                    });
                }
            }
            catch (Exception ex)
            {
                var bex = ex.GetBaseException();
                this.Message = bex.Message;
            }
        }
    }
}
