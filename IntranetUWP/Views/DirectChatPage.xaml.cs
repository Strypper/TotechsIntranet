using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.Models.MixModels;
using IntranetUWP.ViewModels.PagesViewModel;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.ViewManagement.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using WindowsDispatcher = Windows.ApplicationModel.Core.CoreApplication;

namespace IntranetUWP.Views
{

    public sealed partial class DirectChatPage : Page
    {
        public ChatHubPageViewModel vm { get; set; }
        public ConversationDTO Conversation { get; set; }
        public UserDTO TargetUserInformation { get; set; }
        public IntranetSignalRHelper signalRHelper { get; set; }
        private bool isAppFocus;

        public ObservableCollection<ChatMessageDTO> ChatMessages { get; set; } = new ObservableCollection<ChatMessageDTO>();
        public DirectChatPage()
        {
            this.InitializeComponent();
            Application.Current.Suspending += Current_Suspending;
            Application.Current.Resuming += Current_Resuming;
            Window.Current.CoreWindow.Activated += (sender, args) =>
            {
                if (args.WindowActivationState == CoreWindowActivationState.Deactivated)
                {
                    isAppFocus = false;
                }
                else
                {
                    isAppFocus = true;
                }
            };
        }

        private void Current_Suspending(object sender, SuspendingEventArgs e)
        {
        }

        private void Current_Resuming(object sender, object e)
        {
            Conversation.ChatMessages.Add(new ChatMessageDTO() { MessageContent = "Im back" });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MessageTextBox.Focus(FocusState.Programmatic);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var param = e.Parameter as ConversationWithViewModel;
            Conversation = param.conversation;
            vm = param.vm;
            signalRHelper = MainPage.Context.GetRequiredService<IntranetSignalRHelper>();

            //Sub to receive message
            signalRHelper.GeneralChatMessageReceived += OnChatMessageReceived;

            TargetUserInformation = Conversation.Users.Where(user => user.Guid != App.localSettings.Values["UserGuid"].ToString()).FirstOrDefault();
            foreach (var chatMessage in Conversation.ChatMessages)
            {
                chatMessage.IsFromSelf = chatMessage.User.Guid == App.localSettings.Values["UserId"].ToString() ? true : false;
                ChatMessages.Add(chatMessage);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //UnSub here
            signalRHelper.GeneralChatMessageReceived -= OnChatMessageReceived;
        }
        private async void OnChatMessageReceived(ChatMessageDTO chatMessage)
        {
            await WindowsDispatcher.MainView
                .CoreWindow
                .Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal,
                                                        () =>
                                                        {
                                                            ChatMessages.Add(chatMessage);
                                                            vm.InvokeRecentChatOrder(Conversation.id);
                                                        });
            if (isAppFocus)
                    {
                    }
            else
            {
                if (chatMessage.IsFromSelf == false)
                {
                    new ToastContentBuilder()
                    .SetToastScenario(ToastScenario.Default)
                    .AddText(TargetUserInformation.FullName)
                    .AddText(chatMessage.MessageContent)
                    .AddAppLogoOverride(new Uri(TargetUserInformation.ProfilePic))
                    .AddAudio(new ToastAudio() { Src = new Uri("ms-appx:///Assets/AppAudio/clearly-602.mp3") })
                    .Show();
                }
            }
        }

        private void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        => sendMessage();

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        => sendMessage();
        private void sendMessage()
        {
            if(!string.IsNullOrWhiteSpace(MessageTextBox.Text) && !string.IsNullOrEmpty(MessageTextBox.Text))
            {
                try
                {
                    var VietNamZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    var dt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                    var DateTimeInVietNamLocal = TimeZoneInfo.ConvertTime(dt, TimeZoneInfo.Utc, VietNamZone);

                    var sendMessageDTO = new SendMessageDTO()
                    {
                        ChatMessage = new ChatMessageDTO() 
                                            { 
                                                MessageContent = MessageTextBox.Text, 
                                                SentTime = DateTimeInVietNamLocal 
                                            },
                        Conversation = Conversation,
                        FromUser = vm.CurrentUser,
                        ToUser = TargetUserInformation,
                    };
                    vm.sendMessageCommand.Execute(sendMessageDTO);
                    MessageTextBox.Text = "";
                }
                catch (Exception ex) { Debug.WriteLine(ex.Message); }
            }
        }

        private void Emoji_Click(object sender, RoutedEventArgs e)
        {
            CoreInputView.GetForCurrentView().TryShow(CoreInputViewKind.Emoji);
        }
    }

}
