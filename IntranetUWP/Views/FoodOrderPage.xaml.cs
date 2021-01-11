using IntranetUWP.Models;
using IntranetUWP.UserControls;
using IntranetUWP.ViewModels.PagesViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        private readonly HttpClient httpClient = new HttpClient();
        public string getFoodDataUrl = "https://intranetcloudapi.azurewebsites.net/api/Food/GetAll";
        public string createFoodDataUrl = "https://intranetcloudapi.azurewebsites.net/api/Food/Create";
        public string deleteFoodDataUrl(int id) => $"https://intranetcloudapi.azurewebsites.net/api/Food/Delete/{Food.id}";
        public FoodOrderPageViewModel vm = new FoodOrderPageViewModel();
        public List<UserDTO> Users { get; set; } = new List<UserDTO>();
        public ObservableCollection<FoodDTO> Foods = new ObservableCollection<FoodDTO>();
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
            this.DataContext = vm;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Foods = await getFoodData();
            FoodGridView.ItemsSource = Foods;
            Foods.CollectionChanged += Foods_CollectionChanged;
        }

        private void Foods_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        { 
            if (Foods.Count > 0) DeleteAllFood.IsEnabled = true; 
            else DeleteAllFood.IsEnabled = false; 
        }

        private async Task<ObservableCollection<FoodDTO>> getFoodData()
        {
            var response = await httpClient.GetAsync(getFoodDataUrl);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<FoodDTO>>(result);
        }
        private async void AddFood_Click(object sender, RoutedEventArgs e)
        {
            CreateFood dialog = new CreateFood();
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
            await dialog.ShowAsync();
        }
        private async void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var dialog = sender as CreateFood;
            Food = dialog.Food;
            var content = new StringContent(JsonConvert.SerializeObject(Food), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(createFoodDataUrl , content);
            if (response.StatusCode.ToString() == "Created") Foods.Add(Food); else Console.WriteLine("Error");
        }
        private void FoodCard_ToggleClick(int foodId)
        {
            FoodImageContainer.Opacity = 0;
            FoodGridView.SelectedItem = null;
            foreach (FoodDTO dto in Foods) if (dto.id != foodId) dto.IsSelected = false;
            var mainFoodIcon = Foods.Where(f => f.id == foodId).FirstOrDefault();
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

        private async void DeleteFood_Click(object sender, RoutedEventArgs e)
        {
            Food = (FoodDTO)FoodGridView.SelectedItem;
            var response = await httpClient.DeleteAsync(deleteFoodDataUrl(Food.id));
            if (response.StatusCode.ToString() == "NoContent")
            {
                Foods.Remove(Food);
                EditFood.IsEnabled = false;
                DeleteFood.IsEnabled = false;
            }
            else Console.WriteLine("Error");
        }
        private void DeleteAllFood_Click(object sender, RoutedEventArgs e) => Foods.ToList().All(i => Foods.Remove(i));
        private void FoodGridView_LostFocus(object sender, RoutedEventArgs e) { FoodGridView.SelectedItem = null; EditFood.IsEnabled = false; DeleteFood.IsEnabled = false; }
    }
}
