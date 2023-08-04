using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weather.Models;
using Weather.Services.SnackBarSevice;
using Weather.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.ViewModels
{
    public class WeatherDaysViewModel : WeatherPreferencesViewModel 
    {
        string date_today;
        private float humidityNow;
        private float windNow;
        private float cloudinessNow;
        private float pressureNow;
        private string descriptionWeatherNow;
        private string imageWeatherSourceNow;
        private float tempNow;
        private bool _isLoading = false;

        public ObservableCollection<Daily> Days { get; set; }
        
        public float HumidityNow
        {
            get { return humidityNow; }
            set { SetProperty(ref humidityNow, value); }
        }
        public float WindNow
        {
            get { return windNow; }
            set { SetProperty(ref windNow, value); }
        }
        public float PressureNow
        {
            get { return pressureNow; }
            set { SetProperty(ref pressureNow, value); }
        }
        public float CloudinessNow
        {
            get { return cloudinessNow; }
            set { SetProperty(ref cloudinessNow, value); }
        }
        public string DescriptionWeatherNow
        {
            get { return descriptionWeatherNow; }
            set { SetProperty(ref descriptionWeatherNow, value); }
        }
        public string ImageWeatherSourceNow
        {
            get { return imageWeatherSourceNow; }
            set { SetProperty(ref imageWeatherSourceNow, value); }
        }
        public float TempNow
        {
            get { return tempNow; }
            set { SetProperty(ref tempNow, value); }
        }

        public string DateToday
        {
            get { return date_today; }
            set { SetProperty(ref date_today, value); }
        }

        public Command LoadItemsCommand { get; set; }

        public Command OnForecastHourly { get; set; }

        public WeatherDaysViewModel()
        {
            Days = new ObservableCollection<Daily>();
            LoadItemsCommand = new Command(async () => await RefreshForecastAsync());
            OnForecastHourly = new Command<Daily>(ForecastHourly);
            _ = GetForecast();
        }
        async Task GetForecast()
        {
            if(_isLoading) return;

            _isLoading = true;

            if (StatusGetCoordinates)
            {
                if (!await TryGetCoordinates())
                {
                    return;
                }
                else
                {
                    await Save();
                }
            }

            if (ListCity.Count != 0 && !StatusGetCoordinates)
            {
                await ActiveCityAsync();
            }

            var url = $"https://api.openweathermap.org/data/2.5/onecall?lat={Latitude}&lon={Longitude}&appid=0f5bc762e1e2d34191f752caf96a1e60&units=metric";
            var result = await ApiWeather.Get(url);

            if (result.Successful)
            {
                Days.Clear();

                ValueForecast = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                GetForecastNow();

                for (int i = 0; i < 7; i++)
                {
                    Days.Add(ValueForecast.daily[i]);
                }
            }
        }
        async Task<bool> TryGetCoordinates()
        {
            Location location = await ApiGeocoding.GetLocationGPS();

            if (location != null)
            {
                Latitude = location.Latitude;
                Longitude = location.Longitude;
                Current_City = await ApiGeocoding.GetCity(location);
                await MapFocusCity(Latitude, Longitude);

                DependencyService.Get<ISnackBarService>()?.ShowSnackBar($"Obtained city using GPS - {Current_City}");
                return true;
            }
            else
            {
                DependencyService.Get<ISnackBarService>()?.ShowSnackBar("Fail to get location!!!");
                return false;
            }

        }
        async Task RefreshForecastAsync()
        {
            try
            {
                await GetForecast();
            }
            finally
            {
                IsBusy = false;
                _isLoading = false;
            }

        }
        void GetForecastNow()
        {
            DescriptionWeatherNow = ValueForecast.daily[0].weather[0].description;
            ImageWeatherSourceNow = ValueForecast.hourly[3].weather[0].icon;
            TempNow = ValueForecast.hourly[3].temp;
            WindNow = ValueForecast.daily[0].wind_speed;
            HumidityNow = ValueForecast.daily[0].humidity;
            PressureNow = ValueForecast.daily[0].pressure;
            CloudinessNow = ValueForecast.daily[0].clouds;
            DateToday = DateTime.Now.ToString();
        }
        async void ForecastHourly(Daily daily)
        {
            for(int i = 0; i < 8; i++)
            {
                if(ValueForecast.daily[i] == daily)
                {
                    Hours = (i + 1) * 24;
                    break;
                }
            }

            await App.Current.MainPage.Navigation.PushAsync(new WeatherHoursPage());
        }
        
    }
}
