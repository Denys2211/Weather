using System;
using Xamarin.Forms;
using System.ComponentModel;
using Weather.ViewModels;
using System.Linq;
using Weather.Services.VibrationService;
using Weather.CustomControls;
using Xamarin.Essentials;

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
        }

        private void _viewModel_OnCityAdded(object sender, EventArgs e)
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
            _viewModel.ListCity.CollectionChanged += ItemsCityControl.OnItemsChanged;
            SideMenu.OnGestureStarted += SideMenu_OnGestureFinished;
            _viewModel.OnCityAdded += _viewModel_OnCityAdded;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            _viewModel.ListCity.CollectionChanged -= ItemsCityControl.OnItemsChanged;
            SideMenu.OnGestureStarted -= SideMenu_OnGestureFinished;
            _viewModel.OnCityAdded -= _viewModel_OnCityAdded;
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

        private void CustomScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            var scrollView = sender as ScrollView;
            var contentSize = scrollView.ContentSize.Height;

            var maxPos = contentSize - scrollView.Height;

            if (Convert.ToInt16(e.ScrollY) >= Convert.ToInt16(maxPos))
            {
                Content.InputTransparent = false;
            }
            else if (Convert.ToInt16(e.ScrollY) == 0)
            {
                Content.InputTransparent = true;
            }
        }
    }
}
