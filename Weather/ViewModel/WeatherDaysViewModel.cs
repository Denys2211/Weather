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
        public ObservableCollection<Days> Week { get; set; }

        readonly Days[] days;

        public Command LoadItemsCommand { get; set; }

        public WeatherDaysViewModel()
        {
            days = new Days[7];
            Week = new ObservableCollection<Days>();
            LoadItemsCommand = new Command(() => GetForecast());
        }

        private string Lviv = "lat=49.839683&lon=24.029717";

        private async void GetForecast()
        {
            var url = $"https://api.openweathermap.org/data/2.5/onecall?{Lviv}&appid=16114e1240ecc9c8df14d1a2df3864df&units=metric";
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
                        days[i] = new Days();
                        days[i].Name = UnixTimeStampToDateTime(forcastInfo.daily[i].dt).ToString("dddd");
                        days[i].Date = UnixTimeStampToDateTime(forcastInfo.daily[i].dt).ToString("dd MMM");
                        days[i].Temp = forcastInfo.daily[i].temp.max.ToString("0");
                        Week.Add(days[i]);
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
