using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Weather.Models;
using Weather.Services.SnackBarSevice;
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

        string entry_city;
        public string Entry_City
        {
            get { return entry_city; }
            set { SetProperty(ref entry_city, value); }
        }
        string current_city;
        public string Current_City
        {
            get 
            {
                if(ListCity != null && ListCity.Count > 0 && current_city == null)
                {
                    return ListCity.Where(c => c.IsSelected).FirstOrDefault().Name;
                }

                return current_city; 
            }
            set
            { 
                SetProperty(ref current_city, value);
            }
        }

        public event EventHandler OnCityAdded;

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

                if(value)
                {
                    DependencyService.Get<ISnackBarService>()?.ShowSnackBar("GPS ON!!!");
                }
                else
                {
                    DependencyService.Get<ISnackBarService>()?.ShowSnackBar("GPS OFF!!!");
                }
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
        protected async Task ActiveCityAsync(CustomerLocation city = null)
        {
            if (city != null)
            {
                Current_City = city.Name;
            }

            if(Current_City == null)
            {
                DependencyService.Get<ISnackBarService>()?.ShowSnackBar("Current city is empty!!!");
                return;
            }

            var index_City = ListCity.IndexOf(ListCity.FirstOrDefault(c => c.Name == Current_City));

            bool isMaping = Latitude == ListCity[index_City].Lat && Longitude == ListCity[index_City].Lon;

            foreach (var item in ListCity)
            {
                item.IsSelected = false;
            }

            if(StatusGetCoordinates)
               ListCity.Move(index_City, index_City);

            Latitude = ListCity[index_City].Lat;
            Longitude = ListCity[index_City].Lon;
            Current_City = ListCity[index_City].Name;
            ListCity[index_City].IsSelected = true;

            if(!isMaping)
               await MapFocusCity(Latitude, Longitude);

            SaveToPropertiesApp();
        }

        protected async Task<bool> CheckExistCityInListAsync(string city)
        {
            foreach (var item in ListCity)
            {
                if (item.Name == city)
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", "City is Exist", "Ok");
                    return true;
                }
            }

            return false;
        }

        protected async Task Save()
        {
            try
            {
                var existCity = ListCity.FirstOrDefault(c => c.Name == Current_City);


                if (existCity != null)
                {
                    await ActiveCityAsync(existCity);
                }
                else
                {
                    ListCity.Insert(0, new CustomerLocation
                    {
                        Name = Current_City,
                        IsSelected = true,
                        Lat = Latitude,
                        Lon = Longitude
                    });

                    SaveToPropertiesApp();
                }

                OnCityAdded?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                Entry_City = string.Empty;
            }

        }

        protected async Task Save(string city)
        {
            try
            {
                if (await CheckExistCityInListAsync(city))
                {
                    return;
                }

                var location = await ApiGeocoding.GetCoordinatesFromCityName(city);
                if (location != null)
                {
                    ListCity.Insert(0, new CustomerLocation
                    {
                        Name = Current_City = city,
                        IsSelected = true,
                        Lat = Latitude = location.Latitude,
                        Lon = Longitude = location.Longitude
                    });

                    SaveToPropertiesApp();
                    await MapFocusCity(Latitude, Longitude);
                }

                OnCityAdded?.Invoke(this, EventArgs.Empty);
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
