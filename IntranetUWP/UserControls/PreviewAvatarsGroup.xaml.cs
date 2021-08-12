using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.UserControls.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class PreviewAvatarsGroup : UserControl
    {
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public string getUsersDataUrl = "User/GetAll";
        public ObservableCollection<UserDTO> Members
        {
            get { return (ObservableCollection<UserDTO>)GetValue(MembersProperty); }
            set { SetValue(MembersProperty, value); }
        }

        public static readonly DependencyProperty MembersProperty =
            DependencyProperty.Register("Members", 
                typeof(ObservableCollection<UserDTO>), 
                typeof(PreviewAvatarsGroup), 
                new PropertyMetadata(new ObservableCollection<UserDTO>()));



        public PreviewAvatarsGroup()
        {
            this.InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (StackLayout.Children.Count == 0)
            {
                FlyoutList.ItemsSource = Members.Skip(5);
                foreach (var user in Members.Take(5))
                {
                    var userAvatar = new PersonPicture();
                    userAvatar.ProfilePicture = new BitmapImage(new Uri(user.profilePic));
                    userAvatar.Width = 40;
                    StackLayout.Children.Add(userAvatar);
                }
            }
        }

        private async void AddMember_Click(object sender, RoutedEventArgs e)
        {
            if(VisualTreeHelper.GetOpenPopups(Window.Current).Count == 1)
            {
                var peoplePickerDialog = new PeoplePickerContentDialog();
                //This should be the whole user Lists
                peoplePickerDialog.UsersList = await httpHelper.GetAsync<ObservableCollection<UserDTO>>(getUsersDataUrl);
                await peoplePickerDialog.ShowAsync();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Can't open more than one Dialog");
            }
        }
    }
}
