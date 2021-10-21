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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views.MemberChildPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeamsPage : Page
    {
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public readonly string getUsersDataUrl = "User/GetAll";
        public readonly string getAllTeamsWithMembersUrl = "Team/GetAllTeamsWithMembers";
        public readonly string createTeamDataUrl = "Team/CreatTeamWithUsers";
        public readonly string deleteTeamDataUrl = "Team/Delete";
        private IList<TeamsDTO> _source;
        private ObservableCollection<TeamsDTO> TeamsFiltered { get; }
        public bool IsBusy { get; set; }
        public TeamsPage()
        {
            InitializeComponent();
            TeamsFiltered = new ObservableCollection<TeamsDTO>();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _source = await httpHelper.GetAsync<IList<TeamsDTO>>(getAllTeamsWithMembersUrl);
            foreach (var team in _source)
                TeamsFiltered.Add(team);

        }


        private void TeamsFilter_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // is user typing?
            if (args.CheckCurrent())
            {
                List<TeamsDTO> TempFiltered = null;

                if (string.IsNullOrWhiteSpace(FilterTeamSearchBox.Text))
                {
                    TeamsFiltered.Clear();
                    foreach (TeamsDTO item in _source)
                    {
                        TeamsFiltered.Add(item);
                    }
                }
                else
                {
                    TempFiltered = _source.Where(contact
                        => contact.TeamName.Contains(FilterTeamSearchBox.Text,
                        StringComparison.InvariantCultureIgnoreCase)).ToList();


                    for (int i = TeamsFiltered.Count - 1; i >= 0; i--)
                    {
                        TeamsDTO item = TeamsFiltered[i];
                        if (!TempFiltered.Contains(item))
                        {
                            _ = TeamsFiltered.Remove(item);
                        }
                    }

                    foreach (TeamsDTO item in TempFiltered)
                    {
                        if (!TeamsFiltered.Contains(item))
                        {
                            TeamsFiltered.Add(item);
                        }
                    }

                }


            }
        }

        private async void CreateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var createTeamDialog = new CreateTeamDialog();
            createTeamDialog.Members = await httpHelper.GetAsync<ObservableCollection<UserDTO>>(getUsersDataUrl);
            createTeamDialog.PrimaryButtonClick += CreateTeamDialog_PrimaryButtonClick;
            await createTeamDialog.ShowAsync();
        }

        private async void TeamCard_DeleteTeam(int teamId)
        {
            var removeTeam = TeamsFiltered.FirstOrDefault(t => t.id == teamId);
            var confirmDeletButtonStyle = new Style(typeof(Button));
            confirmDeletButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, Colors.Red));
            confirmDeletButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, Colors.White));
            var confirmDeleteDialog = await new ContentDialog()
            {
                Title = "🗑 Delete this team ?",
                Content = $"Check before you delete {removeTeam.TeamName} ?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                PrimaryButtonStyle = confirmDeletButtonStyle,
                PrimaryButtonCommand = new RelayCommand(async () =>
                {
                    IsBusy = true;
                    if (removeTeam != null)
                    {
                        var deleteResult = await httpHelper.RemoveAsync(deleteTeamDataUrl, teamId);
                        if (deleteResult == true) TeamsFiltered.Remove(removeTeam);
                        else Debug.Write("Delete operation error");
                    }
                    IsBusy = false;
                })
            }.ShowAsync();

        }

        private async void CreateTeamDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var team = (sender as CreateTeamDialog).Team;
            var createResult = await httpHelper.CreateAsyncWithoutDTO<TeamsDTO>(createTeamDataUrl, team);

            if (createResult == true)
            {
                TeamsFiltered.Add(team);
            }
            else Debug.WriteLine("Create operation error");
        }
    }
}
