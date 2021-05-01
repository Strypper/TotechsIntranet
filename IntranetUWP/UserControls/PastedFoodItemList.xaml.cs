using IntranetUWP.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class PastedFoodItemList : UserControl, INotifyPropertyChanged
    {
        public delegate void FoodDelegate(FoodDTO food);
        private bool _isSelectable;
        public bool IsSelectable
        {
            get => _isSelectable;
            set
            {
                OnPropertyChanged();
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                OnPropertyChanged();
            }
        }



        public FoodDTO Food
        {
            get { return (FoodDTO)GetValue(FoodProperty); }
            set { SetValue(FoodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Food.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodProperty =
            DependencyProperty.Register("Food", typeof(FoodDTO), typeof(PastedFoodItemList), new PropertyMetadata(new FoodDTO()));


        public event FoodDelegate FoodEvent;
        public PastedFoodItemList()
        {
            this.InitializeComponent();
        }

        private void MainFood_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            switch (rb.Name)
            {
                case "RiceOption":
                    PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Rice.png"));
                    _isSelectable = true;
                    _isChecked = false;
                    OnPropertyChanged("_isSelectable");
                    OnPropertyChanged("_isChecked");
                    EnglishFoodName.Text = "Rice";
                    Food.mainIcon = 1;
                    FoodEvent.Invoke(Food);
                    break;
                case "BreadOption":
                    PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Bread.png"));
                    SecondaryFood.Source = null;
                    _isSelectable = false;
                    _isChecked = false;
                    OnPropertyChanged("_isSelectable");
                    OnPropertyChanged("_isChecked");
                    EnglishFoodName.Text = "Bread";
                    Food.mainIcon = 2;
                    FoodEvent.Invoke(Food);
                    break;
                case "SpaghetiOption":
                    PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Spagheti.png"));
                    SecondaryFood.Source = null;
                    _isSelectable = false;
                    _isChecked = false;
                    OnPropertyChanged("_isSelectable");
                    OnPropertyChanged("_isChecked");
                    EnglishFoodName.Text = "Spagheti";
                    Food.mainIcon = 3;
                    FoodEvent.Invoke(Food);
                    break;
                case "NoodleOption":
                    PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Noodle.png"));
                    SecondaryFood.Source = null;
                    _isSelectable = false;
                    _isChecked = false;
                    OnPropertyChanged("_isSelectable");
                    OnPropertyChanged("_isChecked");
                    EnglishFoodName.Text = "Noodle";
                    Food.mainIcon = 4;
                    FoodEvent.Invoke(Food);
                    break;
                case "DefaultOption":
                    PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/LunchFood.png"));
                    SecondaryFood.Source = null;
                    _isSelectable = false;
                    _isChecked = false;
                    OnPropertyChanged("_isSelectable");
                    OnPropertyChanged("_isChecked");
                    EnglishFoodName.Text = "";
                    Food.mainIcon = 5;
                    FoodEvent.Invoke(Food);
                    break;
            }
        }

        private void SecondaryFood_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton;
            switch (rb.Name)
            {
                case "Meat":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Meat.png"));
                    Food.secondaryIcon = 6;
                    FoodEvent.Invoke(Food);
                    break;
                case "Chicken":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Chicken.png"));
                    Food.secondaryIcon = 7;
                    FoodEvent.Invoke(Food);
                    break;
                case "Egg":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Egg.png"));
                    Food.secondaryIcon = 8;
                    FoodEvent.Invoke(Food);
                    break;
                case "Shrimp":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Shrimp.png"));
                    Food.secondaryIcon = 9;
                    FoodEvent.Invoke(Food);
                    break;
                case "Falafel":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Falafel.png"));
                    Food.secondaryIcon = 10;
                    FoodEvent.Invoke(Food);
                    break;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void EnglishFoodName_LostFocus(object sender, RoutedEventArgs e)
        {
            Food.foodEnglishName = EnglishFoodName.Text;
            FoodEvent.Invoke(Food);
        }

        private void VietNameseFoodName_LostFocus(object sender, RoutedEventArgs e)
        {
            Food.foodName = VietNameseFoodName.Text;
            FoodEvent.Invoke(Food);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultOption.IsChecked = true;
        }
    }
}
