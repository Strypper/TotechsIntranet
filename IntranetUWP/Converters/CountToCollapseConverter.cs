﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace IntranetUWP.Converters
{
    class CountToCollapseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var count = (int)value;
            return count != 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
