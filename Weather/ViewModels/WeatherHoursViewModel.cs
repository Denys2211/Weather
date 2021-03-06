using System.Collections.ObjectModel;
using Weather.Models;
using Xamarin.Forms;

namespace Weather.ViewModels
{
    public class WeatherHoursViewModel : BaseViewModel
    {
        public ObservableCollection<Hourly> Day { get; set; }

        public WeatherHoursViewModel()
        {
            Day = new ObservableCollection<Hourly>();
            GetForecastHours();
        }
        private async void GetForecastHours()
        {
            if(Hours <= 48)
            {
                for (int i = 0; i < Hours; i++)
                {
                    if (Hours - i <= 24)
                        Day.Add(ValueForecast.hourly[i]);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Weather Info", "No forecast information found", "OK");
            }

        }
    }
}
