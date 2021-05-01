using IntranetUWP.Models;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views.MemberChildPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class iDealogicMemberPage : Page
    {
        public List<UserDTO> Users { get; set; }
        public iDealogicMemberPage()
        {
            this.InitializeComponent();
            Users = DemoUserData.getData();
        }
    }
}
