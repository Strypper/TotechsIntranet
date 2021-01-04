using IntranetUWP.Models;
using IntranetUWP.UserControls;
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
        public ObservableCollection<FoodDTO> Foods { get; set; } = new ObservableCollection<FoodDTO>();
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
        public FoodDTO Food { get; set; } = new FoodDTO();
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
            Foods = DemoFoodData.getData();
            Users = DemoUserData.getData();
        }

        private async void AddFood_Click(object sender, RoutedEventArgs e)
        {
            CreateFood dialog = new CreateFood();
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
            await dialog.ShowAsync();
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var dialog = sender as CreateFood;
            Food = dialog.Food;
            Foods.Add(Food);
        }

        private void FoodCard_ToggleClick(int foodId)
        {
            FoodImageContainer.Opacity = 0;
            FoodGridView.SelectedItem = null;
            foreach (FoodDTO dto in Foods) if (dto.FoodId != foodId) dto.IsSelected = false;
            var mainFoodIcon = Foods.Where(f => f.FoodId == foodId).FirstOrDefault();
            MainFoodImage.Source = new BitmapImage(new Uri(_mainFoods[mainFoodIcon.MainIcon]));
            if (mainFoodIcon.SecondaryIcon != 11) SecondaryFoodImage.Source = new BitmapImage(new Uri(_secondaryFoods[mainFoodIcon.SecondaryIcon])); else SecondaryFoodImage.Source = null;
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

        private void DeleteFood_Click(object sender, RoutedEventArgs e)
        {
            Foods.Remove((FoodDTO)FoodGridView.SelectedItem);
            EditFood.IsEnabled = false;
            DeleteFood.IsEnabled = false;
        }

        private void DeleteAllFood_Click(object sender, RoutedEventArgs e) => Foods.ToList().All(i => Foods.Remove(i));

        private void FoodGridView_LostFocus(object sender, RoutedEventArgs e) { FoodGridView.SelectedItem = null; EditFood.IsEnabled = false; DeleteFood.IsEnabled = false; }
    }
}
