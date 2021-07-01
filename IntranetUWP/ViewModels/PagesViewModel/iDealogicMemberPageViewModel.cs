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
        public string getUsersDataUrl = "User/GetAll";
        public string deleteUserDataUrl = "User/Delete";
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper(); 
        private ObservableCollection<UserDTO> users = new ObservableCollection<UserDTO>();
        public ObservableCollection<UserDTO> Users { get; set; }
        public ICommand getAllUsersCommand { get; set; }
        public ICommand createNewUserCommand { get; set; }
        public ICommand askBeforeDeleteUserCommand { get; set; }
        public UserDTO SelectedUser { get; set; }
        public iDealogicMemberPageViewModel()
        {
            IsBusy = true;
            Users = new ObservableCollection<UserDTO>();

            createNewUserCommand = new RelayCommand(async() => await OpenCreateMemberDialog());
            askBeforeDeleteUserCommand = new RelayCommand(async () => await AskBeforeRemove());
            BindUsersBackToUI();
        }

        private async Task GetUserData() => users = await httpHelper.GetAsync<ObservableCollection<UserDTO>>(getUsersDataUrl);

        private async Task BindUsersBackToUI()
        {
            await GetUserData();
            foreach (var user in users)
            {
                Users.Add(user);
            }
            IsBusy = true;
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
