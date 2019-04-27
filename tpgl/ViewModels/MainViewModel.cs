using System;
using System.ComponentModel;
using tpgl.Helpers;
using tpgl.Services;

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
                        this.PropertyChanged(this, new PropertyChangedEventArgs("message"));
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

            this.tpgService = new TPGService(Secrets.APIKey, Secrets.APIEndpoint);

            this.tpgService.GetStops().ContinueWith((stops) =>
            {
                var res = stops.Result;
                this.Message = "First stop is " + res.Stops[0].StopName;
            });
        }
    }
}
