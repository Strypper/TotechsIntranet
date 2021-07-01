using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using Windows.Media.Core;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        Compositor compositor = Window.Current.Compositor;
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ProfileImage.Source = new BitmapImage(
                                  new Uri(App.localSettings.Values["ProfilePic"]
                                  as string)
                                  );

            //VideoBackGround.MediaPlayer.IsLoopingEnabled = true;

            var currentTheme = Application.Current.RequestedTheme;
            AdaptiveTheme(currentTheme);

            //Detect theme change
            var Listener = new ThemeListener();
            Listener.ThemeChanged += Listener_ThemeChanged;
        }

        private void Listener_ThemeChanged(ThemeListener sender)
        {
            var theme = sender.CurrentTheme;
            AdaptiveTheme(theme);
        }

        private void AdaptiveTheme(ApplicationTheme theme)
        {
            switch (theme)
            {
                case ApplicationTheme.Dark:
                    //VideoBackGround.Source = MediaSource.CreateFromUri(new Uri("https://intranetblobstorages.blob.core.windows.net/backgroundvideo/ProfileVideo.mp4"));
                    break;
                case ApplicationTheme.Light:
                    //VideoBackGround.Source = MediaSource.CreateFromUri(new Uri("https://intranetblobstorages.blob.core.windows.net/backgroundvideo/SnowVideo.mp4"));
                    break;
                default:
                    break;
            }
        }
    }
}
