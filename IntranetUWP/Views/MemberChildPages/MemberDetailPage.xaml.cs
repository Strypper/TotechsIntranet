using IntranetUWP.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Numerics;
using IntranetUWP.Extensions;
using System.Collections.ObjectModel;
using IntranetUWP.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.DataTransfer;

namespace IntranetUWP.Views.MemberChildPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemberDetailPage : Page
    {
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public string getAllTeamWithMember = "Team/GetAllTeamsWithMembers";
        public string getUserById = "User/Get";
        public UserDTO User { get; set; } = new UserDTO();


        public MemberDetailPage()
         => this.InitializeComponent();

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
            Relationship.EnableImplicitAnimation(VisualPropertyType.Offset, 500);
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            User = e.Parameter as UserDTO;
            var anim = ConnectedAnimationService
                .GetForCurrentView()
                .GetAnimation("ForwardConnectedAnimation");
            if (anim != null)
            {
                anim.TryStart(Avatar, new UIElement[] { FullName });
            }

            await GetTeamsInfo(User);
        }

        private void SwipeItem_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            Frame.Navigate(typeof(iDealogicMemberPage), null, 
                           new SlideNavigationTransitionInfo() {  Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private async Task GetTeamsInfo(UserDTO userInfo)
        {
            var teams = await httpHelper.GetAsync<ObservableCollection<TeamsDTO>>(getAllTeamWithMember);
            TeamsCards.ItemsSource = teams.Where(t => t.Members.Any(m => m.id == userInfo.id));
        }

        private void NavigateBackToMembers_Click(object sender, RoutedEventArgs e)
           => Frame.Navigate(typeof(iDealogicMemberPage), null,
                           new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });

        public static BitmapImage GetUserInfo(int userInfo, TeamsDTO team)
        {
            if (userInfo != 0)
            {
                var user = team.Members.FirstOrDefault(u => u.id == userInfo);
                return new BitmapImage(new Uri(user.profilePic));
            }
            else return null;
        }

        private async void EmailHyperLink_Click(object sender, RoutedEventArgs e)
        {
            var emailMessage = new EmailMessage();
            emailMessage.Subject = "This is a subject";
            emailMessage.Body = "Hello, this is sample email body.";

            var emailRecipient = new EmailRecipient(User.userName);
            emailMessage.To.Add(emailRecipient);

            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        private async void CopyEmail_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(User.userName);
            Clipboard.SetContent(dataPackage);

            PageStatus.Title = "Copied Email:";
            PageStatus.Message = $"{User.userName} is now in your clipboard";
            PageStatus.Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success;
            PageStatus.IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Copy };
            PageStatus.IsOpen = true;
            await Task.Delay(3000);
            if (PageStatus.IsOpen == true)
            {
                PageStatus.IsOpen = false;
            }
        }
    }
}
