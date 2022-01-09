using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.UserControls.Dialogs;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

        public readonly string getUserDataUrl       = "User/Get";
        public readonly string updateUserDataUrl    = "User/Update";
        public readonly string getAllTeamWithMember = "Team/GetAllTeamsWithMembers";

        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();

        public event PropertyChangedEventHandler PropertyChanged;

        public ProfilePage()
            => this.InitializeComponent();

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.localSettings.Values["UserId"] != null)
            {
                User = await httpHelper
                             .GetByIdAsync<UserDTO>(getUserDataUrl, (int)App.localSettings.Values["UserId"]);
                var teams = await httpHelper.GetAsync<ObservableCollection<TeamsDTO>>(getAllTeamWithMember);
                TeamsCards.ItemsSource = teams.Where(t => t.Members.Any(m => m.id == User.id));
            }

            SkillsSymbol.Symbol = SkillsList.Items.Count > 0 ? Symbol.Edit : Symbol.Add;
            BioSymbol.Symbol = String.IsNullOrEmpty(User.bio) ? Symbol.Add : Symbol.Edit;

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
            var bioDialog = new BioDialog() { Content = User.bio };
            bioDialog.PrimaryButtonClick += BioDialog_PrimaryButtonClick;
            await bioDialog.ShowAsync();    
        }

        private void BioDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Bio.Text = (sender as BioDialog).Content;
            User.bio = Bio.Text;
        }

        private async void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            var skillsDialog = new SkillDialog();
            foreach (var user in User.skills)
            {
                skillsDialog.Skills.Add(user);
            }
            skillsDialog.PrimaryButtonClick += SkillsDialog_PrimaryButtonClick;
            await skillsDialog.ShowAsync();
        }
        private void SkillsDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var updatedSkills = (sender as SkillDialog).Skills;
            User.skills.Clear();
            foreach (var skill in updatedSkills)
            {
                User.skills.Add(skill);
            }
        }

        private async void UpdateInfo_Click(object sender, RoutedEventArgs e)
        {
            var updateResult = await httpHelper.UpdateAsync(updateUserDataUrl, User);
            if (updateResult == true)
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

        public static BitmapImage GetUserInfo(int userInfo, TeamsDTO team)
        {
            if (userInfo != 0)
            {
                var user = team.Members.FirstOrDefault(u => u.id == userInfo);
                return new BitmapImage(new Uri(user.profilePic));
            }
            else return null;
        }
    }
}
