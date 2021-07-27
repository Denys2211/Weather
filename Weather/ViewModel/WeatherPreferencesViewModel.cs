using System;
using System.Collections.ObjectModel;
using Weather.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.ViewModel
{
    public class WeatherPreferencesViewModel : BaseViewModel
    {
        public Command SaveCommand { get; set; }
        public ObservableCollection<City> ListCity;
        const string CITY_LIST_PROP_NAME = "cities";
        public WeatherPreferencesViewModel ()
        {
            SaveCommand = new Command(Save);
            ListCity = new ObservableCollection<City>();
        }
        public bool StatusGetCoordinates
        { 
            get { return Preferences.Get(nameof(StatusGetCoordinates), false); }
            set
            {
                Preferences.Set(nameof(StatusGetCoordinates), value);
                OnPropertyChanged(nameof(StatusGetCoordinates));
            }
        }
        internal string Serialize()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(ListCity);
        }

        internal void Deserialise(string list)
        {
            ListCity = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<City>>(list);
        }
        private void Save()
        {
            ListCity.Add(new City
            {
                name = "Lviv",
                coord = new Coord { lat = 49.8382600f, lon = 24.0232400f }
            });
                var list = Serialize();
                Application.Current.Properties[CITY_LIST_PROP_NAME] = list;
        }
    }
    
}
