using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Weather.Helper;
using Weather.Models;
using Xamarin.Forms;

namespace Weather.ViewModel
{
    public class WeatherViewModel : BaseViewModel
    {
        public ObservableCollection<Days> Week { get; set; }

        public Command LoadItemsCommand { get; set; }

        public WeatherViewModel()
        {
            GetForecast();
            LoadItemsCommand = new Command(() => GetForecast());
            Week = new ObservableCollection<Days>();
        }

        private string Lviv = "lat=49.50&lon=-24.00";

        private async void GetForecast()
        {
            var url = $"https://api.openweathermap.org/data/2.5/onecall?{Lviv}&appid=16114e1240ecc9c8df14d1a2df3864df&units=metric";
            var result = await ApiCaller.Get(url);

            if (result.Successful)
            {
                Week.Clear();
                IsBusy = true;
                try
                {
                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);

                    Days oneDay = new Days
                    {
                        Name = UnixTimeStampToDateTime(forcastInfo.daily[0].dt).ToString("dddd"),
                        Date = UnixTimeStampToDateTime(forcastInfo.daily[0].dt).ToString("dd MMM"),
                        Temp = forcastInfo.daily[0].temp.day.ToString("0")
                    }; Week.Add(oneDay);
                    Days twoDay = new Days
                    {
                        Name = UnixTimeStampToDateTime(forcastInfo.daily[1].dt).ToString("dddd"),
                        Date = UnixTimeStampToDateTime(forcastInfo.daily[1].dt).ToString("dd MMM"),
                        Temp = forcastInfo.daily[1].temp.day.ToString("0")
                    }; Week.Add(twoDay);
                    Days threeDay = new Days
                    {
                        Name = UnixTimeStampToDateTime(forcastInfo.daily[2].dt).ToString("dddd"),
                        Date = UnixTimeStampToDateTime(forcastInfo.daily[2].dt).ToString("dd MMM"),
                        Temp = forcastInfo.daily[2].temp.day.ToString("0")
                    }; Week.Add(threeDay);
                    Days fourDay = new Days
                    {
                        Name = UnixTimeStampToDateTime(forcastInfo.daily[3].dt).ToString("dddd"),
                        Date = UnixTimeStampToDateTime(forcastInfo.daily[3].dt).ToString("dd MMM"),
                        Temp = forcastInfo.daily[3].temp.day.ToString("0")
                    }; Week.Add(fourDay);
                    Days fiveDay = new Days
                    {
                        Name = UnixTimeStampToDateTime(forcastInfo.daily[4].dt).ToString("dddd"),
                        Date = UnixTimeStampToDateTime(forcastInfo.daily[4].dt).ToString("dd MMM"),
                        Temp = forcastInfo.daily[4].temp.day.ToString("0")
                    }; Week.Add(fiveDay);
                    Days sixDay = new Days
                    {
                        Name = UnixTimeStampToDateTime(forcastInfo.daily[5].dt).ToString("dddd"),
                        Date = UnixTimeStampToDateTime(forcastInfo.daily[5].dt).ToString("dd MMM"),
                        Temp = forcastInfo.daily[5].temp.day.ToString("0")
                    }; Week.Add(sixDay);
                    Days sevenDay = new Days
                    {
                        Name = UnixTimeStampToDateTime(forcastInfo.daily[6].dt).ToString("dddd"),
                        Date = UnixTimeStampToDateTime(forcastInfo.daily[6].dt).ToString("dd MMM"),
                        Temp = forcastInfo.daily[6].temp.day.ToString("0")
                    }; Week.Add(sevenDay);

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
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
