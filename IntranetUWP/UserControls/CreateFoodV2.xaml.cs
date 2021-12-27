using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IntranetUWP.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class CreateFoodV2 : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<FoodIconModel> primaryFoodList { get; set; } = new ObservableCollection<FoodIconModel>();
        private ObservableCollection<FoodIconModel> secondaryFoodList { get; set; } = new ObservableCollection<FoodIconModel>();
        public FoodDTO Food { get; set; } = new FoodDTO() { mainIcon = 5 };

        public CreateFoodV2()
        {
            this.InitializeComponent();
            secondaryFoodList = FoodIconData.getSecondaryFoodIcons();
            primaryFoodList = FoodIconData.getPrimaryFoodIcons();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FoodNav.SelectedItem = primaryFoodList.FirstOrDefault(f => f.FoodId == 1);
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
            => e.AcceptedOperation = DataPackageOperation.Move;

        private void SecondaryFoodGrid_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            PrimaryFood.Scale = new Vector3(1, 1, 0);
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            var secondFoods = e.DataView.Properties["SecondaryFood"] as FoodIconModel;
            if (secondFoods != null)
            {
                if (SecondaryFood.Text != "")
                {
                    DropIconOut.Begin();
                    DropIconOut.Completed += (s, a) =>
                    {
                        DropIconOut.Stop();
                        TranslateTransform.X = 0;
                        TranslateTransform.Y = 0;
                        SecondaryFood.Text = secondFoods.Icon;
                        FadeIconIn.Begin();
                        DropIconIn.Begin();
                    };
                }
                else
                {
                    SecondaryFood.Text = secondFoods.Icon;
                    FadeIconIn.Begin();
                    DropIconIn.Begin();
                }
                Food.secondaryIcon = secondFoods.FoodId;
            }
        }

        private void SecondaryFoodGrid_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            PrimaryFood.Scale = new Vector3(0.95f, 0.95f, 0);
            var secondaryFood = e.Items[0] as FoodIconModel;
            e.Data.Properties.Add("SecondaryFood", secondaryFood);
        }

        private void FoodNav_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                var item = args.SelectedItem as FoodIconModel;
                PrimaryFood.Text = navItemTag;
                FadePrimaryIconIn.Begin();
                //If not rice selection
                if (navItemTag != "\U0001F35A")
                {
                    SecondaryFoodGrid.IsEnabled = false;
                    SecondaryFoodGrid.Visibility = Visibility.Collapsed;
                    DragInstruction.Visibility = Visibility.Collapsed;
                    FoodImage.SetValue(Grid.ColumnSpanProperty, 2);
                    DropIconOut.Begin();
                    DropIconOut.Completed += (s, a) =>
                    {
                        DropIconOut.Stop();
                        TranslateTransform.X = 0;
                        TranslateTransform.Y = 0;
                        SecondaryFood.Text = "";
                    };
                }
                else
                {
                    SecondaryFoodGrid.IsEnabled = true;
                    SecondaryFoodGrid.Visibility = Visibility.Visible;
                    DragInstruction.Visibility = Visibility.Visible;
                    FoodImage.SetValue(Grid.ColumnSpanProperty, 1);

                    Food.secondaryIcon = null;
                }
                Food.mainIcon = item.FoodId;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void VietnameseFoodName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vnText = sender as TextBox;
            Food.foodName = vnText.Text;
        }

        private void EnglishFoodName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var engText = sender as TextBox;
            Food.foodEnglishName = engText.Text;
        }
    }

    public class FoodIconModel
    {
        public int FoodId { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
    }

    public class FoodIconData
    {
        public static ObservableCollection<FoodIconModel> getPrimaryFoodIcons()
        {
            var data = new ObservableCollection<FoodIconModel>();
            data.Add(new FoodIconModel() { FoodId = 5, Icon = "\U0001F371", Name = "DEFAULT" });
            data.Add(new FoodIconModel() { FoodId = 1, Icon = "\U0001F35A", Name = "RICE" });
            data.Add(new FoodIconModel() { FoodId = 2, Icon = "\U0001F956", Name = "BREAD" });
            data.Add(new FoodIconModel() { FoodId = 3, Icon = "\U0001F35D", Name = "SPAGHETTI" });
            data.Add(new FoodIconModel() { FoodId = 4, Icon = "\U0001F35C", Name = "NOODLE" });
            data.Add(new FoodIconModel() { FoodId = 6, Icon = "\U0001F372", Name = "SOUP" });
            data.Add(new FoodIconModel() { FoodId = 7, Icon = "\U0001F957", Name = "SALAD" });
            return data;
        }

        public static ObservableCollection<FoodIconModel> getSecondaryFoodIcons()
        {
            var data = new ObservableCollection<FoodIconModel>();
            data.Add(new FoodIconModel() { FoodId = 6, Icon = "\U0001F969", Name = "MEAT" });
            data.Add(new FoodIconModel() { FoodId = 2, Icon = "\U0001F953", Name = "BACON" });
            data.Add(new FoodIconModel() { FoodId = 7, Icon = "\U0001F357", Name = "CHICKEN" });
            data.Add(new FoodIconModel() { FoodId = 8, Icon = "\U0001F373", Name = "EGG" });
            data.Add(new FoodIconModel() { FoodId = 9, Icon = "\U0001F364", Name = "SHRIMP" });
            data.Add(new FoodIconModel() { FoodId = 11, Icon = "\U0001F41F", Name = "FISH" });
            data.Add(new FoodIconModel() { FoodId = 10, Icon = "\U0001F9C6", Name = "FALAFEL" });
            data.Add(new FoodIconModel() { FoodId = 8, Icon = "\U0001F33F", Name = "VEGETABLE" });
            return data;
        }
    }
}
