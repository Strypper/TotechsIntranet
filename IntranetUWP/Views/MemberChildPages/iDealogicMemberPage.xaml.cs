using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace IntranetUWP.Views.MemberChildPages
{
    public sealed partial class iDealogicMemberPage : Page
    {
        public iDealogicMemberPageViewModel vm = new iDealogicMemberPageViewModel();
        public iDealogicMemberPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
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

        private void UsersCarousel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersCarousel.SelectedItem as UserDTO != null)
            {
                DetailButton.IsEnabled = true;
                DisableAndDeleteBar.Visibility = Visibility.Visible;
            }
            else 
            { 
                DetailButton.IsEnabled = false;
                DisableAndDeleteBar.Visibility = Visibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
