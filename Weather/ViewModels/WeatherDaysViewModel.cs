using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weather.Models;
using Weather.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.ViewModels
{
    public class WeatherDaysViewModel : WeatherPreferencesViewModel 
    {
        public ObservableCollection<Daily> Days { get; set; }
        
        public float HumidityNow { get; set; }

        public float WindNow { get; set; }

        public float PressureNow { get; set; }

        public float CloudinessNow { get; set; }

        public string DescriptionWeatherNow { get; set; }

        public string ImageWeatherSourceNow { get; set; }

        public float TempNow { get; set; }
        
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
            Days = new ObservableCollection<Daily>();
            LoadItemsCommand = new Command(async () => await RefreshForecastAsync());
            OnForecastHourly = new Command<Daily>(ForecastHourly);
            _ = GetForecast();
        }
        async Task GetForecast()
        {
            if (StatusGetCoordinates)
            {
                await GetCoordinates();
                
                if (!await CheckExistCityInListAsync(Current_City))
                {
                    Entry_City = Current_City;
                    await Save(Entry_City);
                    await ActiveCityAsync(ListCity[0]);
                }
            }
            else if(ListCity.Count != 0)
            {
                await ActiveCityAsync(ListCity[Index_City]);
            }
            
            var url = $"https://api.openweathermap.org/data/2.5/onecall?lat={Latitude}&lon={Longitude}&appid=0f5bc762e1e2d34191f752caf96a1e60&units=metric";
            var result = await ApiWeather.Get(url);

            if (result.Successful)
            {
                try
                {
                    Days.Clear();

                    ValueForecast = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Weather Info", ex.Message, "OK");
                }

                GetForecastNow();
                for (int i = 0; i < 7; i++)
                {
                    Days.Add(ValueForecast.daily[i]);
                }
                await MapFocusCity(Latitude,Longitude);
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
            try
            {
                await GetForecast();
            }
            finally
            {
                IsBusy = false;
            }

        }
        async void GetForecastNow()
        {
            try
            {
                DescriptionWeatherNow = ValueForecast.daily[0].weather[0].description;
                RaisePropertyChanged(nameof(DescriptionWeatherNow));
                ImageWeatherSourceNow = ValueForecast.hourly[3].weather[0].icon;
                RaisePropertyChanged(nameof(ImageWeatherSourceNow));
                TempNow = ValueForecast.hourly[3].temp;
                RaisePropertyChanged(nameof(TempNow));
                WindNow = ValueForecast.daily[0].wind_speed;
                RaisePropertyChanged(nameof(WindNow));
                HumidityNow = ValueForecast.daily[0].humidity;
                RaisePropertyChanged(nameof(HumidityNow));
                PressureNow = ValueForecast.daily[0].pressure;
                RaisePropertyChanged(nameof(PressureNow));
                CloudinessNow = ValueForecast.daily[0].clouds;
                RaisePropertyChanged(nameof(CloudinessNow));
                DateToday = DateTime.Now.ToString();
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", ex.Message, "OK");

            }


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
