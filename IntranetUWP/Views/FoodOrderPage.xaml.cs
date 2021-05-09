using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.UserControls;
using IntranetUWP.ViewModels.PagesViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace IntranetUWP.Views
{
    public sealed partial class FoodOrderPage : Page
    {
        public FoodOrderPageViewModel vm = new FoodOrderPageViewModel();
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        private UserDTO personalData = new UserDTO();
        private int usersFoodData;

        public FoodOrderPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var userFoodData = await httpHelper.GetAsync<ObservableCollection<UserFoodDTO>>(vm.getUserSelectedFoodDataUrl);
            var foodData = await httpHelper.GetAsync<ObservableCollection<FoodDTO>>(vm.getFoodsDataUrl);
            var usersData = await httpHelper.GetAsync<List<UserDTO>>(vm.getUsersDataUrl);
            usersFoodData = userFoodData.Count;
            personalData = usersData.Where(u => u.id == (int)App.localSettings.Values["UserId"]).FirstOrDefault();
            //Init user local values
            if (App.localSettings.Values["FirstName"] != null)
            {
                FirstWelcomeMessage.Text = "Welcome " + App.localSettings.Values["FirstName"] +"!";
            }
            else FirstWelcomeMessage.Text = "Welcome to Intranet ordering system";
            if (App.localSettings.Values["FoodId"] != null && (int)App.localSettings.Values["FoodId"] != 0)
            {
                var mainFood = foodData.Where(f => f.id == (int)App.localSettings.Values["FoodId"]).FirstOrDefault();
                PickedFoodText.Text = "You pick :";
                FoodIndexText.Text = (foodData.IndexOf(mainFood) + 1).ToString();
                FoodNameText.Text = mainFood.foodEnglishName;
                MainFoodImage.Source = new BitmapImage(new Uri(vm._mainFoods[mainFood.mainIcon]));
                if (mainFood.secondaryIcon != 11) SecondaryFoodImage.Source = new BitmapImage(new Uri(vm._secondaryFoods[mainFood.secondaryIcon]));
                else SecondaryFoodImage.Source = null;
            }
            else 
            { 
                PickedFoodText.Text = "Pick these below dishes";
                MainFoodImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/LunchFood.png"));
            }
            FadeIconIn.Begin();
        }

        private void FoodCard_ToggleClick(int foodId, bool isToggled)
        {
            WorkingBar.Visibility = Visibility.Visible;
            if(App.localSettings.Values["FoodId"] != null && (int)App.localSettings.Values["FoodId"] != 0)
            {
                //Case: Deselect current toggled food
                if((int)App.localSettings.Values["FoodId"] == foodId)
                {
                    usersFoodData--;
                    var deselectedFood = vm.Foods.Where(f => f.id == (int)App.localSettings.Values["FoodId"]).FirstOrDefault();
                    deselectedFood.IsSelected = false;
                    deselectedFood.NumberOfSelectedUser--;
                    deselectedFood.Percentage = (deselectedFood.NumberOfSelectedUser / usersFoodData) * 100;
                    foreach (FoodDTO f in vm.Foods)
                        if (f.id != deselectedFood.id) 
                            f.Percentage = (f.NumberOfSelectedUser / (usersFoodData)) * 100; 
                    App.localSettings.Values["FoodId"] = 0;
                    vm.NumberOfFood--;
                    vm.UserFoods.Remove(vm.UserFoods.Where(uf => uf.user.id == personalData.id).FirstOrDefault());
                    PickedFoodText.Text = "Please select below dishes";
                    FoodIndexText.Text = "0";
                    FoodNameText.Text = "";
                    MainFoodImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/LunchFood.png"));
                    SecondaryFoodImage.Source = null;
                    FadeIconIn.Begin();
                }
                //Case: From one toggled food to different food
                else
                {
                    vm.UserFoods.Remove(vm.UserFoods.Where(uf => uf.user.id == personalData.id).FirstOrDefault());
                    var newSelectedFood = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
                    var previousSelectedFood = vm.Foods.Where(f => f.id == (int)App.localSettings.Values["FoodId"]).FirstOrDefault();
                    newSelectedFood.IsSelected = true;
                    newSelectedFood.NumberOfSelectedUser++;
                    newSelectedFood.Percentage = (newSelectedFood.NumberOfSelectedUser / usersFoodData) * 100;
                    App.localSettings.Values["FoodId"] = foodId;
                    previousSelectedFood.IsSelected = false;
                    previousSelectedFood.NumberOfSelectedUser--;
                    previousSelectedFood.Percentage = (previousSelectedFood.NumberOfSelectedUser / usersFoodData) * 100;

                    var mainFood = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
                    PickedFoodText.Text = "You pick :";
                    FoodIndexText.Text = mainFood.itemNo.ToString();
                    FoodNameText.Text = mainFood.foodEnglishName;
                    MainFoodImage.Source = new BitmapImage(new Uri(vm._mainFoods[mainFood.mainIcon]));
                    if (mainFood.secondaryIcon != 11) SecondaryFoodImage.Source = new BitmapImage(new Uri(vm._secondaryFoods[mainFood.secondaryIcon]));
                    else SecondaryFoodImage.Source = null;
                    FadeIconIn.Begin();
                    vm.UserFoods.Insert(0, new UserFoodDTO()
                    {
                        user = personalData,
                        food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault(),
                        foodList = vm.Foods
                    });
                }
            }
            //No local food have choosen
            else
            {
                if (isToggled == true)
                {
                    usersFoodData++;
                    FoodImageContainer.Opacity = 0;
                    FoodGridView.SelectedItem = null;
                    vm.UserFoods.Insert(0, new UserFoodDTO()
                    {
                        user = personalData,
                        food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault(),
                        foodList = vm.Foods
                    });
                    foreach (FoodDTO dto in vm.Foods) if (dto.id != foodId)
                            dto.Percentage = (dto.NumberOfSelectedUser / (usersFoodData)) * 100;
                        else
                        {
                            dto.IsSelected = true;
                            dto.NumberOfSelectedUser += 1;
                            dto.Percentage = (dto.NumberOfSelectedUser / (usersFoodData)) * 100;
                            App.localSettings.Values["FoodId"] = foodId;
                            vm.NumberOfFood++;
                        }

                    var mainFood = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
                    PickedFoodText.Text = "You pick :";
                    FoodIndexText.Text = mainFood.itemNo.ToString();
                    FoodNameText.Text = mainFood.foodEnglishName;
                    MainFoodImage.Source = new BitmapImage(new Uri(vm._mainFoods[mainFood.mainIcon]));
                    if (mainFood.secondaryIcon != 11) SecondaryFoodImage.Source = new BitmapImage(new Uri(vm._secondaryFoods[mainFood.secondaryIcon]));
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
            vm.SelectedFood = food;
            var confirmDeletButtonStyle = new Style(typeof(Button));
            confirmDeletButtonStyle.Setters.Add(new Setter(BackgroundProperty, Colors.Red));
            confirmDeletButtonStyle.Setters.Add(new Setter(ForegroundProperty, Colors.White));
            var confirmDeleteDialog = await new ContentDialog()
            {
                Title = "🗑 Delete this food ?",
                Content = $"Check before you delete {food.foodEnglishName} ?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                PrimaryButtonStyle = confirmDeletButtonStyle,
                PrimaryButtonCommand = vm.deleteFoodCommand
            }.ShowAsync();
        }

        private async void FoodCard_EditSwipe(int foodId)
        {
            var food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
            CreateFood editFoodDialog = new CreateFood();
            editFoodDialog.Food = food;
            await editFoodDialog.ShowAsync();
        }

        private void FoodGridView_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem copyFromClipBoard = new MenuFlyoutItem { Text = "Copy from clipboard",
                Command = vm.getFoodFromClipboard,
                Icon = new FontIcon()
                    {
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        Glyph = "\xF0E3"
                }
                };
            
            myFlyout.Items.Add(copyFromClipBoard);

            //the code can show the flyout in your mouse click 
            myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }
    }
}
