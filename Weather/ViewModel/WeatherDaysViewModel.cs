using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weather.Models;
using Weather.View;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.ViewModel
{
    public class WeatherDaysViewModel : WeatherPreferencesViewModel 
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
        
        string date_today;
        public string DateToday
        {
            get { return date_today; }
            set { SetProperty(ref date_today, value); }
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
        async Task GetForecast()
        {
            if (StatusGetCoordinates)
                await GetCoordinates();
            else if(ListCity.Count != 0)
            {
                Latitude = ListCity[Index_City].coord.lat; 
                Longitude = ListCity[Index_City].coord.lon;
                Current_City = ListCity[Index_City].name;
            }

            var url = $"https://api.openweathermap.org/data/2.5/onecall?lat={Latitude}&lon={Longitude}&appid=0f5bc762e1e2d34191f752caf96a1e60&units=metric";
            var result = await ApiWeather.Get(url);

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
        async Task GetCoordinates()
        {
            Location location = await ApiGeocoding.GetLocationGPS();
            if (location != null)
            {
                Latitude = location.Latitude;
                Longitude = location.Longitude;
                Current_City = await ApiGeocoding.GetCity(location);
            }
        }
        async Task RefreshForecastAsync()
        {
            IsBusy = true;
            await GetForecast();
            IsBusy = false;

        }
        void GetForecastNow()
        {

            DescriptionWeatherNow = ValueForecast.daily[0].weather[0].description;
            OnPropertyChanged(nameof(DescriptionWeatherNow));
            ImageWeatherSourceNow = ValueForecast.daily[0].weather[0].icon;
            OnPropertyChanged(nameof(ImageWeatherSourceNow));
            TempNow = ValueForecast.hourly[3].temp;
            OnPropertyChanged(nameof(TempNow));
            WindNow = ValueForecast.daily[0].wind_speed;
            OnPropertyChanged(nameof(WindNow));
            HumidityNow = ValueForecast.daily[0].humidity;
            OnPropertyChanged(nameof(HumidityNow));
            PressureNow = ValueForecast.daily[0].pressure;
            OnPropertyChanged(nameof(PressureNow));
            CloudinessNow = ValueForecast.daily[0].clouds;
            OnPropertyChanged(nameof(CloudinessNow));
            DateToday = DateTime.Now.ToString("dd.MM.yyyy");

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
