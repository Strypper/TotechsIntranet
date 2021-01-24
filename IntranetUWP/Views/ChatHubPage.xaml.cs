using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatHubPage : Page
    {
        private ObservableCollection<ExplorerItem> DataSource;
        private ObservableCollection<ChatMessage> ChatMessages = new ObservableCollection<ChatMessage>();
        private ChatMessage chatMessage = new ChatMessage();
        private HubConnection connection;
        public ChatHubPage()
        {
            this.InitializeComponent();
            DataSource = GetData();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) => splitViewPane.IsPaneOpen = !splitViewPane.IsPaneOpen;
        private ObservableCollection<ExplorerItem> GetData()
        {
            var list = new ObservableCollection<ExplorerItem>();
            ExplorerItem folder1 = new ExplorerItem()
            {
                Name = "Work Documents",
                Type = ExplorerItem.ExplorerItemType.Folder,
                Children =
                {
                    new ExplorerItem()
                    {
                        Name = "Functional Specifications",
                        Type = ExplorerItem.ExplorerItemType.Folder,
                        Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "TreeView spec",
                                Type = ExplorerItem.ExplorerItemType.File,
                            }
                        }
                    },
                    new ExplorerItem()
                    {
                        Name = "Feature Schedule",
                        Type = ExplorerItem.ExplorerItemType.File,
                    },
                    new ExplorerItem()
                    {
                        Name = "Overall Project Plan",
                        Type = ExplorerItem.ExplorerItemType.File,
                    },
                    new ExplorerItem()
                    {
                        Name = "Feature Resources Allocation",
                        Type = ExplorerItem.ExplorerItemType.File,
                    }
                }, 
                IsVisisbleAddButton = true
            };
            ExplorerItem folder2 = new ExplorerItem()
            {
                Name = "Personal Folder",
                Type = ExplorerItem.ExplorerItemType.Folder,
                Children =
                        {
                            new ExplorerItem()
                            {
                                Name = "Home Remodel Folder",
                                Type = ExplorerItem.ExplorerItemType.Folder,
                                Children =
                                {
                                    new ExplorerItem()
                                    {
                                        Name = "Contractor Contact Info",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Paint Color Scheme",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Flooring Woodgrain type",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    },
                                    new ExplorerItem()
                                    {
                                        Name = "Kitchen Cabinet Style",
                                        Type = ExplorerItem.ExplorerItemType.File,
                                    }
                                }
                            }
                        },
                IsVisisbleAddButton = true
            };

            list.Add(folder1);
            list.Add(folder2);
            return list;
        }

        private async void AddChannel_Click(object sender, RoutedEventArgs e)
        {
            ExplorerItem folder1 = new ExplorerItem()
            {
                Name = "Work Documents",
                Type = ExplorerItem.ExplorerItemType.Folder,
            };
            DataSource.Add(folder1);
            try
            {
                //await connection.InvokeAsync("IdentifyUser", 5);
                await connection.InvokeAsync("SendMessage", MessageTextBox.Text, 5);
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            connection = new HubConnectionBuilder()
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

            await connection.StartAsync();

            connection.On<string, string>("ReceiveMessage", async (message, user) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    chatMessage.UserName = user;
                    chatMessage.Message = message;
                    ChatMessages.Add(chatMessage);
                });
            });
        }
    }

    public class ExplorerItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public enum ExplorerItemType { Folder, File };
        public string Name { get; set; }
        public ExplorerItemType Type { get; set; }
        private ObservableCollection<ExplorerItem> m_children;
        public ObservableCollection<ExplorerItem> Children
        {
            get
            {
                if (m_children is null) m_children = new ObservableCollection<ExplorerItem>();
                return m_children;
            }
            set => m_children = value;
        }

        private bool m_isExpanded;
        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                if (m_isExpanded != value)
                {
                    m_isExpanded = value;
                    NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        private bool isVisisbleAddButton;
        public bool IsVisisbleAddButton
        {
            get { return isVisisbleAddButton; }

            set
            {
                if (isVisisbleAddButton != value)
                {
                    isVisisbleAddButton = value;
                    NotifyPropertyChanged("IsVisisbleAddButton");
                }
            }

        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ChatMessage
    {
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
