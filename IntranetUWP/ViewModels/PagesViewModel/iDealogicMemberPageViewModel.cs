using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.RefitInterfaces;
using IntranetUWP.UserControls.Dialogs;
using IntranetUWP.ViewModels.Commands;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public readonly string getAllProjectDataUrl = "Project/GetAll";
        public readonly string getProjectByUserIdDataUrl = "UserProjects/GetProjectsByUser";
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper(); 
        private List<UserDTO> users = new List<UserDTO>();
        public ObservableCollection<UserDTO> Users { get; set; }
        private List<ProjectDTO> projects = new List<ProjectDTO>();
        public ObservableCollection<ProjectDTO> Projects { get; set; }
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
        private readonly IUserData userData = RestService.For<IUserData>(App.BaseUrl);
        public iDealogicMemberPageViewModel()
        {
            IsBusy = false;
            Users = new ObservableCollection<UserDTO>();
            Projects = new ObservableCollection<ProjectDTO>();

            createNewUserCommand = new RelayCommand(async() => await OpenCreateMemberDialog());
            askBeforeDeleteUserCommand = new RelayCommand(async () => await AskBeforeRemove());
        }

        private async Task GetUserData() => users = await httpHelper.GetAsync<List<UserDTO>>(getUsersDataUrl);

        private async Task GetProjectData() => projects = await httpHelper.GetAsync<List<ProjectDTO>>(getAllProjectDataUrl);

        public async Task BindUsersBackToUI()
        {
            await GetUserData();
            await GetProjectData();
            users.ForEach(u => Users.Add(u));
            projects.ForEach(t => Projects.Add(t));
            var userGuid = App.localSettings.Values["UserGuid"].ToString();
            if (userGuid != null && Users.Count > 0)
            {
                SelectedUser = userGuid != null ? Users.FirstOrDefault(u => u.Guid == userGuid) : null;
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
                    Content = $"Check before you delete {removeUser.FirstName} ?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No",
                    PrimaryButtonStyle = confirmDeletButtonStyle,
                    PrimaryButtonCommand = new RelayCommand(async () =>
                    {
                        IsBusy = true;
                        if (removeUser != null)
                        {
                            var user = Users.Where(u => u == removeUser).FirstOrDefault();
                            var deleteResult = await userData.DeleteUser(user.Guid);
                            if (deleteResult.StatusCode == HttpStatusCode.NoContent) Users.Remove(user); else Debug.Write("Delete operation error");
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
