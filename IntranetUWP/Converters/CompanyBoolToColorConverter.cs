using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace IntranetUWP.Converters
{
    public class CompanyBoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var company = (bool)value;
            return company == true 
                ? new SolidColorBrush(Color.FromArgb(255, 237, 47, 53)) 
                : new SolidColorBrush(Color.FromArgb(255, 20, 82, 118));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

