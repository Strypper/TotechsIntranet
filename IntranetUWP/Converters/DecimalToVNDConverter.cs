using System;
using Windows.UI.Xaml.Data;

namespace IntranetUWP.Converters
{
    public class DecimalToVNDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var money = (decimal)value;
            return (money * 1000).ToString("#,###");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
