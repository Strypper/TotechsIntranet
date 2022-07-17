using IntranetUWP.Constanst;
using IntranetUWP.Helpers;
using IntranetUWP.Views;
using IntranetUWP.Views.MemberChildPages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.WindowsAzure.Messaging;
using Microsoft.Azure.NotificationHubs;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using IntranetUWP.Models;
using IntranetUWP.RefitInterfaces;
using Refit;
using Windows.UI.Xaml.Navigation;
using IntranetUWP.Contracts;
using IntranetUWP.Services;
using Microsoft.Toolkit.Helpers;
using Windows.Storage;
using Newtonsoft.Json;
using Microsoft.Toolkit.Uwp.Helpers;

namespace IntranetUWP
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private UserDTO user;
        public UserDTO User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private IntranetSignalRHelper signalRHelper { get; set; }
        private IServiceCollection _serviceCollection;
        private ILocalUsersService localUsersService;
        public UserIdentityDTO identityUser { get; set; }
        private bool signalRStatus;
        public bool SignalRStatus
        {
            get { return signalRStatus; }
            set 
            {
                signalRStatus = value;
                OnPropertyChanged();
            }
        }

        public static Ioc Context => _context;
        public static Ioc _context = null;

        private readonly IUserData userData = RestService.For<IUserData>(App.BaseUrl);

        public MainPage()
        {
            this.InitializeComponent();
            InitializeEnvironment();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            identityUser = (UserIdentityDTO)e.Parameter;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            signalRHelper = MainPage.Context.GetRequiredService<IntranetSignalRHelper>();
            localUsersService = MainPage.Context.GetRequiredService<ILocalUsersService>();
            await signalRHelper.ConnectAsync().ContinueWith(async task =>
            {
                if (task.Exception != null)
                {
                    Debug.WriteLine("Unable to connect");
                    SignalRStatus = false;
                }
                else
                {
                    Debug.WriteLine("Connection success");
                    SignalRStatus = true;
                }
            });

            if (App.localSettings.Values["UserGuid"] != null)
            {
                User = await userData.GetUserByGuid(App.localSettings.Values["UserGuid"].ToString());
                if (User != null)
                {
                    User.FillUserIdentityInfo(identityUser);

                    await localUsersService.SaveLocalUserAsync(User);

                    await InitNotificationsAsync();
                    //Frame.Navigate(typeof(ProfilePage));
                }
            }
        }

        private void NavigationViewPanel_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            Microsoft.UI.Xaml.Controls.NavigationViewItem item = args.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;
            NavView_Navigate(item);
        }

        private void NavView_Navigate(Microsoft.UI.Xaml.Controls.NavigationViewItem item)
        {
            switch (item.Name)
            {
                case "LunchOrder":
                    TheMainFrame.Navigate(typeof(FoodOrderPage));
                    break;
                case "TeaBreak":
                    TheMainFrame.Navigate(typeof(TeaBreakPage));
                    break;
                case "PlayTime":
                    TheMainFrame.Navigate(typeof(PlayTimePage));
                    break;
                case "ChatHub":
                    TheMainFrame.Navigate(typeof(ChatHubPage));
                    break;
                case "Projects":
                    TheMainFrame.Navigate(typeof(ProjectsPage));
                    break;
                case "Members":
                    TheMainFrame.Navigate(typeof(MemberPage));
                    break;
                case "SettingsItem":
                    TheMainFrame.Navigate(typeof(SettingsPage));
                    break;
                case "Profile":
                    TheMainFrame.Navigate(typeof(ProfilePage));
                    break;
                default:
                    TheMainFrame.Navigate(typeof(FoodOrderPage));
                    break;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            App.localSettings.Values.Clear();
            Frame.Navigate(typeof(LoginPage));
        }

        private void InitializeEnvironment()
        {
            _context = new Ioc();

            if (_serviceCollection == null)
            {
                _serviceCollection = new ServiceCollection();
            }
            ConfigureServices(_serviceCollection);
            Context.ConfigureServices(_serviceCollection.BuildServiceProvider());
        }
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton((_) => new HubConnectionBuilder()
                    .WithAutomaticReconnect()
                    .WithUrl(SignalRConstants.BaseUrl).Build());
            services.AddSingleton((_) => new UserSerializer());
            //.WithUrl(SignalRContants.LocalBaseUrl, options =>
            //{
            //    options.HttpMessageHandlerFactory = (handler) =>
            //    {
            //        if (handler is HttpClientHandler clientHandler)
            //        {
            //            clientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //        }
            //        return handler;
            //    };
            //}).Build());

            services.AddSingleton<IntranetSignalRHelper>();
            services.AddSingleton<ILocalUsersService, ToolkitObjectStorageServices>();
        }
        private async Task InitNotificationsAsync()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            var hub = new NotificationHubClient(AzureNotificationHubContants.ConnectionString,
                                                AzureNotificationHubContants.HubName);
            var result = await hub.CreateWindowsNativeRegistrationAsync(channel.Uri, new string[] { User.Guid });
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
