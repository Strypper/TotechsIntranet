using IntranetUWP.Models;
using Syncfusion.UI.Xaml.Charts;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace IntranetUWP.Converters
{
    public class ChartPieAdornmentToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ChartPieAdornment adornment = value as ChartPieAdornment;
            return (adornment.Item as ContributionDTO).Contributor.FullName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ChartPieAdornmentToPersonPictureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ChartPieAdornment adornment = value as ChartPieAdornment;
            return new BitmapImage(new Uri((adornment.Item as ContributionDTO).Contributor.ProfilePic));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
