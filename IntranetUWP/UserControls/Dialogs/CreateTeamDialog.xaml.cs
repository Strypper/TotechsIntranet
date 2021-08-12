using IntranetUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media.Imaging;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class CreateTeamDialog : ContentDialog
    {
        public ObservableCollection<UserDTO> Members
        {
            get { return (ObservableCollection<UserDTO>)GetValue(MembersProperty); }
            set { SetValue(MembersProperty, value); }
        }

        public static readonly DependencyProperty MembersProperty =
            DependencyProperty.Register("Members",
                typeof(ObservableCollection<UserDTO>),
                typeof(PreviewAvatarsGroup),
                new PropertyMetadata(new ObservableCollection<UserDTO>()));

        public TeamsDTO Team { get; set; } = new TeamsDTO();

        public CreateTeamDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var teamAbout = string.Empty;
            AboutTeam.Document.GetText(TextGetOptions.None, out teamAbout);
            foreach (UserDTO member in MemberList.SelectedItems)
            {
                Team.Members.Add(member);
            }

            Team.TeamName = TeamName.Text;
            Team.About = teamAbout;
            Team.Clients = Clients.Text;
            Team.Company = (bool)iDealogicToggle.IsChecked ? true : false;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
        private void iDealogicToggle_Click(object sender, RoutedEventArgs e)
        {
            iDealogicToggle.IsChecked = true;
            DevinitionToggle.IsChecked = false;
        }

        private void DeviToggle_Click(object sender, RoutedEventArgs e)
        {
            DevinitionToggle.IsChecked = true;
            iDealogicToggle.IsChecked = false;
        }

        private void MemberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TeachLeadText.Visibility = MemberList.SelectedItems.Count != 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Avatars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = (sender as GridView).SelectedItem as UserDTO;
            if(data != null)
            {
                Avatar.AvatarImage = new BitmapImage(new Uri(data.profilePic));
                TechLeadName.Text = data.lastName + " " + data.middleName + " " + data.firstName;
                Team.TechLead = data.id;
            }
        }
    }
}
