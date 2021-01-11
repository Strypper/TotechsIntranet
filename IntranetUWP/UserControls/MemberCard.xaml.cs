using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class MemberCard : UserControl
    {
        public int UserId
        {
            get { return (int)GetValue(UserIdProperty); }
            set { SetValue(UserIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for userId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserIdProperty =
            DependencyProperty.Register("UserId", typeof(int), typeof(MemberCard), null);

        public string ProfilePictureUrl
        {
            get { return (string)GetValue(ProfilePictureUrlProperty); }
            set { SetValue(ProfilePictureUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProfilePictureUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfilePictureUrlProperty =
            DependencyProperty.Register("ProfilePictureUrl", typeof(string), typeof(MemberCard), null);

        public string EmployeeName
        {
            get { return (string)GetValue(EmployeeNameProperty); }
            set { SetValue(EmployeeNameProperty, value); }
        }

        public static readonly DependencyProperty EmployeeNameProperty =
            DependencyProperty.Register("EmployeeName", typeof(string), typeof(MemberCard), null);

        public MemberCard()
        {
            this.InitializeComponent();
        }
    }
}
