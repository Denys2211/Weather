using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Weather.Services;
using Weather.Models;
using Weather.ViewModel;
using Xamarin.Forms;
using System.ComponentModel;

namespace Weather.View
{
    [DesignTimeVisible(false)]
    public partial class WeatherDaysPage : ContentPage
    {
        public WeatherDaysPage()
        {
            InitializeComponent();
        }

    }
}
  