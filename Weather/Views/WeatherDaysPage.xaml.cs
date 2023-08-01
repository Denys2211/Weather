using System;
using Xamarin.Forms;
using System.ComponentModel;
using Weather.ViewModels;
using System.Linq;
using Weather.Services.VibrationService;
using Weather.CustomControls;
using System.Collections.Specialized;

namespace Weather.Views
{
    [DesignTimeVisible(false)]
    public partial class WeatherDaysPage : ContentPage
    {
        private WeatherPreferencesViewModel _viewModel;

        public WeatherDaysPage()
        {
            InitializeComponent();

            _viewModel = BindingContext as WeatherPreferencesViewModel;
            _viewModel.ListCity.CollectionChanged += ItemsSityControl.OnItemsChanged;
        }

        private void _viewModel_OnSityAdded(object sender, EventArgs e)
        {
            _viewModel = BindingContext as WeatherPreferencesViewModel;

            if (_viewModel.ListCity != null)
            {
                var item = _viewModel.ListCity[0];
                if (item != null)
                {
                    ListCityItems.ScrollTo(_viewModel.ListCity.IndexOf(item), -1, ScrollToPosition.Center, false);
                }
            }
        }

        protected override void OnAppearing()
        {
            _viewModel.ListCity.CollectionChanged += ItemsSityControl.OnItemsChanged;
            SideMenu.OnGestureStarted += SideMenu_OnGestureFinished;
            _viewModel.OnSityAdded += _viewModel_OnSityAdded;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _viewModel.ListCity.CollectionChanged -= ItemsSityControl.OnItemsChanged;
            SideMenu.OnGestureStarted -= SideMenu_OnGestureFinished;
            _viewModel.OnSityAdded -= _viewModel_OnSityAdded;
            base.OnDisappearing();
        }

        private void SideMenu_OnGestureFinished(object sender, EventArgs e)
        {
            _viewModel = BindingContext as WeatherPreferencesViewModel;

            if (_viewModel.ListCity != null)
            {
                var item = _viewModel.ListCity.FirstOrDefault(t => t.IsSelected);
                if (item != null)
                {
                    ListCityItems.ScrollTo(_viewModel.ListCity.IndexOf(item), -1, ScrollToPosition.Center, false);
                }
            }

            DependencyService.Get<IVibrator>()?.Vibrate();
        }

        public void OpenSwipe(object sender, EventArgs e)
        {
            if (SideMenu.State == CustomControls.SideMenu.SideMenuViewState.LeftMenuShown)
            {
                SideMenu.State = CustomControls.SideMenu.SideMenuViewState.Default;
            }
            else
            {
                SideMenu.State = CustomControls.SideMenu.SideMenuViewState.LeftMenuShown;
            }
        }
    }
}
