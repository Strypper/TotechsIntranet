using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace IntranetUWP.Converters
{
    public class BoolToCompanyImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var company = (bool)value;
            return new BitmapImage(new Uri(company == true
                ? "ms-appx:///Assets/iDealogic.png"
                : "ms-appx:///Assets/Devinition.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
