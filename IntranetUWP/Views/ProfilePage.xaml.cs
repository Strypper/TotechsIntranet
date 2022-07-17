using IntranetUWP.Contracts;
using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.RefitInterfaces;
using IntranetUWP.UserControls.Dialogs;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Refit;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;


namespace IntranetUWP.Views
{
    public sealed partial class ProfilePage : Page, INotifyPropertyChanged
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

        private readonly IUserData    userData    = RestService.For<IUserData>(App.BaseUrl);
        private readonly IProjectData projectData = RestService.For<IProjectData>(App.BaseUrl);
        private ILocalUsersService localUsersService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ProfilePage()
        {
            this.InitializeComponent();
            localUsersService = MainPage.Context.GetRequiredService<ILocalUsersService>();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.localSettings.Values["UserGuid"] != null)
            {
                User         = await localUsersService.GetLocalUserAsync(App.localSettings.Values["UserGuid"].ToString());

                var projects = await projectData.GetAllProjectsWithMembers();
                ProjectsCards.ItemsSource = projects.Where(t => t.Members.Any(m => m.Guid == User.Guid));
                BioSymbol.Symbol = String.IsNullOrEmpty(User.Bio) ? Symbol.Add : Symbol.Edit;
            } 

            SkillsSymbol.Symbol = SkillsList.Items.Count > 0 ? Symbol.Edit : Symbol.Add;

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
                    break;
                case ApplicationTheme.Light:
                    break;
                default:
                    break;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void BioEdit_Click(object sender, RoutedEventArgs e)
        {
            var bioDialog = new BioDialog() { Content = User.Bio };
            bioDialog.PrimaryButtonClick += BioDialog_PrimaryButtonClick;
            await bioDialog.ShowAsync();    
        }

        private void BioDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Bio.Text = (sender as BioDialog).Content;
            User.Bio = Bio.Text;
        }

        private async void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            var skillsDialog = new SkillDialog();
            skillsDialog.PrimaryButtonClick += SkillsDialog_PrimaryButtonClick;
            await skillsDialog.ShowAsync();
        }
        private void SkillsDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var updatedSkills = (sender as SkillDialog).Skills;
            User.Skills.Clear();
            foreach (var skill in updatedSkills)
            {
                User.Skills.Add(skill);
            }
        }

        private async void UpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(ChangePasswordBox.Password))
            {
                var updateResult = await userData.Update(User);
                if (updateResult.StatusCode == HttpStatusCode.NoContent)
                {
                    PageStatus.Message = "Your info successfully updated";
                    PageStatus.Title = "Update Status";
                    PageStatus.Severity = InfoBarSeverity.Success;
                    PageStatus.IsOpen = true;
                }
                else
                {
                    PageStatus.Message = "Your info still not updated";
                    PageStatus.Title = "Update Status";
                    PageStatus.Severity = InfoBarSeverity.Error;
                    PageStatus.IsOpen = true;
                    Debug.WriteLine("Create operation error");
                }
            }
            else
            {
                User.Password = ChangePasswordBox.Password;
                var updateResult = await userData.UpdatePassword(User);
                if (updateResult.StatusCode == HttpStatusCode.NoContent)
                {
                    PageStatus.Message         = "Your info with new password successfully updated, please logout and relogin again";
                    PageStatus.Title           = "Update Status";
                    PageStatus.Severity        = InfoBarSeverity.Success;
                    PageStatus.IsOpen          = true;
                    ChangePasswordBox.Password = String.Empty;
                }
                else
                {
                    PageStatus.Message = "Your info still not updated";
                    PageStatus.Title = "Update Status";
                    PageStatus.Severity = InfoBarSeverity.Error;
                    PageStatus.IsOpen = true;
                    Debug.WriteLine("Create operation error");
                }
            }
        }

        public static BitmapImage GetUserInfo(string userGuid, ProjectDTO team)
        {
            if (!String.IsNullOrEmpty(userGuid))
            {
                var user = team.Members.FirstOrDefault(u => u.Guid == userGuid);
                return new BitmapImage(new Uri(user.ProfilePic));
            }
            else return null;
        }
    }
}
