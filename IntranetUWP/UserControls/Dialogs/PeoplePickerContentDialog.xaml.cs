using IntranetUWP.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class PeoplePickerContentDialog : ContentDialog
    {


        public ObservableCollection<UserDTO> UsersList
        {
            get { return (ObservableCollection<UserDTO>)GetValue(UsersListProperty); }
            set { SetValue(UsersListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UsersList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsersListProperty =
            DependencyProperty.Register("UsersList", 
                                        typeof(ObservableCollection<UserDTO>), 
                                        typeof(PeoplePickerContentDialog), 
                                        new PropertyMetadata(new ObservableCollection<UserDTO>()));


        public PeoplePickerContentDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
