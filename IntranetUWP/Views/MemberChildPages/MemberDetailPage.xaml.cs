using IntranetUWP.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Numerics;
using IntranetUWP.Extensions;

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
            if (user.profilePic != null)
            {
                Avatar.Source = new BitmapImage(new Uri(user.profilePic));
            }
            UserName.Content = user.userName;
            CompanyImage.Source = new BitmapImage(new Uri(user.company == true
                ? "ms-appx:///Assets/iDealogic.png"
                : "ms-appx:///Assets/Devinition.png"));
            LastName.Text = user.lastName;
            MiddleName.Text = user.middleName;
            FirstName.Text = user.firstName;
            var anim = ConnectedAnimationService
                .GetForCurrentView()
                .GetAnimation("ForwardConnectedAnimation");
            if (anim != null)
            {
                anim.TryStart(Avatar, new UIElement[] { FullName });
            }
        }

        private void SwipeItem_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            Frame.Navigate(typeof(iDealogicMemberPage), null, 
                           new SlideNavigationTransitionInfo() {  Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CompanyImageBorder.Translation = new Vector3(0, 0, 0);


            var compositor = this.Visual().Compositor;

            // Create background visuals.
            var leftBackgroundVisual = compositor.CreateSpriteVisual();

            var destinationBrush = compositor.CreateBackdropBrush();

            var secondMiddleBackgroundVisual = compositor.CreateSpriteVisual();
            var skillListVisual = compositor.CreateSpriteVisual();

            SkillsList.SizeChanged += (s, ev) => skillListVisual.Size = ev.NewSize.ToVector2();
            Avatar.SizeChanged += (s, ev) => skillListVisual.Size = ev.NewSize.ToVector2();

            // Enable implilcit Offset and Size animations.
            Age.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            SpecialRole.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            Role.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            Level.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            UserName.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            PhoneNumber.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            Country.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            FormerWork.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            Hobby.EnableImplicitAnimation(VisualPropertyType.Offset, 500);

            BioHeader.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            SkillsHeader.EnableImplicitAnimation(VisualPropertyType.Offset, 500);

            ThirdColumn.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            ThirdColumnUserInfoGrid.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            ThirdColumnImageGrid.EnableImplicitAnimation(VisualPropertyType.Offset, 500);

            SecondTopColumn.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            SecondBottomColumn.EnableImplicitAnimation(VisualPropertyType.Offset, 500);


            BioSection.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            SkillsList.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
            Avatar.EnableImplicitAnimation(VisualPropertyType.Offset, 500);

            leftBackgroundVisual.EnableImplicitAnimation(VisualPropertyType.Size, 500);
            skillListVisual.EnableImplicitAnimation(VisualPropertyType.Size, 500);


            // Enable implicit Visible/Collapsed animations.
            BioSection.EnableFluidVisibilityAnimation(showFromScale: 0.6f, hideToScale: 0.8f, showDelay: 200, showDuration: 500, hideDuration: 500);
            SecondTopColumn.EnableFluidVisibilityAnimation(showFromScale: 0.6f, hideToScale: 0.8f, showDelay: 200, showDuration: 500, hideDuration: 500);
            SecondBottomColumn.EnableFluidVisibilityAnimation(showFromScale: 0.6f, hideToScale: 0.8f, showDelay: 200, showDuration: 500, hideDuration: 500);
            BioSection.EnableFluidVisibilityAnimation(showFromScale: 0.6f, hideToScale: 0.8f, showDelay: 400, showDuration: 500, hideDuration: 500);
            SkillsList.EnableFluidVisibilityAnimation(showFromScale: 0.6f, hideToScale: 0.8f, showDelay: 400, showDuration: 500, hideDuration: 500);
        }

        private void NavigateBackToMembers_Click(object sender, RoutedEventArgs e)
           => Frame.Navigate(typeof(iDealogicMemberPage), null,
                           new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
    }
}
