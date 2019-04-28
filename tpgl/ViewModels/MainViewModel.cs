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
