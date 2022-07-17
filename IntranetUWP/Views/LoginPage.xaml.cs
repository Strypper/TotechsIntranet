using IntranetUWP.ViewModels.PagesViewModel;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IntranetUWP.Views
{
	public sealed partial class LoginPage : Page
	{
		private LoginPageViewModel ViewModel { get; }
		public LoginPage()
		{
			InitializeComponent();
			ViewModel = new LoginPageViewModel();
		}

		private async void NavigateMainFrameButton_Click(object sender, RoutedEventArgs args)
		{
			// avoid double login attempt, disable login button
			var button = (Control)sender;
			button.IsEnabled = false;

			try
			{
				var userInfo = await ViewModel.LoginAsync();
				_ = Frame.Navigate(typeof(MainPage), userInfo);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}
			finally
			{
				button.IsEnabled = true;
			}
		}

        private async void PasswordEnter_Click(Windows.UI.Xaml.Input.KeyboardAccelerator sender, Windows.UI.Xaml.Input.KeyboardAcceleratorInvokedEventArgs args)
        {
			NavigateMainFrameButton.IsEnabled = false;
			try
			{
				var identityUserInfo = await ViewModel.LoginAsync();
				Frame.Navigate(typeof(MainPage), identityUserInfo);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}
			finally
			{
				NavigateMainFrameButton.IsEnabled = true;
			}
		}
    }
}
