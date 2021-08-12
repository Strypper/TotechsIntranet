using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace IntranetUWP.Views.MemberChildPages
{
    public sealed partial class iDealogicMemberPage : Page
    {
        public iDealogicMemberPageViewModel vm = new iDealogicMemberPageViewModel();
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public string getTeamsByUserIdDataUrl = "UserTeam/GetTeamByUser";
        public iDealogicMemberPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var userId = App.localSettings.Values["UserId"];
            UsersCarousel.SelectedItem = userId == null ? null : vm.Users.FirstOrDefault(u => u.id == (int)userId);
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ConnectedAnimation animation =
                ConnectedAnimationService.GetForCurrentView().GetAnimation("backAnimation");
            if (animation != null)
            {
                //await UsersCarousel.TryStartConnectedAnimationAsync();
            }
        }

        private void NavigateToMemberDetail_Click(object sender, RoutedEventArgs e)
        {
            var container = UsersCarousel.ContainerFromItem(UsersCarousel.SelectedItem);
            if(container != null)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", container as CarouselItem);
                Frame.Navigate(typeof(MemberDetailPage), UsersCarousel.SelectedItem, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
        }

        private async void UsersCarousel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersCarousel.SelectedItem as UserDTO != null)
            {
                var userInfo = UsersCarousel.SelectedItem as UserDTO;
                var teams = await httpHelper.GetByIdAsync<ObservableCollection<TeamsDTO>>(getTeamsByUserIdDataUrl, userInfo.id);
                LastName.Text = userInfo.lastName == null ? "" : userInfo.lastName;
                MiddleName.Text = userInfo.middleName == null ? "" : userInfo.middleName;
                FirstName.Text = userInfo.firstName == null ? "" : userInfo.firstName;
                Bio.Text = userInfo.bio == null ? "" : userInfo.bio;
                Role.Text = userInfo.role == null ? "" : userInfo.role;
                Level.Text = userInfo.level == null ? "" : userInfo.level;
                Teams.Text = teams.Count == 0 ? "Not assigned to team yet" : string.Join(", ", teams.Select(t => t.TeamName));
                DetailButton.IsEnabled = true;
                DisableAndDeleteBar.Visibility = Visibility.Visible;
            }
            else 
            { 
                DetailButton.IsEnabled = false;
                DisableAndDeleteBar.Visibility = Visibility.Collapsed;
            }
        }
    }
}
