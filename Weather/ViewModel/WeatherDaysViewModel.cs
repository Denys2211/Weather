using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Weather.Helper;
using Weather.Models;
using Xamarin.Forms;

namespace Weather.ViewModel
{
    public class WeatherDaysViewModel : BaseViewModel
    {
        public ObservableCollection<Daily> Week { get; set; }

        public Command LoadItemsCommand { get; set; }

        public WeatherDaysViewModel()
        {
            CityCoord = "lat=49.839683&lon=24.029717";
            Week = new ObservableCollection<Daily>();
            LoadItemsCommand = new Command(() => GetForecastDays());
            IsBusy = true;
        }
        private async void GetForecastDays()
        {
            var url = $"https://api.openweathermap.org/data/2.5/onecall?{CityCoord}&appid=16114e1240ecc9c8df14d1a2df3864df&units=metric";
            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                IsBusy = true;
                try
                {
                    Week.Clear();

                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    for (int i = 0; i < 7; i++)
                    {
                        Week.Add(forcastInfo.daily[i]);
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Weather Info", ex.Message, "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Weather Info", "No forecast information found", "OK");
            }
        }
        
    }
}
