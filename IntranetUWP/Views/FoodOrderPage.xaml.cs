using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        string getUserSelectedFoodDataUrl = "UserFood/GetAll";
        Random ran = new Random();
        private int users;
        private readonly IDictionary<int, string> _mainFoods = new Dictionary<int, string>
        {
            { 1, "ms-appx:///Assets/FoodAssets/Rice.png"},
            { 2, "ms-appx:///Assets/FoodAssets/Bread.png"},
            { 3, "ms-appx:///Assets/FoodAssets/Spagheti.png"},
            { 4, "ms-appx:///Assets/FoodAssets/Noodle.png"},
            { 5, "ms-appx:///Assets/FoodAssets/LunchFood.png"}
        };

        private readonly IDictionary<int?, string> _secondaryFoods = new Dictionary<int?, string>
        {
            { 6, "ms-appx:///Assets/FoodAssets/Meat.png"},
            { 7, "ms-appx:///Assets/FoodAssets/Chicken.png"},
            { 8, "ms-appx:///Assets/FoodAssets/Egg.png"},
            { 9, "ms-appx:///Assets/FoodAssets/Shrimp.png"},
            { 10, "ms-appx:///Assets/FoodAssets/Falafel.png"},
            { 11, null }
        };
        public FoodOrderPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var userFoodData = await httpHelper.GetAsync<ObservableCollection<UserFoodDTO>>(getUserSelectedFoodDataUrl);
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
                    MainFoodImage.Source = new BitmapImage(new Uri(_mainFoods[mainFoodIcon.mainIcon]));
                    if (mainFoodIcon.secondaryIcon != 11) SecondaryFoodImage.Source = new BitmapImage(new Uri(_secondaryFoods[mainFoodIcon.secondaryIcon]));
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

        private void NotifyTeam_Click(object sender, RoutedEventArgs e)
        {
            // Generate the toast notification content and pop the toast
            new ToastContentBuilder()
                .SetToastScenario(ToastScenario.Reminder)
                .AddArgument("action", "viewFoodPage")
                .AddText("🍱 Lunch food is now ready !!!!")
                .AddText($"There are {vm.Foods.Count} disks 🍽 this week")
                .AddText("Deadline: 12:00PM Thursday noon ⏰")
                .AddHeroImage(new Uri("ms-appx:///Assets/FoodAssets/FoodToast.png"))
                .AddComboBox("foodList", "Top 5 food", vm.Foods.OrderByDescending(f => f.Percentage).FirstOrDefault().id.ToString(), 
                                                       vm.Foods.OrderByDescending(f => f.Percentage).Select(f => (f.id.ToString(), f.foodEnglishName)).Take(5).ToArray())
                .AddButton(new ToastButton().SetContent("Order this food").AddArgument("chosenFood", "foodList"))
                .AddAudio(new ToastAudio() 
                    { 
                       Src = new Uri("ms-appx:///Assets/AppAudio/clearly-602.mp3") 
                    })
                .Show();
        }
    }
}
