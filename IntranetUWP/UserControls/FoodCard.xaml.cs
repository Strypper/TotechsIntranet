using IntranetUWP.Helpers;
using IntranetUWP.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public delegate void FoodCardEventHandler(int foodId, bool isToggled);
    public delegate void DeleteFoodCardEventHandler(int foodId);
    public delegate void EditFoodCardEventHandler(int foodId);

    public sealed partial class FoodCard : UserControl
    {
        public int FoodId
        {
            get { return (int)GetValue(FoodIdProperty); }
            set { SetValue(FoodIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodIdProperty =
            DependencyProperty.Register("FoodId", typeof(int), typeof(FoodCard), null);



        public string FoodName
        {
            get { return (string)GetValue(FoodNameProperty); }
            set { SetValue(FoodNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodNameProperty =
            DependencyProperty.Register("FoodName", typeof(string), typeof(FoodCard), null);



        public string FoodEnglishName
        {
            get { return (string)GetValue(FoodEnglishNameProperty); }
            set { SetValue(FoodEnglishNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodEnglishName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodEnglishNameProperty =
            DependencyProperty.Register("FoodEnglishName", typeof(string), typeof(FoodCard), null);

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value);}
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(FoodCard), new PropertyMetadata(null));

        public bool IsUnavailable
        {
            get { return (bool)GetValue(IsUnavailableProperty); }
            set { SetValue(IsUnavailableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUnavailable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUnavailableProperty =
            DependencyProperty.Register("IsUnavailable", typeof(bool), typeof(FoodCard), new PropertyMetadata(null));


        public int MainFoodIcon
        {
            get { return (int)GetValue(MainFoodIconProperty); }
            set
            {
                  SetValue(MainFoodIconProperty, value);
                  MainFoodImage.Source = new BitmapImage(new Uri(_mainFoods[value]));
            }
        }

        // Using a DependencyProperty as the backing store for MainFoodIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainFoodIconProperty =
            DependencyProperty.Register("MainFoodIcon", typeof(int), typeof(FoodCard), null);

        public int? SecondaryFoodIcon
        {
            get { return (int?)GetValue(SecondaryFoodIconProperty); }
            set
            {
                if(value != null)
                {
                    if(_secondaryFoods[value] != null)
                    {
                        SetValue(SecondaryFoodIconProperty, value);
                        SecondaryFoodImage.Source = new BitmapImage(new Uri(_secondaryFoods[value]));
                    }
                }
                else SecondaryFoodImage.Source = null;
            }
        }

        // Using a DependencyProperty as the backing store for SecondaryFoodIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondaryFoodIconProperty =
            DependencyProperty.Register("SecondaryFoodIcon", typeof(int?), typeof(FoodCard), null);

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



        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, Double.IsNaN(value) ? 0.0 : value); }
        }
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(double), new PropertyMetadata(0.0));

        public double NumberOfSelectedUser
        {
            get { return (double)GetValue(NumberOfSelectedUserProperty); }
            set { SetValue(NumberOfSelectedUserProperty, value); }
        }
        public static readonly DependencyProperty NumberOfSelectedUserProperty =
            DependencyProperty.Register("NumberOfSelectedUser", typeof(double), typeof(double), new PropertyMetadata(0d));

        public int ItemNo
        {
            get { return (int)GetValue(ItemNoProperty); }
            set { SetValue(ItemNoProperty, value); }
        }
        public static readonly DependencyProperty ItemNoProperty =
            DependencyProperty.Register("ItemNo", typeof(int), typeof(int), new PropertyMetadata(0));

        public List<string> usersAvatar
        {
            get { return (List<string>)GetValue(usersAvatarProperty); }
            set { SetValue(usersAvatarProperty, value); }
        }
        public static readonly DependencyProperty usersAvatarProperty =
            DependencyProperty.Register("usersAvatar", typeof(List<string>), typeof(List<string>), new PropertyMetadata(0));

        public event FoodCardEventHandler ToggleClick;
        public event DeleteFoodCardEventHandler DeleteSwipe;
        public event EditFoodCardEventHandler EditSwipe;
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public FoodCard() 
            => this.InitializeComponent();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PickRateBar.Value = Percentage;
            UnavaibleIcon.Visibility = IsUnavailable == true ? Visibility.Visible : Visibility.Collapsed;
            MainFoodImage.Opacity = IsUnavailable == true ? 0.3f : 1.0f;
            ChooseButton.IsEnabled = IsUnavailable == true ? false : true;
            if (App.localSettings.Values["ProfilePic"] != null)
            {
                OwnerUser.ProfilePicture = new BitmapImage(new Uri(App.localSettings.Values["ProfilePic"].ToString()));
            }
            if (App.localSettings.Values["FoodId"] != null && IsUnavailable == false)
            {
                ChooseButton.IsChecked = FoodId == (int)App.localSettings.Values["FoodId"] ? true : false;
                OwnerUser.Visibility = FoodId == (int)App.localSettings.Values["FoodId"] ? Visibility.Visible : Visibility.Collapsed;
            }
            if (usersAvatar != null && IsUnavailable == false)
            {
                if (usersAvatar.FirstOrDefault() != null)
                {
                    FirstUser.ProfilePicture = new BitmapImage(new Uri(usersAvatar.FirstOrDefault()));
                    OwnerUser.Translation = new Vector3(-15, 0, 0);
                    FirstUser.Visibility = Visibility.Visible;
                }
                else FirstUser.Visibility = Visibility.Collapsed;
                if (usersAvatar.Count > 1 && IsUnavailable == false)
                {
                    if (usersAvatar.ElementAt(1) != null)
                    {
                        SecondUser.ProfilePicture = new BitmapImage(new Uri(usersAvatar.ElementAt(1)));
                    }
                    OwnerUser.Translation = new Vector3(-30, 0, 0);
                    SecondUser.Visibility = Visibility.Visible;
                }
                else SecondUser.Visibility = Visibility.Collapsed;
                if (usersAvatar.Count > 2 && IsUnavailable == false)
                {
                    if (usersAvatar.ElementAt(2) != null)
                    {
                        ThirdUser.ProfilePicture = new BitmapImage(new Uri(usersAvatar.ElementAt(2)));
                    }
                    OwnerUser.Translation = new Vector3(-30, 0, 0);
                    ThirdUser.Visibility = Visibility.Visible;
                }
                else ThirdUser.Visibility = Visibility.Collapsed;
            }
        }
        private async void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggleSwitch)
            {
                if (toggleSwitch.IsChecked == true)
                {
                    ToggleClick?.Invoke(FoodId, true);
                    var createupdateUserFoodDTO = new CreateUpdateUserFoodDTO();
                    createupdateUserFoodDTO.userId = (int)App.localSettings.Values["UserId"];
                    createupdateUserFoodDTO.foodId = FoodId;
                    await httpHelper.CreateAsync<UserFoodDTO>("UserFood/Create", createupdateUserFoodDTO);
                }
                else
                {
                    var userFood = await httpHelper.GetByIdAsync<UserFoodDTO>("UserFood/GetUserSelectedFood", (int)App.localSettings.Values["UserId"]);
                    if(userFood != null)
                    {
                        await httpHelper.RemoveAsync("UserFood/Delete", userFood.id);
                    }
                    ToggleClick?.Invoke(FoodId, false);
                }
            }
        }
        private void SwipeEditItem_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args) 
            => EditSwipe?.Invoke(FoodId);
        private void SwipeDeleteItem_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args) 
            => DeleteSwipe?.Invoke(FoodId);
        private void FoodCardRightTapped(object sender, RightTappedRoutedEventArgs e) 
            => FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        private void Edit_Click(object sender, RoutedEventArgs e) 
            => EditSwipe?.Invoke(FoodId);
        private void Unavaible_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Remove_Click(object sender, RoutedEventArgs e) 
            => DeleteSwipe?.Invoke(FoodId);
    }
}
