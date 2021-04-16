using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.ViewModels.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntranetUWP.ViewModels.PagesViewModel
{
    public class ChatHubPageViewModel : ViewModelBase
    {
        public ObservableCollection<ChatMessageDTO> ChatMessages { get; set; }
        private IntranetSignalRHelper signalRHelper { get; set; }
        public SignalRSendMessageCommandParameter sendMessageCommand { get; set; }
        private string messContent;

        public string MessContent
        {
            get { return messContent; }
            set { messContent = value; OnPropertyChanged("MessContent"); }
        }

        public static ChatHubPageViewModel CreatedConnectedChatHubVM(IntranetSignalRHelper service)
        {
            ChatHubPageViewModel vm = new ChatHubPageViewModel(service);
            service.ConnectAsync().ContinueWith(task =>
            {
                if(task.Exception != null)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to connect");
                }
            });
            return vm;
        }
        public ChatHubPageViewModel(IntranetSignalRHelper service)
        {
            signalRHelper = service;
            sendMessageCommand = new SignalRSendMessageCommandParameter(this);
            ChatMessages = new ObservableCollection<ChatMessageDTO>();
            signalRHelper.GeneralChatMessageReceived += ChatMessReceived;
        }

        public async Task SendMessage(ChatMessageDTO messContent)
        {
            await signalRHelper.SendMessageAsync(messContent.MessageContent, 5);
            MessContent = "";
        }

        private void ChatMessReceived(ChatMessageDTO chatMessage)
        {
            ChatMessages.Add(chatMessage);
        }
    }
}
