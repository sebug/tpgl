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

        public event PropertyChangedEventHandler PropertyChanged;

        private ITPGService tpgService;

        public MainViewModel()
        {
            this.Message = "tpgl from the view model";

            this.tpgService = DependencyService.Resolve<ITPGService>();
        }

        public async Task LoadStops()
        {
            var stops = await this.tpgService.GetStops();

            if (stops != null && stops.Stops != null && stops.Stops.Any())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.Message = "DI3" + stops.Stops.First().StopName;
                    this.Stops = new ObservableCollection<Stop>(stops.Stops);
                });
            }
        }
    }
}
