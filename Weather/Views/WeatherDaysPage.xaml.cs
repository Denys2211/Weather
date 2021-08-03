using System;
using Xamarin.Forms;
using System.ComponentModel;

namespace Weather.Views
{
    [DesignTimeVisible(false)]
    public partial class WeatherDaysPage : ContentPage
    {
        public WeatherDaysPage()
        {
            InitializeComponent();
        }
        public void OpenSwipe(object sender, EventArgs e)
        {
            MainSwipeView.Open(OpenSwipeItem.LeftItems);
        }
    }
}
  