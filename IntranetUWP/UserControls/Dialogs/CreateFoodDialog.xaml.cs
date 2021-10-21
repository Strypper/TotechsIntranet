using IntranetUWP.Models;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using System.Runtime.CompilerServices;

namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class CreateFoodDialog : ContentDialog
    {

        public FoodDTO Food{ get; set; } = new FoodDTO();

        public CreateFoodDialog()
        {
            this.InitializeComponent();
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Food = CreateFoodModule.Food;
        }
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args){}
    }
}
