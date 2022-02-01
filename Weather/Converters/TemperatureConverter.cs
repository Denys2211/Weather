using System;
using System.Globalization;
using Xamarin.Forms;

namespace Weather.Converters
{
    public class TemperatureConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temperature = ((float)value).ToString();
            var splittemperature = temperature.Split('.');

            return splittemperature[0]+ "°";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
