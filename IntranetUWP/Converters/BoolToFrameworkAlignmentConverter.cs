using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IntranetUWP.Converters
{
    public class BoolToFrameworkAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var alignment = (bool)value;
            return alignment == true ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
