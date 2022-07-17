using IntranetUWP.Models;
using System.Numerics;
using Windows.ApplicationModel.Email;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class MemberCarouselCard : UserControl
    {
        public delegate void NavigateToMemberDetailEventHandler(UserDTO user);
        public UserDTO UserInfo
        {
            get { return (UserDTO)GetValue(UserInfoProperty); }
            set { SetValue(UserInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserInfoProperty =
            DependencyProperty.Register("UserInfo", typeof(UserDTO), typeof(MemberCarouselCard), new PropertyMetadata(null));

        public event NavigateToMemberDetailEventHandler NavigateMemberDetail;

        public MemberCarouselCard()
        {
            this.InitializeComponent();
        }

        private void DropShadowPanel_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            GradientLayer.Translation = new Vector3(0, 0, 0);
        }

        private void DropShadowPanel_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            GradientLayer.Translation = new Vector3(0, 45, 0);
        }

        private void NavigateToDetail_Click(object sender, RoutedEventArgs e)
            => NavigateMemberDetail?.Invoke(UserInfo);

        private async void SendEmail_ClickAsync(object sender, RoutedEventArgs e)
        {
            var emailMessage = new EmailMessage();
            emailMessage.Subject = "This is a subject";
            emailMessage.Body = "Hello, this is sample email body.";

            var emailRecipient = new EmailRecipient(UserInfo.UserName);
            emailMessage.To.Add(emailRecipient);

            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
    }
}
