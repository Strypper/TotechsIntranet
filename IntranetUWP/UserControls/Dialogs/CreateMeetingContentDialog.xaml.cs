using IntranetUWP.Models;
using IntranetUWP.RefitInterfaces;
using Refit;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;


namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class CreateMeetingContentDialog : ContentDialog
    {
        private readonly IUserData userData = RestService.For<IUserData>(App.BaseUrl);
        public ObservableCollection<UserDTO> AllMembers { get; set; } = new ObservableCollection<UserDTO>();
        public CreateMeetingContentDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var allMembers = await userData.GetAllUsers();
            allMembers.ForEach(member => AllMembers.Add(member));
        }
    }
}
