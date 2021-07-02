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
				await ViewModel.LoginAsync();
				_ = Frame.Navigate(typeof(MainPage));
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
	}
}
