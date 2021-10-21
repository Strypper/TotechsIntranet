using IntranetUWP.Views;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace IntranetUWP 
{ 
    public sealed partial class MainPage : Page
    {
        public string ProfileImage { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            TheMainFrame.Navigate(typeof(FoodOrderPage));
            if(App.localSettings.Values["ProfilePic"] != null)
            {
                ProfileImage = App.localSettings.Values["ProfilePic"] as string;
            }
            if (App.localSettings.Values["FirstName"] != null)
            {
                PersonalName.Text = App.localSettings.Values["FirstName"] as String 
                                 + (App.localSettings.Values["LastName"] != null 
                                 ? (" " +  App.localSettings.Values["LastName"] as String) 
                                 : " ");
            }
        }

        private void NavigationViewPanel_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            Microsoft.UI.Xaml.Controls.NavigationViewItem item = args.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;
            NavView_Navigate(item);
        }

        private void NavView_Navigate(Microsoft.UI.Xaml.Controls.NavigationViewItem item)
        {
            switch (item.Name)
            {
                case "LunchOrder":
                    TheMainFrame.Navigate(typeof(FoodOrderPage));
                    break;
                case "TeaBreak":
                    TheMainFrame.Navigate(typeof(TeaBreakPage));
                    break;
                case "PlayTime":
                    TheMainFrame.Navigate(typeof(PlayTimePage));
                    break;
                case "ChatHub":
                    TheMainFrame.Navigate(typeof(ChatHubPage));
                    break;
                case "Member":
                    TheMainFrame.Navigate(typeof(MemberPage));
                    break;
                case "SettingsItem":
                    TheMainFrame.Navigate(typeof(SettingsPage));
                    break;
                case "Profile":
                    TheMainFrame.Navigate(typeof(ProfilePage));
                    break;
                default:
                    TheMainFrame.Navigate(typeof(FoodOrderPage));
                    break;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            App.localSettings.Values.Clear();
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
