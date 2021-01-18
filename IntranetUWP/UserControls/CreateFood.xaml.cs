using IntranetUWP.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.UserControls
{
    public sealed partial class CreateFood : ContentDialog, INotifyPropertyChanged
    {

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

        public FoodDTO Food { get; set; } = new FoodDTO();

        public CreateFood()
        {
            this.InitializeComponent();
            DefaultOption.IsChecked = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Food.foodName = VietnameseFoodName.Text;
            Food.foodEnglishName = EnglishFoodName.Text;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
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
                    VietnameseFoodName.Text = "Cơm";
                    Food.mainIcon = 1;
                    break;
                case "BreadOption":
                    PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Bread.png"));
                    SecondaryFood.Source = null;
                    _isSelectable = false;
                    _isChecked = false;
                    OnPropertyChanged("_isSelectable");
                    OnPropertyChanged("_isChecked");
                    EnglishFoodName.Text = "Bread";
                    VietnameseFoodName.Text = "Bánh mỳ";
                    Food.mainIcon = 2;
                    break;
                case "SpaghetiOption":
                    PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Spagheti.png"));
                    SecondaryFood.Source = null;
                    _isSelectable = false;
                    _isChecked = false;
                    OnPropertyChanged("_isSelectable");
                    OnPropertyChanged("_isChecked");
                    EnglishFoodName.Text = "Spagheti";
                    VietnameseFoodName.Text = "Mỳ ý";
                    Food.mainIcon = 3;
                    break;
                case "NoodleOption":
                    PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Noodle.png"));
                    SecondaryFood.Source = null;
                    _isSelectable = false;
                    _isChecked = false;
                    OnPropertyChanged("_isSelectable");
                    OnPropertyChanged("_isChecked");
                    EnglishFoodName.Text = "Noodle";
                    VietnameseFoodName.Text = "";
                    Food.mainIcon = 4;
                    break;
                case "DefaultOption":
                    PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/LunchFood.png"));
                    SecondaryFood.Source = null;
                    _isSelectable = false;
                    _isChecked = false;
                    OnPropertyChanged("_isSelectable");
                    OnPropertyChanged("_isChecked");
                    EnglishFoodName.Text = "";
                    VietnameseFoodName.Text = "";
                    Food.mainIcon = 5;
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
                    VietnameseFoodName.Text = "Cơm thịt";
                    break;
                case "Chicken":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Chicken.png"));
                    Food.secondaryIcon = 7;
                    VietnameseFoodName.Text = "Cơm gà";
                    break;
                case "Egg":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Egg.png"));
                    Food.secondaryIcon = 8;
                    VietnameseFoodName.Text = "Cơm trứng";
                    break;
                case "Shrimp":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Shrimp.png"));
                    Food.secondaryIcon = 9;
                    VietnameseFoodName.Text = "Cơm tôm";
                    break;
                case "Falafel":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Falafel.png"));
                    Food.secondaryIcon = 10;
                    VietnameseFoodName.Text = "Cơm xíu mại";
                    break;
            }
        }
    }
}
