using System;
using Xamarin.Forms;
using System.ComponentModel;
using Weather.ViewModels;
using System.Linq;

namespace Weather.Views
{
    [DesignTimeVisible(false)]
    public partial class WeatherDaysPage : ContentPage
    {
        public WeatherDaysPage()
        {
            InitializeComponent();
            SideMenu.OnGestureStarted += SideMenu_OnGestureFinished;
        }

        private void SideMenu_OnGestureFinished(object sender, EventArgs e)
        {
            WeatherPreferencesViewModel viewModel = BindingContext as WeatherPreferencesViewModel;

            if (viewModel.ListCity != null)
            {
                var item = viewModel.ListCity.FirstOrDefault(t => t.IsSelected);
                if (item != null)
                {
                    ListCityItems.ScrollTo(viewModel.ListCity.IndexOf(item), -1, ScrollToPosition.Center, false);
                }
            }
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
  