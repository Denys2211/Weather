using Prism.Mvvm;
using Weather.Models;
using Weather.Services;
using Xamarin.Forms;

namespace Weather.ViewModel
{
    public class BaseViewModel : BindableBase
    {
        public ApiCallerWeather ApiWeather = DependencyService.Get<ApiCallerWeather>();
        public ApiCallerGeocoding ApiGeocoding = DependencyService.Get<ApiCallerGeocoding>();
        protected static ForecastInfo ValueForecast { get; set; }
        protected static int Hours { get; set; }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }

        }
        
    }
}
