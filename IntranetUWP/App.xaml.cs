using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.Views;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IntranetUWP
{
    sealed partial class App : Application
    {
        HttpClient httpClient = new HttpClient();
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public readonly string LoginUrl = "https://intranetapi.azurewebsites.net/api/User/Login";
        public static Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        //Active by Toast
        protected override async void OnActivated(IActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }
                Window.Current.Content = rootFrame;
            }
            if (e is ToastNotificationActivatedEventArgs)
            {
                var toastActivationArgs = e as ToastNotificationActivatedEventArgs;
                QueryString args = QueryString.Parse(toastActivationArgs.Argument);
                var CoreView = CoreApplication.GetCurrentView();
                var AppView = ApplicationView.GetForCurrentView();
                CoreApplicationViewTitleBar coreTitleBar = CoreView.TitleBar;
                coreTitleBar.ExtendViewIntoTitleBar = false;
                ApplicationViewTitleBar titleBar = AppView.TitleBar;
                switch (RequestedTheme)
                {
                    case ApplicationTheme.Light:
                        titleBar.ButtonForegroundColor = Colors.Black;
                        titleBar.BackgroundColor = Color.FromArgb(255, 230, 230, 230);
                        break;
                    case ApplicationTheme.Dark:
                        titleBar.BackgroundColor = Color.FromArgb(255, 31, 31, 31);
                        titleBar.ButtonForegroundColor = Colors.White;
                        break;
                }
                if (localSettings.Values != null)
                {
                    var UserName = localSettings.Values["UserName"];
                    var Password = localSettings.Values["Password"];
                    LoginModel logininfo = new LoginModel() { userName = UserName as string, password = Password as string };
                    var content = new StringContent(JsonConvert.SerializeObject(logininfo), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(LoginUrl, content);
                    if (response.StatusCode.ToString() == "OK")
                    {
                        if (args.Contains("chosenFood"))
                        {
                            ValueSet userInput = toastActivationArgs.UserInput;
                            var foodId = userInput.First().Value.ToString();
                            var createupdateUserFoodDTO = new CreateUpdateUserFoodDTO();
                            createupdateUserFoodDTO.userId = (int)localSettings.Values["UserId"];
                            createupdateUserFoodDTO.foodId = Int32.Parse(foodId);
                            await httpHelper.CreateAsync<UserFoodDTO>("UserFood/Create", createupdateUserFoodDTO);
                            App.localSettings.Values["FoodId"] = Int32.Parse(foodId);
                            if (!(rootFrame.Content is MainPage)) rootFrame.Navigate(typeof(MainPage));
                        }
                        else
                        {
                            switch (args["action"])
                            {
                                case "viewFoodPage":

                                    //If we're already viewing that page, do nothing
                                    if (rootFrame.Content is MainPage)
                                        break;

                                    // Otherwise navigate to view it
                                    rootFrame.Navigate(typeof(MainPage));
                                    break;
                            }
                        }
                    }
                    else rootFrame.Navigate(typeof(LoginPage));
                }
                if (rootFrame.BackStack.Count == 0)
                    rootFrame.BackStack.Add(new PageStackEntry(typeof(MainPage), null, null));
            }
            Window.Current.Activate();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            // 360 minutes = 6 hours ;)
            RegisterBackgroundTask("Notify lunch on Thursday", 120);
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) { }
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    var CoreView = CoreApplication.GetCurrentView();
                    var AppView = ApplicationView.GetForCurrentView();
                    CoreApplicationViewTitleBar coreTitleBar = CoreView.TitleBar;
                    coreTitleBar.ExtendViewIntoTitleBar = false;


                    ApplicationViewTitleBar titleBar = AppView.TitleBar;
                    //titleBar.ButtonBackgroundColor = Colors.Transparent;
                    //titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                    switch (RequestedTheme)
                    {
                        case ApplicationTheme.Light:
                            titleBar.ButtonForegroundColor = Colors.Black;
                            titleBar.BackgroundColor = Color.FromArgb(255, 230, 230, 230);
                            break;
                        case ApplicationTheme.Dark:
                            titleBar.BackgroundColor = Color.FromArgb(255, 31, 31, 31);
                            titleBar.ButtonForegroundColor = Colors.White;
                            break;
                    }
                    if (localSettings.Values != null)
                    {
                        var UserName = localSettings.Values["UserName"];
                        var Password = localSettings.Values["Password"];
                        LoginModel logininfo = new LoginModel() { userName = UserName as string, password = Password as string };
                        var content = new StringContent(JsonConvert.SerializeObject(logininfo), Encoding.UTF8, "application/json");
                        var response = await httpClient.PostAsync(LoginUrl, content);
                        if (response.StatusCode.ToString() == "OK")
                        {
                            rootFrame.Navigate(typeof(MainPage), e.Arguments);
                        } else rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                    }
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            var currentTime = DateTime.Now;
            if (currentTime.DayOfWeek == DayOfWeek.Thursday)
            {
                if(currentTime.Hour < 12)
                {
                    //Fire toast
                    new ToastContentBuilder()
                    .SetToastScenario(ToastScenario.Reminder)
                    .AddText("🍱 It's Thursday my friend!!!!")
                    .AddText("There are 12 dishes 🍽 this week")
                    .AddText("Deadline: 12:00PM Thursday noon ⏰")
                    .AddHeroImage(new Uri("ms-appx:///Assets/FoodAssets/FoodToast.png"))
                    .AddComboBox("foodList", "1", ("1", "Chicken rice"),
                                                     ("2", "Pork noodle"),
                                                     ("3", "Salmon fish rice"),
                                                     ("4", "Pizza"),
                                                     ("5", "Hamburger"))
                    .AddButton(new ToastButton("Order", "order"))
                    .Show();
                }
            }
        }

        void RegisterBackgroundTask(string name, uint freshnessTime)
        {
            var timeTrigger = new TimeTrigger(freshnessTime, false);
            if (!BackgroundTaskHelper.IsBackgroundTaskRegistered(name))
                BackgroundTaskHelper.Register(backgroundTaskName: name, trigger: timeTrigger);
        }
        void UnregisterBackgroundTask(string name)
        {
            if (BackgroundTaskHelper.IsBackgroundTaskRegistered(name))
                BackgroundTaskHelper.Unregister(name);
            // Easy where to put this method : where you need it?
        }

    }
}
