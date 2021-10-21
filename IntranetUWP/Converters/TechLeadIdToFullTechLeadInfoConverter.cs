using IntranetUWP.Helpers;
using IntranetUWP.Models;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace IntranetUWP.Converters
{
    public class TechLeadIdToFullTechLeadInfoConverter : IValueConverter
    {
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public string getUserById = "User/Get";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var user = httpHelper.GetByIdAsync<UserDTO>(getUserById, (int)value).GetAwaiter().GetResult();
            return user.profilePic;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
