using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Diagnostics;
using IntranetUWP.RefitInterfaces;
using Refit;
using Microsoft.Toolkit.Uwp;

namespace IntranetUWP.ViewModels.PagesViewModel
{
    public class ChatHubPageViewModel : ViewModelBase
    {   
        public readonly string getUserDataUrl                 = "User/Get";
        public readonly string getAllUsersDataUrl             = "User/GetAll";
        public readonly string createConversationDataUrl      = "UserConversation/Create";
        public readonly string getByUserIdDirectModeDataUrl   = "Conversation/GetByUserIdDirectMode";
        public  ObservableCollection<ChatMessageDTO>  ChatMessages       { get; set; }
        public  ObservableCollection<ConversationDTO> Conversations      { get; set; }
        public  ObservableCollection<UserDTO>         Users              { get; set; }
        public  IntranetSignalRHelper                 signalRHelper      { get; set; }
        public  SignalRSendMessageCommandParameter    sendMessageCommand { get; set; }

        public delegate void ReorderRecentChatEventHandler(int chatId);
        public event ReorderRecentChatEventHandler ReorderRecentChat;

        private string messContent;

        public  string MessContent
        {
            get { return messContent; }
            set { messContent = value; OnPropertyChanged("MessContent"); }
        }

        private int pivotIndex;

        public  int PivotIndex
        {
            get { return pivotIndex; }
            set { pivotIndex = value; OnPropertyChanged("PivotIndex"); }
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
            set { selectedConversation = value; OnPropertyChanged("SelectedConversation"); }
        }

        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        private readonly IUserData userData = RestService.For<IUserData>(App.BaseUrl);
        private readonly IConversationData conversationData = RestService.For<IConversationData>(App.BaseUrl);
        public ChatHubPageViewModel()
        {
            signalRHelper      = MainPage.Context.GetRequiredService<IntranetSignalRHelper>();
            ChatMessages       = new ObservableCollection<ChatMessageDTO>();
            Users              = new ObservableCollection<UserDTO>();
            sendMessageCommand = new SignalRSendMessageCommandParameter(this);
            Conversations      = new IncrementalLoadingCollection<ConversationSupportIncrementalLoading, ConversationDTO>();
            //signalRHelper.GeneralChatMessageReceived += ChatMessReceived;
        }

        public async Task InitDefaultValue()
        {
            CurrentUser = await userData.GetUserByGuid(App.localSettings.Values["UserGuid"].ToString());
            var usersResult = await httpHelper.GetAsync<List<UserDTO>>(getAllUsersDataUrl);
            if(usersResult != null) usersResult.Where(u => u.Guid != currentUser.Guid)
                                               .ToList()
                                               .ForEach(user => Users.Add(user));
        }

        public void InvokeRecentChatOrder(int chatId)
        {
            ReorderRecentChat.Invoke(chatId);
        }

        public async Task SendMessage(SendMessageDTO sendMessageDTO)
        {
            await signalRHelper.SendMessageAsync(sendMessageDTO.ChatMessage.MessageContent, 
                                                 sendMessageDTO.ChatMessage.SentTime,
                                                 sendMessageDTO.Conversation.id,
                                                 sendMessageDTO.FromUser.Guid,
                                                 sendMessageDTO.ToUser.Guid);
            MessContent = "";
        }

        public async Task CreateConversation(UserDTO targetUser)
        {
            IsBusy = true;
            if (CurrentUser != null)
            {
                var createConversationDTO = new CreateUpdateUserConversationDTO()
                {
                    CurrentUserGuid = CurrentUser.Guid,
                    TargetUserGuid = targetUser.Guid
                };
                var createResult = await httpHelper.CreateAsync<ConversationDTO>(createConversationDataUrl, createConversationDTO);
                if (createResult != null && Conversations != null)
                {
                    bool existingConversation = Conversations.Any(conversation => conversation.id == createResult.id);
                    if (existingConversation)
                    {
                        //true
                    }
                    else
                    {
                        //false
                        Conversations.Add(createResult);
                    }
                    PivotIndex = 1;
                    if (Conversations != null & Conversations.Count != 0)
                    {
                        SelectedConversation = Conversations.First(conversation => conversation.id == createResult.id);
                    }
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
