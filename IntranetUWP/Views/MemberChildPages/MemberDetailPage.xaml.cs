using IntranetUWP.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views.MemberChildPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemberDetailPage : Page
    {
        public MemberDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var user = e.Parameter as UserDTO;
            if(user.profilePic != null)
            {
                Avatar.Source = new BitmapImage(new Uri(user.profilePic));
            }
            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            if (anim != null)
            {
                anim.TryStart(Avatar);
            }
        }

        private void SwipeItem_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            Frame.Navigate(typeof(iDealogicMemberPage), null, 
                           new SlideNavigationTransitionInfo() {  Effect = SlideNavigationTransitionEffect.FromLeft });
        }
    }
}
