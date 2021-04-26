using System;
using Windows.UI.Xaml.Data;

namespace IntranetUWP.Converters
{
    public class BoolToCompanyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var company= (bool)value;
            return company == true ? "iDealogic" : "Devinition";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
