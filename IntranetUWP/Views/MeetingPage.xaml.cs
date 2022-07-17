using IntranetUWP.Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace IntranetUWP.Views
{
    public sealed partial class MeetingPage : Page
    {
        public List<int> TestList { get; set; }

        public MeetingScheduleDTO MeetingSchedule
        {
            get { return (MeetingScheduleDTO)GetValue(MeetingScheduleProperty); }
            set { SetValue(MeetingScheduleProperty, value); }
        }

        public static readonly DependencyProperty MeetingScheduleProperty =
            DependencyProperty.Register("MeetingSchedule", 
                                        typeof(MeetingScheduleDTO), 
                                        typeof(MeetingPage), 
                                        null);


        public MeetingPage()
        {
            this.InitializeComponent();
            this.TestList = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                TestList.Add(i);
            }
        }
    }
}
