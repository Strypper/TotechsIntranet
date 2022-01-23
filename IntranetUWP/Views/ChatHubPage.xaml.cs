using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using IntranetUWP.Views.MemberChildPages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.Diagnostics;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatHubPage : Page
    {
        public ChatHubPageViewModel vm { get; set; }
        public ChatHubPage()
        {
            this.InitializeComponent();
            var connection = new HubConnectionBuilder()
                    //.WithUrl("https://intranetapi.azurewebsites.net/chathub").Build();
                    .WithUrl("https://localhost:44371/chathub", options =>
                    {
                        options.HttpMessageHandlerFactory = (handler) =>
                        {
                            if (handler is HttpClientHandler clientHandler)
                            {
                                clientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                            }
                            return handler;
                        };
                    }).Build();
            vm = ChatHubPageViewModel.CreatedConnectedChatHubVM(new IntranetSignalRHelper(connection));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) => splitViewPane.IsPaneOpen = !splitViewPane.IsPaneOpen;
        //private async void AddChannel_Click(object sender, RoutedEventArgs e)
        //{
        //    ExplorerItem folder1 = new ExplorerItem()
        //    {
        //        Name = "Work Documents",
        //        Type = ExplorerItem.ExplorerItemType.Folder,
        //    };
        //    DataSource.Add(folder1);
        //}
        private void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            try
            {
                vm.sendMessageCommand.Execute(MessageTextBox.Text);
                MessageTextBox.Text = "";
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        private void ChatList_LostFocus(object sender, RoutedEventArgs e)
        {
            ChatList.SelectedItem = null;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var currentTheme = Application.Current.RequestedTheme;
            AdaptiveTheme(currentTheme);

            //Detect theme change
            var Listener = new ThemeListener();
            Listener.ThemeChanged += Listener_ThemeChanged;

            await vm.InitDefaultValue();
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
                    BackgroundImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/DemoPurpose/Others/sand.png"));
                    break;
                case ApplicationTheme.Light:
                    BackgroundImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/DemoPurpose/Others/snow.jpg"));
                    break;
                default:
                    break;
            }
        }

        private void Member_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChatList.Visibility         = Visibility.Collapsed;
            MessageInputArea.Visibility = Visibility.Collapsed;
            MemberDetailFrame.Navigate(typeof(MemberDetailPage), MembersList.SelectedItem);
        }

        private async void StartConversation_Click(object sender, RoutedEventArgs e)
        {
            var user = (UserDTO)((FrameworkElement)sender).DataContext;
            await vm.CreateConversation(user);
        }
    }
}
