using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FoodOrderPage : Page
    {
        public FoodOrderPageViewModel vm = new FoodOrderPageViewModel();
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        private int users;

        public FoodOrderPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var userFoodData = await httpHelper.GetAsync<ObservableCollection<UserFoodDTO>>(vm.getUserSelectedFoodDataUrl);
            users = userFoodData.Count;
        }

        private void FoodCard_ToggleClick(int foodId, bool isToggled)
        {
            WorkingBar.Visibility = Visibility.Visible;
            if(App.localSettings.Values["FoodId"] != null && (int)App.localSettings.Values["FoodId"] != 0)
            {
                //Case: Deselect current toggled food
                if((int)App.localSettings.Values["FoodId"] == foodId)
                {
                    users--;
                    var deselectedFood = vm.Foods.Where(f => f.id == (int)App.localSettings.Values["FoodId"]).FirstOrDefault();
                    deselectedFood.IsSelected = false;
                    deselectedFood.numberOfSelectedUser--;
                    deselectedFood.Percentage = (deselectedFood.numberOfSelectedUser / users) * 100;
                    foreach (FoodDTO f in vm.Foods)
                        if (f.id != deselectedFood.id) 
                            f.Percentage = (f.numberOfSelectedUser / (users)) * 100; 
                    App.localSettings.Values["FoodId"] = 0;
                }
                //Case: From one toggled food to different food
                else
                {
                    var newSelectedFood = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
                    var previousSelectedFood = vm.Foods.Where(f => f.id == (int)App.localSettings.Values["FoodId"]).FirstOrDefault();
                    newSelectedFood.IsSelected = true;
                    newSelectedFood.numberOfSelectedUser++;
                    newSelectedFood.Percentage = (newSelectedFood.numberOfSelectedUser / users) * 100;
                    App.localSettings.Values["FoodId"] = foodId;
                    previousSelectedFood.IsSelected = false;
                    previousSelectedFood.numberOfSelectedUser--;
                    previousSelectedFood.Percentage = (previousSelectedFood.numberOfSelectedUser / users) * 100;
                }
            }
            //No local food have choosen
            else
            {
                if (isToggled == true)
                {
                    users++;
                    FoodImageContainer.Opacity = 0;
                    FoodGridView.SelectedItem = null;
                    foreach (FoodDTO dto in vm.Foods) if (dto.id != foodId)
                            dto.Percentage = (dto.numberOfSelectedUser / (users)) * 100;
                        else
                        {
                            dto.IsSelected = true;
                            dto.numberOfSelectedUser += 1;
                            dto.Percentage = (dto.numberOfSelectedUser / (users)) * 100;
                            App.localSettings.Values["FoodId"] = foodId;
                        }
                    var mainFoodIcon = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
                    MainFoodImage.Source = new BitmapImage(new Uri(vm._mainFoods[mainFoodIcon.mainIcon]));
                    if (mainFoodIcon.secondaryIcon != 11) SecondaryFoodImage.Source = new BitmapImage(new Uri(vm._secondaryFoods[mainFoodIcon.secondaryIcon]));
                    else SecondaryFoodImage.Source = null;
                    FadeIconIn.Begin();
                    WorkingBar.Visibility = Visibility.Collapsed;
                }
            }
            WorkingBar.Visibility = Visibility.Collapsed;
        }

        private void FoodGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FoodGridView.SelectedItem != null) 
            {
                EditFood.IsEnabled = true;
                DeleteFood.IsEnabled = true;
            }
        }
        private void FoodGridView_LostFocus(object sender, RoutedEventArgs e) { FoodGridView.SelectedItem = null; EditFood.IsEnabled = false; DeleteFood.IsEnabled = false; }

        private async void FoodCard_DeleteSwipe(int foodId)
        {
            var food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
            var deleteResult = await httpHelper.RemoveAsync(vm.deleteFoodDataUrl, food.id);
            if (deleteResult == true) vm.Foods.Remove(food); else Debug.Write("Delete operation error");
        }

        private void FoodGridView_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem copyFromClipBoard = new MenuFlyoutItem { Text = "Copy from clipboard",
                Command = vm.getFoodFromExcel,
                Icon = new FontIcon()
                    {
                        FontFamily = new FontFamily("Segoe Fluent Icons"),
                        Glyph = "\xF0E3"
                    }
                };
            
            myFlyout.Items.Add(copyFromClipBoard);

            //the code can show the flyout in your mouse click 
            myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }
    }
}
