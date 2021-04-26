using IntranetUWP.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public readonly string LoginUrl = "https://intranetapi.azurewebsites.net/api/User/Login";
        public readonly string RegisterUrl = "https://intranetapi.azurewebsites.net/api/User/Create";
        public string GetFoodUrl(int id) => $"https://intranetapi.azurewebsites.net/api/UserFood/GetUserSelectedFood/{id}";
        HttpClient httpClient = new HttpClient();
        public LoginPage()
        {
            this.InitializeComponent();
            RegisterSwitch.Toggled += (s, e) => NavigateMainFrameButton.Content = (s as ToggleSwitch).IsOn ? "Register" : "Login";
        }

        private async void NavigateMainFrameButton_Click(object sender, RoutedEventArgs e)
        {
            if(RegisterSwitch.IsOn != true)
            {
                WorkingBar.ShowError = false;
                WorkingBar.Visibility = Visibility.Visible;
                LoginModel logininfo = new LoginModel() { userName = UserName.Text, password = Password.Password };
                var content = new StringContent(JsonConvert.SerializeObject(logininfo), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(LoginUrl, content);
                var result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.ToString() == "OK")
                {
                    var userInfo = JsonConvert.DeserializeObject<UserDTO>(result);
                    App.localSettings.Values["UserId"] = userInfo.id;
                    App.localSettings.Values["UserName"] = userInfo.userName;
                    App.localSettings.Values["Password"] = Password.Password;
                    App.localSettings.Values["ProfilePic"] = userInfo.profilePic;
                    var foodRequest = await httpClient.GetAsync(GetFoodUrl(userInfo.id));
                    var foodResult = await foodRequest.Content.ReadAsStringAsync();
                    var foodInfo = JsonConvert.DeserializeObject<UserFoodDTO>(foodResult);
                    if(foodInfo != null) App.localSettings.Values["FoodId"] = foodInfo.food.id;
                    Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    WorkingBar.ShowError = true;
                    Password.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 240, 0));
                }
                System.Diagnostics.Debug.WriteLine(response.StatusCode.ToString());
                System.Diagnostics.Debug.WriteLine(result);
            }
            else
            {
                WorkingBar.ShowError = false;
                WorkingBar.Visibility = Visibility.Visible;
                RegistingModel signUpInfo = new RegistingModel() { userName = UserName.Text, password = Password.Password, 
                                                                   company = iDealogicToggle.IsChecked == true ? true : false,
                                                                   firstName = FirstName.Text, middleName = MiddleName.Text, lastName = LastName.Text,
                                                                   gender = BoyToggle.IsChecked == true ? true : false, age = AgeSlider.Value.ToString()};
                var content = new StringContent(JsonConvert.SerializeObject(signUpInfo), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(RegisterUrl, content);
                var result = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.ToString() == "Created")
                {
                    RegisterSwitch.IsOn = false;
                    WorkingBar.Visibility = Visibility.Collapsed;
                    Status.Text = "Alright let's log in 🤩";
                }
                else
                {
                    WorkingBar.ShowError = true;
                    Status.Text = "Something is wrong 🤔";
                    Password.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 240, 0));
                }
                System.Diagnostics.Debug.WriteLine(response.StatusCode.ToString());
                System.Diagnostics.Debug.WriteLine(result);
            }
        }

        private void iDealogicToggle_Click(object sender, RoutedEventArgs e)
        {
            iDealogicToggle.IsChecked = true;
            DevinitionToggle.IsChecked = false;
        }
        private void DeviToggle_Click(object sender, RoutedEventArgs e)
        {
            DevinitionToggle.IsChecked = true;
            iDealogicToggle.IsChecked = false;
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            Password.ClearValue(PasswordBox.ForegroundProperty);
            WorkingBar.Visibility = Visibility.Collapsed;
        }
    }
}
