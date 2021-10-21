using IntranetUWP.Models;
using System;
using System.Linq;
using Windows.ApplicationModel.Email;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class TeamCard : UserControl
    {
        public delegate void DeleteTeamCardEventHandler(int teamId);
        public event DeleteTeamCardEventHandler DeleteTeam;
        public TeamsDTO Team
        {
            get { return (TeamsDTO)GetValue(TeamProperty); }
            set { SetValue(TeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Teams.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TeamProperty =
            DependencyProperty.Register("Team", typeof(TeamsDTO), typeof(TeamsDTO), new PropertyMetadata(null));


        public TeamCard()
            => this.InitializeComponent();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Team != null)
            {
                var teachLead = Team.Members.FirstOrDefault(m => m.id == Team.TechLead);
                if (teachLead != null)
                {
                    TeamAvatar.ProfilePicture = new BitmapImage(new Uri(teachLead.profilePic));
                }
            }
        }

        private void DeleteTeam_Click(object sender, RoutedEventArgs e)
            => DeleteTeam?.Invoke(Team.id);

        private async void SendMailToWholeTeam_Clicked(object sender, RoutedEventArgs e)
        {
            var emailMessage = new EmailMessage();
            emailMessage.Subject = "Welcome new member";
            emailMessage.Body = $"Dear {Team.TeamName}";

            var emailRecipient = new EmailRecipient(string.Join("; ", Team.Members.Select(m => m.userName)));
            emailMessage.To.Add(emailRecipient);

            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
    }
}
