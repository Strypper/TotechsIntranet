using IntranetUWP.Models;
using IntranetUWP.RefitInterfaces;
using Microsoft.Toolkit.Collections;
using Refit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IntranetUWP.UserControls
{
    public sealed partial class RecentChat : UserControl
    {
        public delegate void RecentChatEventHandler(ConversationDTO recentChat);
        public event RecentChatEventHandler SelectRecentChatEvent;
        public ObservableCollection<ConversationDTO> Conversations
        {
            get { return (ObservableCollection<ConversationDTO>)GetValue(ConversationsProperty); }
            set { SetValue(ConversationsProperty, value); }
        }
        public static readonly DependencyProperty ConversationsProperty =
            DependencyProperty.Register("Conversations", typeof(ObservableCollection<ConversationDTO>), typeof(RecentChat), new PropertyMetadata(new ObservableCollection<ConversationDTO>()));

        public ConversationDTO SelectedConversation
        {
            get { return (ConversationDTO)GetValue(SelectedConversationProperty); }
            set { SetValue(SelectedConversationProperty, value); }
        }
        public static readonly DependencyProperty SelectedConversationProperty =
            DependencyProperty.Register("SelectedConversation", typeof(ConversationDTO), typeof(RecentChat), null);

        public ObservableCollection<UserDTO> OnlineUsers
        {
            get { return (ObservableCollection<UserDTO>)GetValue(OnlineUsersProperty); }
            set { SetValue(OnlineUsersProperty, value); }
        }

        public static readonly DependencyProperty OnlineUsersProperty =
            DependencyProperty.Register("OnlineUsers", typeof(ObservableCollection<UserDTO>), typeof(RecentChat), null);




        public RecentChat()
        {
            this.InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRecentChat = RecentChatList.SelectedItem as ConversationDTO;
            SelectRecentChatEvent.Invoke(selectedRecentChat);
        }

        public void ChangeListViewOrder(int chatId)
        {
            var conversation = Conversations.FirstOrDefault(con => con.id == chatId);
            var index = Conversations.IndexOf(conversation);
            if(index != 0)
            {
                Conversations.RemoveAt(index);
                Conversations.Insert(0, conversation);
            }
        }
    }
}
