using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using Windows.UI.Core;
using Windows.UI.ViewManagement.Core;
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
        private ObservableCollection<ExplorerItem> DataSource;
        public ChatHubPageViewModel vm { get; set; }
        public ChatHubPage()
        {
            this.InitializeComponent();
            DataSource = GetData();
            var connection = new HubConnectionBuilder()
                    //.WithUrl("https://localhost:44371/chathub", options =>
                    //{
                    //    options.HttpMessageHandlerFactory = (handler) =>
                    //    {
                    //        if (handler is HttpClientHandler clientHandler)
                    //        {
                    //            clientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    //        }
                    //        return handler;
                    //    };
                    //}).Build();
                    .WithUrl("https://intranetapi.azurewebsites.net/chathub").Build();
            vm = ChatHubPageViewModel.CreatedConnectedChatHubVM(new IntranetSignalRHelper(connection));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) => splitViewPane.IsPaneOpen = !splitViewPane.IsPaneOpen;
        private ObservableCollection<ExplorerItem> GetData()
        {
            var list = new ObservableCollection<ExplorerItem>();
            ExplorerItem folder1 = new ExplorerItem()
            {
                Name = "Announcements",
                IconProp = "\uE789",
                Color = "#ffc200",
                Type = ExplorerItem.ExplorerItemType.Folder,
                Children =
                {
                    new ExplorerItem()
                    {
                        Name = "Health Care",
                        IconProp = "\uE95E",
                        Color = "#ea1c37",
                        Type = ExplorerItem.ExplorerItemType.Folder
                    },
                    new ExplorerItem()
                    {
                        Name = "An Phu Building",
                        IconProp = "\uEC07",
                        Type = ExplorerItem.ExplorerItemType.File,
                    },
                    new ExplorerItem()
                    {
                        Name = "English Class",
                        IconProp = "\uEC87",
                        Type = ExplorerItem.ExplorerItemType.File,
                    }
                },
                IsVisisbleAddButton = true
            };
            ExplorerItem developerZone = new ExplorerItem()
            {
                Name = "Developer Zone",
                IconProp = "\uE943",
                Type = ExplorerItem.ExplorerItemType.Folder,
                Children =
                {
                    new ExplorerItem()
                    {
                        Name = "Design Zone",
                        IconProp = "\uE2B1",
                        Type = ExplorerItem.ExplorerItemType.Folder
                    },
                    new ExplorerItem()
                    {
                        Name = "Front-end Zone",
                        IconProp = "\uE8FC",
                        Type = ExplorerItem.ExplorerItemType.Folder
                    },
                    new ExplorerItem()
                    {
                        Name = "Back-end Zone",
                        IconProp = "\uEDA2",
                        Type = ExplorerItem.ExplorerItemType.File,
                    },
                    new ExplorerItem()
                    {
                        Name = "DevOp Zone",
                        IconProp = "\uEBD2",
                        Type = ExplorerItem.ExplorerItemType.File,
                    }
                },
                IsVisisbleAddButton = true
            };

            ExplorerItem folder2 = new ExplorerItem()
            {
                Name = "HR Help",
                IconProp = "\uE902",
                Type = ExplorerItem.ExplorerItemType.Folder,
                IsVisisbleAddButton = true
            };

            ExplorerItem folder3 = new ExplorerItem()
            {
                Name = "IT Helpdesk",
                IconProp = "\uEC4E",
                Type = ExplorerItem.ExplorerItemType.Folder,
                IsVisisbleAddButton = true
            };

            list.Add(folder1);
            list.Add(developerZone);
            list.Add(folder2);
            list.Add(folder3);
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
        }
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

        private void Emoji_Click(object sender, RoutedEventArgs e)
        {}

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
    }

    public class ExplorerItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public enum ExplorerItemType { Folder, File };
        public string Name { get; set; }
        public string IconProp { get; set; }
        public string Color { get; set; } = "#f1f1f1";
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
}
