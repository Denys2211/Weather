using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Weather.Models;
using Weather.Services;
using Xamarin.Forms;

namespace Weather.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public ApiCallerWeather ApiWeather = DependencyService.Get<ApiCallerWeather>();
        public ApiCallerGeocoding ApiGeocoding = DependencyService.Get<ApiCallerGeocoding>();
        protected static ForecastInfo ValueForecast { get; set; }
        protected static int Hours { get; set; }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion
    }
}
