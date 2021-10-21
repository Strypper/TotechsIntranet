using Windows.UI.Text;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class BioDialog : ContentDialog
    {
        private string BioContent;
        public string Content { get; set; }
        public BioDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Bio.Document.Selection.Text = Content == null ? "" : Content;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Bio.Document.GetText(TextGetOptions.None, out BioContent);
            Content = BioContent;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
