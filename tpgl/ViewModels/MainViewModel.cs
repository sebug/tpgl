using System;
using System.ComponentModel;
using tpgl.Helpers;
using tpgl.Services;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

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

        public event PropertyChangedEventHandler PropertyChanged;

        private ITPGService tpgService;

        public MainViewModel()
        {
            this.Message = "tpgl from the view model";
            this.Message += Helpers.Secrets.APIKey;

            this.tpgService = DependencyService.Resolve<ITPGService>();
        }

        public async Task LoadStops()
        {
            var stops = await this.tpgService.GetStops();

            if (stops != null && stops.Stops != null && stops.Stops.Any())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.Message = "DI" + stops.Stops.First().StopName;
                });
            }
        }
    }
}
