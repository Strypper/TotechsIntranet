using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.UserControls;
using IntranetUWP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace IntranetUWP.ViewModels.PagesViewModel
{
    public class FoodOrderPageViewModel : ViewModelBase
    {
        string getFoodsDataUrl = "Food/GetAll";
        string createFoodDataUrl = "Food/Create";
        string deleteFoodDataUrl = "Food/Delete";
        string deleteAllFoodDataUrl = "Food/DeleteAll";
        string getUsersDataUrl = "User/GetAll";
        string getUserSelectedFoodDataUrl = "UserFood/GetAll";
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public ICommand getAllFoodCommand { get; set; }
        public ICommand createFoodCommand { get; set; }
        public ICommand deleteFoodCommand { get; set; }
        public ICommand deleteAllFoodCommand { get; set; }
        public ICommand getUserCommand { get; set; }
        public ObservableCollection<UserDTO> Users { get; set; }
        public ObservableCollection<FoodDTO> Foods { get; set; }
        public ObservableCollection<UserFoodDTO> UserFoods { get; set; }
        public FoodDTO SelectedFood { get; set; }

        private bool deleteAllFoodButtonState = false;

        public bool DeleteAllFoodButtonState
        {
            get { return deleteAllFoodButtonState; }
            set { deleteAllFoodButtonState = value; OnPropertyChanged("DeleteAllFoodButtonState"); }
        }


        public FoodOrderPageViewModel()
        {
            Users = new ObservableCollection<UserDTO>();
            Foods = new ObservableCollection<FoodDTO>();
            Foods.CollectionChanged += Foods_CollectionChanged;
            UserFoods = new ObservableCollection<UserFoodDTO>();
            GetUserFoodsData();
            getAllFoodCommand = new RelayCommand(async() => await GetAllFood());
            createFoodCommand = new RelayCommand(async() => await CreateFood());
            deleteFoodCommand = new RelayCommand(async () => await RemoveFood());
            deleteAllFoodCommand = new RelayCommand(async () => await RemoveAllFood());
            getUserCommand = new RelayCommand(async () => await GetUserFoodsData());
        }

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


        private async Task GetAllFood()
        {
            var foodList = await httpHelper.GetAsync<ObservableCollection<FoodDTO>>(getFoodsDataUrl);
            foreach ( var food in foodList) { Foods.Add(food); }
        }

        private async Task CreateFood()
        {
            CreateFood createFoodDialog = new CreateFood();
            createFoodDialog.PrimaryButtonClick += CreateFoodDialog_PrimaryButtonClick;
            await createFoodDialog.ShowAsync();
        }

        private async void CreateFoodDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var food = (sender as CreateFood).Food;
            var createResult = await httpHelper.CreateAsync<FoodDTO>(createFoodDataUrl , food);
            if (createResult != null) Foods.Add(createResult); else Debug.WriteLine("Create operation error");
        }

        private async Task RemoveFood()
        {
            var food = Foods.Where(f => f == SelectedFood).FirstOrDefault();
            var deleteResult = await httpHelper.RemoveAsync(deleteFoodDataUrl, food.id);
            if (deleteResult == true) Foods.Remove(food); else Debug.Write("Delete operation error");
        }

        private async Task RemoveAllFood()
        {
            var response = await httpHelper.DeleteAsync(deleteAllFoodDataUrl);
            if (response.StatusCode.ToString() == "NoContent") Foods.ToList().All(i => Foods.Remove(i));
            else Debug.WriteLine("Delete all operation error");
        }

        private async Task GetAllUsers()
        {
            var usersList = await httpHelper.GetAsync<ObservableCollection<UserDTO>>(getUsersDataUrl);
            foreach( var user in usersList ) { Users.Add(user); } 
        }

        private async Task GetUserFoodsData()
        {
            if (UserFoods.Count > 0) { Users.Clear(); UserFoods.Clear(); Foods.Clear(); await Task.Delay(1000); }
            var userSelectedFoods = await httpHelper.GetAsync<ObservableCollection<UserFoodDTO>>(getUserSelectedFoodDataUrl);

            await GetAllUsers();
            await GetAllFood();

            foreach ( var user in Users)
            {
                if(userSelectedFoods.Any(u => u.user.id == user.id) == false)
                {
                    FoodDTO food = new FoodDTO() { id = -1 };
                    userSelectedFoods.Add(new UserFoodDTO() { user = user, food = food, foodList = Foods });
                }
            };

            foreach (var userSelectedFood in userSelectedFoods)
            {
                userSelectedFood.foodList = Foods;
                UserFoods.Add(userSelectedFood);
            };
        }

        private void Foods_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => DeleteAllFoodButtonState = Foods.Count > 0 ? true : false;
    }
}
