using IntranetUWP.Constanst;
using IntranetUWP.Models;
using IntranetUWP.RefitInterfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Refit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Core;
using WindowsDispatcher = Windows.ApplicationModel.Core.CoreApplication;

namespace IntranetUWP.Helpers
{
    public class IntranetSignalRHelper
    {
        private readonly HubConnection          _hubConnection;
        public  event    Action<ChatMessageDTO>  GeneralChatMessageReceived;
        public  ObservableCollection<UserDTO>    OnlineUsersList             = new ObservableCollection<UserDTO>();
        public  string                           SelfUserId                  = App.localSettings.Values["UserGuid"].ToString();
        public IntranetSignalRHelper(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
            _hubConnection.On<string, DateTime, UserDTO>("ReceiveMessage", (message, sentTime, user) =>
            {
                ChatMessageDTO chatmessage = new ChatMessageDTO() 
                    { 
                        User = user,
                        MessageContent = message,
                        IsFromSelf = SelfUserId == user.Guid ? true : false,
                        SentTime = sentTime
                };
                GeneralChatMessageReceived?.Invoke(chatmessage);
            });
            _hubConnection.On<string, List<UserDTO>>("IdentifyUser", async (connectionId, onlineUsersList) =>
            {
                System.Diagnostics.Debug.WriteLine(connectionId);
                if (onlineUsersList != null && onlineUsersList.Count != 0)
                {
                    onlineUsersList.ForEach(user => OnlineUsersList.Add(user));
                }
                await _hubConnection.InvokeAsync("ChatHubUserIndentity", connectionId, SelfUserId);
            });
            _hubConnection.On<string>("UserLogOut", async (userLogoutConnectionString) =>
            {
                var removeUser = OnlineUsersList.FirstOrDefault(user => user.SignalRConnectionId == userLogoutConnectionString);
                if (removeUser != null)
                {
                    await WindowsDispatcher.MainView
                       .CoreWindow
                       .Dispatcher
                       .RunAsync(CoreDispatcherPriority.Normal,
                                () =>
                                {
                                    OnlineUsersList.Remove(removeUser);
                                });
                }
            });
            _hubConnection.On<UserDTO>("UserLogIn", async (loginUser) =>
            {
                if (loginUser != null)
                {
                    await WindowsDispatcher.MainView
                                           .CoreWindow
                                           .Dispatcher
                                           .RunAsync(CoreDispatcherPriority.Normal,
                                                    () =>
                                                    {
                                                        if (OnlineUsersList.Contains(loginUser) == false)
                                                        {
                                                            OnlineUsersList.Add(loginUser);
                                                        }
                                                    });
                }
            });
        }

        public async Task ConnectAsync()
        {
            await _hubConnection.StartAsync();
        }

        public async Task SendMessageAsync(string mess, 
                                           DateTime sentTime, 
                                           int conversationId, 
                                           string fromUserGuid,
                                           string toUserGuid) => await _hubConnection.InvokeAsync("SendMessage", mess, sentTime, conversationId, fromUserGuid, toUserGuid);
    }
}
