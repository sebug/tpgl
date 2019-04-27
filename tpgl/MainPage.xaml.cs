using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tpgl.ViewModels;
using Xamarin.Forms;

namespace tpgl
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var bindingContext = this.BindingContext as MainViewModel;

            if (bindingContext != null)
            {
                Task.Run(async () =>
                {
                    await bindingContext.LoadStops();
                });
            }
        }
    }
}
