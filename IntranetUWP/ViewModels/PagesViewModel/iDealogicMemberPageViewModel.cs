using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.UserControls.Dialogs;
using IntranetUWP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IntranetUWP.ViewModels.PagesViewModel
{
    public class iDealogicMemberPageViewModel : ViewModelBase
    {
        public readonly string getUsersDataUrl = "User/GetAll";
        public readonly string deleteUserDataUrl = "User/Delete";
        public readonly string getAllTeamsDataUrl = "Team/GetAll";
        public readonly string getTeamsByUserIdDataUrl = "UserTeam/GetTeamByUser";
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper(); 
        private List<UserDTO> users = new List<UserDTO>();
        public ObservableCollection<UserDTO> Users { get; set; }
        private List<TeamsDTO> teams = new List<TeamsDTO>();
        public ObservableCollection<TeamsDTO> Teams { get; set; }
        public ICommand getAllUsersCommand { get; set; }
        public ICommand createNewUserCommand { get; set; }
        public ICommand askBeforeDeleteUserCommand { get; set; }
        private UserDTO selectedUser;

        public UserDTO SelectedUser
        {
            get { return selectedUser; }
            set 
            { 
                selectedUser = value;
                OnPropertyChanged();
            }
        }

        public iDealogicMemberPageViewModel()
        {
            IsBusy = false;
            Users = new ObservableCollection<UserDTO>();
            Teams = new ObservableCollection<TeamsDTO>();

            createNewUserCommand = new RelayCommand(async() => await OpenCreateMemberDialog());
            askBeforeDeleteUserCommand = new RelayCommand(async () => await AskBeforeRemove());
        }

        private async Task GetUserData() => users = await httpHelper.GetAsync<List<UserDTO>>(getUsersDataUrl);

        private async Task GetTeamsData() => teams = await httpHelper.GetAsync<List<TeamsDTO>>(getAllTeamsDataUrl);

        public async Task BindUsersBackToUI()
        {
            await GetUserData();
            await GetTeamsData();
            users.ForEach(u => Users.Add(u));
            teams.ForEach(t => Teams.Add(t));
            var userId = App.localSettings.Values["UserId"];
            if (userId != null && Users.Count > 0)
            {
                SelectedUser = userId != null ? Users.FirstOrDefault(u => u.id == (int)userId) : null;
            }
            IsBusy = false;
        }

        private async Task AskBeforeRemove()
        {
            var removeUser = SelectedUser;
            if(removeUser != null)
            {
                var confirmDeletButtonStyle = new Style(typeof(Button));
                confirmDeletButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, Colors.Red));
                confirmDeletButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, Colors.White));
                var confirmDeleteDialog = await new ContentDialog()
                {
                    Title = "🗑 Delete this user ?",
                    Content = $"Check before you delete {removeUser.firstName} ?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No",
                    PrimaryButtonStyle = confirmDeletButtonStyle,
                    PrimaryButtonCommand = new RelayCommand(async () =>
                    {
                        IsBusy = true;
                        if (removeUser != null)
                        {
                            var food = Users.Where(u => u == removeUser).FirstOrDefault();
                            var deleteResult = await httpHelper.RemoveAsync(deleteUserDataUrl, food.id);
                            if (deleteResult == true) Users.Remove(food); else Debug.Write("Delete operation error");
                        }
                        IsBusy = false;
                    })
                }.ShowAsync();
            }
        }

        private async Task OpenCreateMemberDialog()
        {
            CreateUserContentDialog createUserDialog = new CreateUserContentDialog();
            await createUserDialog.ShowAsync();
        }
    }
}
