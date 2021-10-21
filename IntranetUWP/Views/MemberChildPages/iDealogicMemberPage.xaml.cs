using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace IntranetUWP.Views.MemberChildPages
{
    public sealed partial class iDealogicMemberPage : Page, INotifyPropertyChanged
    {
        public iDealogicMemberPageViewModel vm = new iDealogicMemberPageViewModel();
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper(); 
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

        public ObservableCollection<SkillDTO> Skills { get; set; } = new ObservableCollection<SkillDTO>();
        public event PropertyChangedEventHandler PropertyChanged;
        public readonly string getTeamsByUserIdDataUrl = "UserTeam/GetTeamByUser";
        public iDealogicMemberPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            vm.Users.Clear();
            vm.Teams.Clear();
            await vm.BindUsersBackToUI();
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
            Skills.Clear();
            if (UsersCarousel.SelectedItem as UserDTO != null)
            {
                User = UsersCarousel.SelectedItem as UserDTO;
                var teams = await httpHelper.GetByIdAsync<ObservableCollection<TeamsDTO>>(getTeamsByUserIdDataUrl, User.id);
                Teams.Text = teams.Count == 0 ? "Not assigned to team yet" : string.Join(", ", teams.Select(t => t.TeamName));
                if(User != null && Skills.Count == 0)
                {
                    foreach (var skill in User.skills.Take(5)) { Skills.Add(skill); }
                }
                DisableAndDeleteBar.Visibility = Visibility.Visible;
            }
            else 
            { 
                DisableAndDeleteBar.Visibility = Visibility.Collapsed;
            }
        }

        private void MemberCarouselCard_NavigateMemberDetail(UserDTO user)
        {
            var container = UsersCarousel.ContainerFromItem(user);
            if (container != null)
            {
                ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", container as CarouselItem);
                Frame.Navigate(typeof(MemberDetailPage), user, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
