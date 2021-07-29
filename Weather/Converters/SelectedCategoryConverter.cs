using System;
using System.Globalization;
using Xamarin.Forms;

namespace Weather.Converters
{
    public class SelectedCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var categorySelected = value is bool ? (bool)value : false;
            return categorySelected ? (Color)Color.Yellow : (Color)Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var categorySelected = value is bool ? (bool)value : false;
            return categorySelected ? (Color)Color.Yellow : (Color)Color.White;

        }
    }
}
