using System;
using System.Collections.ObjectModel;
using Weather.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.ViewModel
{
    public class WeatherPreferencesViewModel : BaseViewModel
    {
        bool _colorCurrentCity;
        public bool ColorCurrentCity
        {
            get { return Preferences.Get(nameof(ColorCurrentCity), false); }
            set
            {
                SetProperty(ref _colorCurrentCity, value);
                Preferences.Set(nameof(ColorCurrentCity), value);
            }
        }
        public Command ChoiceCity { get; set; }
        public Command SaveCommand { get; set; }
        public ObservableCollection<City> ListCity { get; set; }
        const string CITY_LIST_PROP_NAME = "cities";
        int index_city;
        public int Index_City
        {
            get { return Preferences.Get(nameof(Index_City), 0); }
            set
            {
                SetProperty(ref index_city, value);
                Preferences.Set(nameof(Index_City), value);
            }
        }
        string current_city;
        public string Current_City
        {
            get { return current_city;  }
            set { SetProperty(ref current_city, value); }
        }
        public WeatherPreferencesViewModel ()
        {
            ChoiceCity = new Command<City>(ActiveCity);
            SaveCommand = new Command(Save);
            ListCity = new ObservableCollection<City>();
            if (Application.Current.Properties.ContainsKey(CITY_LIST_PROP_NAME))
            {
                var list = Application.Current.Properties[CITY_LIST_PROP_NAME].ToString();
                Deserialize(list);
            }
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

        internal void Deserialize(string list)
        {
            ListCity = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<City>>(list);
            
        }
        void ActiveCity(City city)
        {
            Index_City = ListCity.Count - city.id - 1;
            Current_City = ListCity[Index_City].name;
        }
        private async void Save()
        {
            foreach (var item in ListCity)
            {
                if (Current_City == item.name)
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", "City is Exist", "Ok");
                    return;
                }
            }
            var location = await ApiGeocoding.GetCoordinatesFromCityName(Current_City);
            if(location != null)
            {
                ListCity.Insert(0, new City
                {
                    id = ListCity.Count,
                    name = Current_City,
                    iscolor = false,
                    coord = new Coord
                    {
                        lat = Convert.ToSingle(location.Latitude),
                        lon = Convert.ToSingle(location.Longitude)
                    }
                });
                var list = Serialize();
                Application.Current.Properties[CITY_LIST_PROP_NAME] = list;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Coordinates Info", "No coordinates received.Please try again", "OK");

            }

        }
    }
    
}
