using IntranetUWP.Models;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;


namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class CreateProjectDialog : ContentDialog
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

        public ProjectDTO Project { get; set; } = new ProjectDTO();

        public CreateProjectDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var projectAbout = string.Empty;
            AboutProject.Document.GetText(TextGetOptions.None, out projectAbout);
            foreach (UserDTO member in MemberList.SelectedItems)
            {
                Project.Members.Add(member);
            }

            Project.ProjectName = ProjectName.Text;
            Project.About = projectAbout;
            Project.Clients = Clients.Text;
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
                Avatar.AvatarImage   = new BitmapImage(new Uri(data.ProfilePic));
                TechLeadName.Text    = data.FullName;
                Project.TechLeadGuid = data.Guid;
            }
        }
    }
}
