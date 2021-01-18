using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using System;
using System.Collections.Generic;
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

        private void FoodCard_ToggleClick(int foodId)
        {
            FoodImageContainer.Opacity = 0;
            FoodGridView.SelectedItem = null;
            foreach (FoodDTO dto in vm.Foods) if (dto.id != foodId) dto.IsSelected = false;
            var mainFoodIcon = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
            MainFoodImage.Source = new BitmapImage(new Uri(_mainFoods[mainFoodIcon.mainIcon]));
            if (mainFoodIcon.secondaryIcon != 11) SecondaryFoodImage.Source = new BitmapImage(new Uri(_secondaryFoods[mainFoodIcon.secondaryIcon])); else SecondaryFoodImage.Source = null;
            FadeIconIn.Begin();
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
    }
}
