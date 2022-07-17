using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.Models.MixModels;
using IntranetUWP.ViewModels.PagesViewModel;
using IntranetUWP.Views.MemberChildPages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;


namespace IntranetUWP.Views
{
    public sealed partial class ChatHubPage : Page
    {
        public ChatHubPageViewModel vm { get; set; }
        public ChatHubPage()
        {
            this.InitializeComponent();
            vm = new ChatHubPageViewModel();
            vm.InitDefaultValue();
            vm.ReorderRecentChat += Vm_ReorderRecentChat;
        }
        private void Vm_ReorderRecentChat(int chatId)
        {
             RecentChatListView.ChangeListViewOrder(chatId);
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e) => splitViewPane.IsPaneOpen = !splitViewPane.IsPaneOpen;
        private void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            try
            {
                var sendMessageDTO = new SendMessageDTO()
                {
                    ChatMessage  = new ChatMessageDTO() { MessageContent = MessageTextBox.Text },
                    Conversation = vm.SelectedConversation,
                    FromUser     = vm.CurrentUser,
                    ToUser       = vm.SelectedConversation.Users.ElementAt(1)
                };
                vm.sendMessageCommand.Execute(sendMessageDTO);
                MessageTextBox.Text = "";
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        private void ChatList_LostFocus(object sender, RoutedEventArgs e)
        {
            ChatList.SelectedItem = null;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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
            ChatHubDetailFrame.Navigate(typeof(MemberDetailPage), MembersList.SelectedItem);
        }
        private async void StartConversation_Click(object sender, RoutedEventArgs e)
        {
            var user = (UserDTO)((FrameworkElement)sender).DataContext;
            await vm.CreateConversation(user);
        }
        private void RecentChat_SelectRecentChatEvent(ConversationDTO recentChat)
        {
            if(recentChat != null)
            {

                var parameter = new ConversationWithViewModel()
                {
                    conversation = recentChat,
                    vm           = vm
                };
                ChatList.Visibility         = Visibility.Collapsed;
                MessageInputArea.Visibility = Visibility.Collapsed;
                ChatHubDetailFrame.Navigate(typeof(DirectChatPage), parameter);
            }
        }
    }
}                                                             
