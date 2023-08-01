using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Weather.Models;
using Weather.Services.VibrationService;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Weather.ViewModels
{
    public class WeatherPreferencesViewModel : BaseViewModel
    {
        public Xamarin.Forms.Maps.Map Map { get; protected set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
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
            get { return current_city; }
            set { SetProperty(ref current_city, value); }
        }

        public event EventHandler OnSityAdded;

        public WeatherPreferencesViewModel()
        {
            Map = new Xamarin.Forms.Maps.Map
            {
                IsEnabled = true,
                HasScrollEnabled = true,
                HasZoomEnabled = true,
                IsVisible = true,
                MapType = MapType.Street,
                HeightRequest = 380,
                WidthRequest = 150,
                MoveToLastRegionOnLayoutChange = false
            };
            Delete = new Command<CustomerLocation>(DeleteCity);
            ChoiceCity = new Command<CustomerLocation>(async (o) => await ActiveCityAsync(o));
            SaveCommand = new Command(async () => await Save(Entry_City));
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
        protected Task MapFocusCity(double lat, double lon)
        {
            Map.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        new Position(lat, lon), Distance.FromMiles(5)));
            return Task.FromResult(true);
        }
        protected async Task ActiveCityAsync(CustomerLocation city)
        {
            Index_City = ListCity.IndexOf(city);
            foreach (var item in ListCity)
            {
                item.IsSelected = false;
            }
            Latitude = ListCity[Index_City].Lat;
            Longitude = ListCity[Index_City].Lon;
            Current_City = ListCity[Index_City].Name;
            ListCity[Index_City].IsSelected = true;
            await MapFocusCity(Latitude, Longitude);
            SaveToPropertiesApp();
        }
        protected async Task<bool> CheckExistCityInListAsync(string city)
        {
            bool check = false;

            foreach (var item in ListCity)
            {
                item.IsSelected = false;
                if (item.Name == city)
                {
                    await ActiveCityAsync(item);
                    check = true;
                }
            }
            return check;
        }
        protected async Task Save(string city)
        {
            try
            {
                foreach (var item in ListCity)
                {
                    if (city == item.Name)
                    {
                        await Application.Current.MainPage.DisplayAlert("Notification", "City is Exist", "Ok");
                        return;
                    }
                }

                var location = await ApiGeocoding.GetCoordinatesFromCityName(city);
                if (location != null)
                {
                    ListCity.Insert(0, new CustomerLocation
                    {
                        Name = city,
                        IsSelected = false,
                        Lat = location.Latitude,
                        Lon = location.Longitude
                    });

                    SaveToPropertiesApp();
                    await ActiveCityAsync(ListCity[0]);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Coordinates Info", "No coordinates received.Please try again", "OK");

                }

                OnSityAdded?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                Entry_City = string.Empty;
            }

        }
        private async void DeleteCity(CustomerLocation city)
        {
            DependencyService.Get<IVibrator>()?.Vibrate();

            if (city != null)
            {
                bool result = await Application.Current.MainPage.DisplayAlert($"{city.Name}", $"Do you want to delete an element?", "Yes", "No");
                if (result)
                {
                    ListCity.Remove(city);
                    SaveToPropertiesApp();
                }

            }
        }
    }

}
