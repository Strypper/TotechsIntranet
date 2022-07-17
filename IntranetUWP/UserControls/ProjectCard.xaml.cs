using IntranetUWP.Models;
using System;
using System.Linq;
using Windows.ApplicationModel.Email;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;


namespace IntranetUWP.UserControls
{
    public sealed partial class ProjectCard : UserControl
    {
        public delegate void DeleteProjectCardEventHandler(int projectId);
        public event DeleteProjectCardEventHandler DeleteProject;
        public ProjectDTO Project
        {
            get { return (ProjectDTO)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(ProjectDTO), typeof(ProjectDTO), new PropertyMetadata(null));


        public ProjectCard()
            => this.InitializeComponent();

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
            => DeleteProject?.Invoke(Project.id);

        private async void SendMailToWholeProject_Clicked(object sender, RoutedEventArgs e)
        {
            var emailMessage = new EmailMessage();
            emailMessage.Subject = "Welcome new member";
            emailMessage.Body = $"Dear {Project.ProjectName}";

            var emailRecipient = new EmailRecipient(string.Join("; ", Project.Members.Select(m => m.UserName)));
            emailMessage.To.Add(emailRecipient);

            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
        public static string LogoAndTechleadSwitcher(ProjectDTO project)
        {
            if (project != null)
            {
                var logo = project.ProjectLogo;
                if (logo == null || String.IsNullOrEmpty(logo) || String.IsNullOrWhiteSpace(logo))
                {
                    return "TechLead";
                }
                else return "Logo";
            }
            else return "TechLead";
        }

        public static BitmapImage TechLeadUrl(ProjectDTO project)
        {
            if (project != null)
            {
                var teachLead = project.Members.FirstOrDefault(m => m.Guid == project.TechLeadGuid);
                if (teachLead != null)
                {
                    return new BitmapImage(new Uri(teachLead.ProfilePic));
                }
                else return null;
            }
            else return null;
        }
    }
}
