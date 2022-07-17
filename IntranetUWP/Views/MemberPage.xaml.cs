using IntranetUWP.Views.MemberChildPages;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace IntranetUWP.Views
{
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
                case "AllMembers":
                    MemberMainFrame
                        .Navigate(typeof(iDealogicMemberPage), 
                                        null, 
                                        new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
                    break;
            }
        }
    }
}
