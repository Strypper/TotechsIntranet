using IntranetUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class RecentChat : UserControl
    {
        private ObservableCollection<ChatMessageDTO> dummyChat = new ObservableCollection<ChatMessageDTO>();
        public RecentChat()
        {
            this.InitializeComponent();
            loadDummyData();
        }

        private void loadDummyData()
        {
            for (int i = 0; i < 50; i++)
            {
                dummyChat.Add(new ChatMessageDTO()
                {
                    UserName = "Strypper Jason",
                    MessageContent = "Bruh we need better access data layers",
                    ProfilePic = "https://i.imgur.com/vc9FudE.jpg"
                });
            }
        }
    }
}
