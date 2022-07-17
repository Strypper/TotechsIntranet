using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.UserControls.Dialogs;
using IntranetUWP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace IntranetUWP.Views.MemberChildPages
{
    public sealed partial class ProjectsPage : Page
    {
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public readonly string getUsersDataUrl = "User/GetAll";
        public readonly string getAllProjectsWithMembersUrl = "Project/GetAllProjectsWithMembers";
        public readonly string createProjectDataUrl = "Project/CreatProjectWithUsers";
        public readonly string deleteProjectDataUrl = "Project/Delete";
        private IList<ProjectDTO> _source;
        private ObservableCollection<ProjectDTO> ProjectsFiltered { get; }
        public bool IsBusy { get; set; }
        public ProjectsPage()
        {
            InitializeComponent();
            ProjectsFiltered = new ObservableCollection<ProjectDTO>();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _source = await httpHelper.GetAsync<IList<ProjectDTO>>(getAllProjectsWithMembersUrl);
            foreach (var project in _source)
                ProjectsFiltered.Add(project);

        }


        private void ProjectsFilter_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // is user typing?
            if (args.CheckCurrent())
            {
                List<ProjectDTO> TempFiltered = null;

                if (string.IsNullOrWhiteSpace(FilterProjectSearchBox.Text))
                {
                    ProjectsFiltered.Clear();
                    foreach (ProjectDTO item in _source)
                    {
                        ProjectsFiltered.Add(item);
                    }
                }
                else
                {
                    TempFiltered = _source.Where(contact
                        => contact.ProjectName.Contains(FilterProjectSearchBox.Text,
                        StringComparison.InvariantCultureIgnoreCase)).ToList();


                    for (int i = ProjectsFiltered.Count - 1; i >= 0; i--)
                    {
                        ProjectDTO item = ProjectsFiltered[i];
                        if (!TempFiltered.Contains(item))
                        {
                            _ = ProjectsFiltered.Remove(item);
                        }
                    }

                    foreach (ProjectDTO item in TempFiltered)
                    {
                        if (!ProjectsFiltered.Contains(item))
                        {
                            ProjectsFiltered.Add(item);
                        }
                    }

                }


            }
        }

        private async void CreateProjectButton_Click(object sender, RoutedEventArgs e)
        {
            var createProjectDialog = new CreateProjectDialog();
            createProjectDialog.Members = await httpHelper.GetAsync<ObservableCollection<UserDTO>>(getUsersDataUrl);
            createProjectDialog.PrimaryButtonClick += CreateProjectDialog_PrimaryButtonClick;
            await createProjectDialog.ShowAsync();
        }

        private async void ProjectCard_DeleteProject(int projectId)
        {
            var removeProject = ProjectsFiltered.FirstOrDefault(t => t.id == projectId);
            var confirmDeletButtonStyle = new Style(typeof(Button));
            confirmDeletButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, Colors.Red));
            confirmDeletButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, Colors.White));
            var confirmDeleteDialog = await new ContentDialog()
            {
                Title = "🗑 Delete this project ?",
                Content = $"Check before you delete {removeProject.ProjectName} ?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                PrimaryButtonStyle = confirmDeletButtonStyle,
                PrimaryButtonCommand = new RelayCommand(async () =>
                {
                    IsBusy = true;
                    if (removeProject != null)
                    {
                        var deleteResult = await httpHelper.RemoveAsync(deleteProjectDataUrl, projectId);
                        if (deleteResult == true) ProjectsFiltered.Remove(removeProject);
                        else Debug.Write("Delete operation error");
                    }
                    IsBusy = false;
                })
            }.ShowAsync();

        }

        private async void CreateProjectDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var project = (sender as CreateProjectDialog).Project;
            var createResult = await httpHelper.CreateAsyncWithoutDTO<ProjectDTO>(createProjectDataUrl, project);

            if (createResult == true)
            {
                ProjectsFiltered.Add(project);
            }
            else Debug.WriteLine("Create operation error");
        }
    }
}
