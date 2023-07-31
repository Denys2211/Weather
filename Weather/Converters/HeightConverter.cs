using System;
using System.Globalization;
using Xamarin.Forms;

namespace Weather.Converters
{
    public class HeightConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var PageHeight = (double)value;

            if(PageHeight > 0)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    return PageHeight - 170;
                }
                else
                {
                    return PageHeight - 120;
                }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
