using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weather.Helper;
using Weather.Models;
using Weather.View;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.ViewModel
{
    public class WeatherDaysViewModel : BaseViewModel
    {
        public ObservableCollection<Daily> Week { get; set; }

        public float HumidityNow { get; set; }

        public float WindNow { get; set; }

        public float PressureNow { get; set; }

        public float CloudinessNow { get; set; }

        public string DescriptionWeatherNow { get; set; }

        public string ImageWeatherSourceNow { get; set; }

        public float TempNow { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        private string location;
        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged("Location"); }
        }

        private string date_today;
        public string DateToday
        {
            get { return date_today; }
            set { date_today = value; OnPropertyChanged("DateToday"); }
        }

        public Command LoadItemsCommand { get; set; }

        public Command OnForecastHourly { get; set; }

        public WeatherDaysViewModel()
        {
            Week = new ObservableCollection<Daily>();
            LoadItemsCommand = new Command(async () => await RefreshForecastAsync());
            OnForecastHourly = new Command<Daily>(ForecastHourly);
            _ = GetForecast();
        }
        private async Task GetForecast()
        {
            await GetCoordinates();
            var url = $"https://api.openweathermap.org/data/2.5/onecall?lat={Latitude}&lon={Longitude}&appid=0f5bc762e1e2d34191f752caf96a1e60&units=metric";
            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                try
                {
                    Week.Clear();

                    ValueForecast = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);
                    GetForecastNow();
                    for (int i = 0; i < 7; i++)
                    {
                        Week.Add(ValueForecast.daily[i]);
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Weather Info", ex.Message, "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Weather Info", "No forecast information found", "OK");
            }
        }
        private async Task RefreshForecastAsync()
        {
            IsBusy = true;
            await GetForecast();
            IsBusy = false;

        }
        private void GetForecastNow()
        {

            DescriptionWeatherNow = ValueForecast.daily[0].weather[0].description;
            OnPropertyChanged("DescriptionWeatherNow");
            ImageWeatherSourceNow = ValueForecast.daily[0].weather[0].icon;
            OnPropertyChanged("ImageWeatherSourceNow");
            TempNow = ValueForecast.hourly[3].temp;
            OnPropertyChanged("TempNow");
            WindNow = ValueForecast.daily[0].wind_speed;
            OnPropertyChanged("WindNow");
            HumidityNow = ValueForecast.daily[0].humidity;
            OnPropertyChanged("HumidityNow");
            PressureNow = ValueForecast.daily[0].pressure;
            OnPropertyChanged("PressureNow");
            CloudinessNow = ValueForecast.daily[0].clouds;
            OnPropertyChanged("CloudinessNow");
            DateToday = DateTime.Now.ToString("dd.MM.yyyy");

        }
        private async Task GetCoordinates()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);
                if (location != null)
                {
                    Latitude = location.Latitude;
                    Longitude = location.Longitude;
                    Location = await GetCity(location);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Weather Info", "No coordinates with GPS found", "OK");

                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Coordinates Info", ex.Message, "OK");
            }
        }

        private async Task<string> GetCity(Location location)
        {
            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault();
            if (currentPlace != null)
                return $"{currentPlace.Locality}";
            return null;
        }

        private async void ForecastHourly(Daily daily)
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
