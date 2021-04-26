using System;
using Windows.UI.Xaml.Data;

namespace IntranetUWP.Converters
{
    public class DoubleToPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var number = (double)value;
            return number.ToString("0.##\\%");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
