using IntranetUWP.Views.MemberChildPages;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemberPage : Page
    {
        public MemberPage()
        {
            this.InitializeComponent();
            MemberMainFrame.Navigate(typeof(iDealogicMemberPage));
        }

        private void NavigationViewPanel_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;
            NavView_Navigate(item);
        }

        private void NavView_Navigate(Microsoft.UI.Xaml.Controls.NavigationViewItem item)
        {
            switch (item.Name)
            {
                case "iDealogic":
                    MemberMainFrame
                        .Navigate(typeof(iDealogicMemberPage), 
                        null, 
                        new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
                    break;
                case "Devinition":
                    MemberMainFrame
                        .Navigate(typeof(iDealogicMemberPage), 
                        null, 
                        new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                    break;
                case "TeamPage":
                    MemberMainFrame.Navigate(typeof(TeamsPage), 
                        null, 
                        new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
                    break;
            }
        }
    }
}
