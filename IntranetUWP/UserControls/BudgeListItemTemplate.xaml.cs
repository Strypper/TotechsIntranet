using IntranetUWP.Helpers;
using IntranetUWP.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;


namespace IntranetUWP.UserControls
{
    public sealed partial class BudgeListItemTemplate : UserControl
    {
        public delegate void ApprovedContributionEventHandler(ContributionDTO contribution);

        public ContributionDTO Contribution
        {
            get { return (ContributionDTO)GetValue(ContributionProperty); }
            set { SetValue(ContributionProperty, value); }
        }

        public static readonly DependencyProperty ContributionProperty =
            DependencyProperty.Register("Contribution", typeof(ContributionDTO), typeof(BudgeListItemTemplate), null);

        //EventHandler
        public event ApprovedContributionEventHandler ApproveContributionEvent;
        public BudgeListItemTemplate()
        {
            this.InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void SwipeControl_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e) => FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);

        private void ProjectCard_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Avatar.Scale = new Vector3(1.08f, 1.08f, 0);

            EmployeeNameText.Translation = new Vector3(5, 0, 0);
            PaymentMethod.Translation = new Vector3(10, 0, 0);
            PaymentMethod.Opacity = 1;
            DateRange.Translation = new Vector3(5, 0, 0);
        }

        private void ProjectCard_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Avatar.Scale = new Vector3(1, 1, 0);

            EmployeeNameText.Translation = new Vector3(0, 10, 0);
            PaymentMethod.Translation = new Vector3(0, 0, 0);
            PaymentMethod.Opacity = 0;
            DateRange.Translation = new Vector3(0, -10, 0);
        }

        private void ApproveContribution_Click(object sender, RoutedEventArgs e)
        {
            ApproveContributionEvent?.Invoke(Contribution);
        }
    }
}
