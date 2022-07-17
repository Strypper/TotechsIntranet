using IntranetUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace IntranetUWP.Converters
{
    public class UserListToTargetUserProfilePicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var users = (List<UserDTO>)value;
            return (users.Count == 2 && users.ElementAt(1) != null) ? new BitmapImage(new Uri(users.Where(user => user.Guid != App.localSettings.Values["UserGuid"].ToString()).FirstOrDefault().ProfilePic)) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class UserListToTargetUserNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var users = (List<UserDTO>)value;
            var targetUser = users.Where(user => user.Guid != App.localSettings.Values["UserGuid"].ToString()).FirstOrDefault();
            return targetUser.FullName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ChatMessageToLastMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var chatMessages = (List<ChatMessageDTO>)value;
            return chatMessages.Count == 0 
                    ? String.Empty 
                    : chatMessages.LastOrDefault() == null 
                    ? String.Empty 
                    : chatMessages.LastOrDefault().MessageContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class SmartDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dateTime = (DateTime)value;
            if (dateTime != null)
            {
                if (dateTime.Date == DateTime.Now.Date)
                {
                    return dateTime.ToString("HH:mm");
                }
                else return dateTime.Date.ToString("MM/dd/yyyy");
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
