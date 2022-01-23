using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntranetUWP.ViewModels.PagesViewModel
{
    public class ChatHubPageViewModel : ViewModelBase
    {   
        public readonly string getUserDataUrl            = "User/Get";
        public readonly string getAllUsersDataUrl        = "User/GetAll";
        public readonly string getConversationDataUrl    = "Conversation/Get";
        public readonly string createConversationDataUrl = "Conversation/Create";
        public  ObservableCollection<ChatMessageDTO> ChatMessages       { get; set; }
        public  ObservableCollection<UserDTO>        Users              { get; set; }
        private IntranetSignalRHelper                signalRHelper      { get; set; }
        public  SignalRSendMessageCommandParameter   sendMessageCommand { get; set; }

        private string messContent;

        public string MessContent
        {
            get { return messContent; }
            set { messContent = value; OnPropertyChanged("MessContent"); }
        }

        private UserDTO currentUser;

        public UserDTO CurrentUser
            {
            get { return currentUser; }
            set { currentUser = value; OnPropertyChanged("CurrentUser"); }
        }

        private ConversationDTO selectedConversation;

        public ConversationDTO SelectedConversation
        {
            get { return selectedConversation; }
            set { selectedConversation = value; OnPropertyChanged("User"); }
        }


        private ObservableCollection<ConversationDTO> conversations;

        public ObservableCollection<ConversationDTO> Conversations
        {
            get { return conversations; }
            set { conversations = value; OnPropertyChanged("Conversations"); }
        }


        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();


        public static ChatHubPageViewModel CreatedConnectedChatHubVM(IntranetSignalRHelper service)
        {
            ChatHubPageViewModel vm = new ChatHubPageViewModel(service);
            service.ConnectAsync().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    System.Diagnostics.Debug.WriteLine("Unable to connect");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Connection success");
                }
            });
            return vm;
        }
        public ChatHubPageViewModel(IntranetSignalRHelper service)
        {
            signalRHelper      = service;
            sendMessageCommand = new SignalRSendMessageCommandParameter(this);
            conversations      = new ObservableCollection<ConversationDTO>();
            ChatMessages       = new ObservableCollection<ChatMessageDTO>();
            Users              = new ObservableCollection<UserDTO>();
            signalRHelper.GeneralChatMessageReceived += ChatMessReceived;
        }

        public async Task InitDefaultValue()
        {
            CurrentUser             = await httpHelper
                                            .GetByIdAsync<UserDTO>(getUserDataUrl, (int)App.localSettings.Values["UserId"]);

            var usersResult         = await httpHelper
                                            .GetAsync<List<UserDTO>>(getAllUsersDataUrl);
            if(usersResult != null)          usersResult.Where(u => u.id != currentUser.id)
                                                        .ToList()
                                                        .ForEach(user => Users.Add(user));   

            var conversationsResult = await httpHelper
                                            .GetAsync<List<ConversationDTO>>(getConversationDataUrl);
            if(conversationsResult != null) conversationsResult.ForEach(conversation => Conversations.Add(conversation));

        }

        public async Task SendMessage(ChatMessageDTO messContent)
        {
            await signalRHelper.SendMessageAsync(messContent.MessageContent, SelectedConversation.id);
            MessContent = "";
        }

        public async Task CreateConversation(UserDTO targetUser)
        {
            IsBusy = true;
            if (CurrentUser != null)
            {
                var conversation = new ConversationDTO()
                {
                    Users = new List<UserDTO>()
                                          {   
                                              currentUser,
                                              targetUser
                                          }
                };
                var createResult = await httpHelper.CreateAsync<ConversationDTO>(createConversationDataUrl, conversation);
                if (createResult != null)
                {
                    Conversations.Add(createResult);
                }
                IsBusy = false;
            }
        }

        private void ChatMessReceived(ChatMessageDTO chatMessage)
        {
            ChatMessages.Add(chatMessage);
        }
    }
}
