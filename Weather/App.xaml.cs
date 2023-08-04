using Weather.Services;
using Weather.Views;
using Xamarin.Forms;

namespace Weather
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<IApiCallerGeocoding, ApiCallerGeocoding>();
            DependencyService.Register<IApiCallerWeather, ApiCallerWeather>();
            MainPage = new NavigationPage(new WeatherDaysPage());
            PermissionsService.CheckAndRequestLocationPermission();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
