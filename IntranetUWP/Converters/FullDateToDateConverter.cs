using System;
using Windows.UI.Xaml.Data;

namespace IntranetUWP.Converters
{
    public class FullDateToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var birthDay = (DateTime?)value;
            return birthDay?.Date.ToString("MM/dd/yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
