using IntranetUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace IntranetUWP.UserControls
{
    public sealed partial class PasteFoodDialog : ContentDialog
    {
        private List<string> SplitedFoodList { get; set; }
        private ObservableCollection<FoodDTO> FoodListDialog = new ObservableCollection<FoodDTO>();
        public PasteFoodDialog(string listText)
        {
            this.InitializeComponent();
            SplitedFoodList = listText.Split("\r\n").ToList();
            foreach(string vietNameeseFoodName in SplitedFoodList)
            {
                FoodListDialog.Add(new FoodDTO() { foodName = vietNameeseFoodName });
            }
            FoodList.ItemsSource = FoodListDialog;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Debug.WriteLine(FoodListDialog);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void PastedFoodItemList_FoodEvent(FoodDTO food)
        {
            foreach(var f in FoodListDialog)
            {
                if(f == food)
                {
                    f.foodName = food.foodName;
                    f.foodEnglishName = food.foodEnglishName;
                    f.mainIcon = food.mainIcon;
                    f.secondaryIcon = food.secondaryIcon;
                }
            }
        }
    }
}
