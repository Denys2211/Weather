using System.Collections.ObjectModel;
using Weather.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.ViewModel
{
    public class WeatherPreferencesViewModel : BaseViewModel
    {
        public Command ChoiceCity { get; set; }
        public Command SaveCommand { get; set; }
        public ObservableCollection<CustomerLocation> ListCity { get; set; }
        public Command Delete { get; set; }
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
        string entry_city;
        public string Entry_City
        {
            get { return entry_city; }
            set { SetProperty(ref entry_city, value); }
        }
        string current_city;
        public string Current_City
        {
            get { return current_city;  }
            set { SetProperty(ref current_city, value); }
        }
        public WeatherPreferencesViewModel ()
        {
            Delete = new Command<CustomerLocation>(DeleteCity);
            ChoiceCity = new Command<CustomerLocation>(ActiveCity);
            SaveCommand = new Command(Save);
            ListCity = new ObservableCollection<CustomerLocation>();
            ReadPropertiesApp();

        }
        public bool StatusGetCoordinates
        { 
            get { return Preferences.Get(nameof(StatusGetCoordinates), false); }
            set
            {
                Preferences.Set(nameof(StatusGetCoordinates), value);
                RaisePropertyChanged(nameof(StatusGetCoordinates));
            }
        }
        internal string Serialize()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(ListCity);
        }

        internal void Deserialize(string list)
        {
            ListCity = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<CustomerLocation>>(list);
            
        }
        void ReadPropertiesApp()
        {
            if (Application.Current.Properties.ContainsKey(CITY_LIST_PROP_NAME))
            {
                var list = Application.Current.Properties[CITY_LIST_PROP_NAME].ToString();
                Deserialize(list);
            }
        }
        void SaveToPropertiesApp()
        {
            var list = Serialize();
            Application.Current.Properties[CITY_LIST_PROP_NAME] = list;
        }
        void ActiveCity(CustomerLocation city)
        {
            Index_City = ListCity.IndexOf(city);
            foreach(var item in ListCity)
            {
                item.IsSelected = false;
            }
            Current_City = ListCity[Index_City].Name;
            ListCity[Index_City].IsSelected = true;
            SaveToPropertiesApp();
        }
        private async void Save()
        {
            foreach (var item in ListCity)
            {
                if (Entry_City == item.Name)
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", "City is Exist", "Ok");
                    return;
                }
            }
            
            var location = await ApiGeocoding.GetCoordinatesFromCityName(Entry_City);
            if(location != null)
            {
                ListCity.Insert(0, new CustomerLocation
                {
                    Name = Entry_City,
                    IsSelected = false,
                    Lat = location.Latitude,
                    Lon = location.Longitude
                });
                SaveToPropertiesApp();
                Entry_City = null;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Coordinates Info", "No coordinates received.Please try again", "OK");

            }
        }
        private async void DeleteCity(CustomerLocation city)
        {
            if (city == null)
            {
                return;
            }

            bool result = await Application.Current.MainPage.DisplayAlert($"{city.Name}", $"Do you want to delete an element?", "Yes", "No");
            if (result)
            {
                ListCity.Remove(city);
                SaveToPropertiesApp();
            }
        }
    }
    
}
