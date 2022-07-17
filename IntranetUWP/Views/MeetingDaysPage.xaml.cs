using IntranetUWP.Models;
using IntranetUWP.UserControls.Dialogs;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IntranetUWP.Views
{
    public sealed partial class MeetingDaysPage : Page
    {
        public IList<MeetingScheduleDTO> MeetingSchedules { get; set; } = new List<MeetingScheduleDTO>();
        public MeetingDaysPage()
        {
            this.InitializeComponent();
        }

        private async void CreateNewMeeting_Click(object sender, RoutedEventArgs e)
        {
            var newMeetingDialog = new CreateMeetingContentDialog();
            await newMeetingDialog.ShowAsync();
        }
    }
}
