﻿using IntranetUWP.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
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
            set => OnPropertyChanged();
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => OnPropertyChanged();
        }

        private int MainIcon;
        private int? SecondaryIcon;

        public FoodDTO Food { get; set; } = new FoodDTO();

        public CreateFood()
        {
            this.InitializeComponent();
            DefaultOption.IsChecked = true;
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            if(Food != null && Food.itemNo != 0)
            {
                PrimaryButtonText = "✏️ Edit this food";
                switch (Food.mainIcon)
                {
                    case 1:
                        RiceOption.IsChecked = true;
                        PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Rice.png"));
                        _isSelectable = true;
                        _isChecked = false;
                        OnPropertyChanged("_isSelectable");
                        OnPropertyChanged("_isChecked");
                        break;
                    case 2:
                        BreadOption.IsChecked = true;
                        PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Bread.png"));
                        SecondaryFood.Source = null;
                        _isSelectable = false;
                        _isChecked = false;
                        OnPropertyChanged("_isSelectable");
                        OnPropertyChanged("_isChecked");
                        break;
                    case 3:
                        SpaghetiOption.IsChecked = true;
                        PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Spagheti.png"));
                        SecondaryFood.Source = null;
                        _isSelectable = false;
                        _isChecked = false;
                        OnPropertyChanged("_isSelectable");
                        OnPropertyChanged("_isChecked");
                        break;
                    case 4:
                        NoodleOption.IsChecked = true;
                        PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Noodle.png"));
                        SecondaryFood.Source = null;
                        _isSelectable = false;
                        _isChecked = false;
                        OnPropertyChanged("_isSelectable");
                        OnPropertyChanged("_isChecked");
                        break;
                    case 5:
                        DefaultOption.IsChecked = true;
                        PrimaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/LunchFood.png"));
                        SecondaryFood.Source = null;
                        _isSelectable = false;
                        _isChecked = false;
                        OnPropertyChanged("_isSelectable");
                        OnPropertyChanged("_isChecked");
                        break;
                }
                switch (Food.secondaryIcon)
                {
                    case 6:
                        Meat.IsChecked = true;
                        SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Meat.png"));
                        break;
                    case 7:
                        Chicken.IsChecked = true;
                        SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Chicken.png"));
                        break;
                    case 8:
                        Egg.IsChecked = true;
                        SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Egg.png"));
                        break;
                    case 9:
                        Shrimp.IsChecked = true;
                        SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Shrimp.png"));
                        break;
                    case 10:
                        Shrimp.IsChecked = true;
                        SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Falafel.png"));
                        break;
                }
                VietnameseFoodName.Text = Food.foodName;
                EnglishFoodName.Text = Food.foodEnglishName;
                UnavaibleButton.IsChecked = Food.IsUnavailable;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Food.foodName = VietnameseFoodName.Text;
            Food.foodEnglishName = EnglishFoodName.Text;
            Food.mainIcon = MainIcon;
            Food.secondaryIcon = SecondaryIcon;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {}

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
                    MainIcon = 1;
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
                    MainIcon = 2;
                    SecondaryIcon = null;
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
                    MainIcon = 3;
                    SecondaryIcon = null;
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
                    MainIcon = 4;
                    SecondaryIcon = null;
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
                    MainIcon = 5;
                    SecondaryIcon = null;
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
                    SecondaryIcon = 6;
                    VietnameseFoodName.Text = "Cơm thịt";
                    break;
                case "Chicken":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Chicken.png"));
                    SecondaryIcon = 7;
                    VietnameseFoodName.Text = "Cơm gà";
                    break;
                case "Egg":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Egg.png"));
                    SecondaryIcon = 8;
                    VietnameseFoodName.Text = "Cơm trứng";
                    break;
                case "Shrimp":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Shrimp.png"));
                    SecondaryIcon = 9;
                    VietnameseFoodName.Text = "Cơm tôm";
                    break;
                case "Falafel":
                    SecondaryFood.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/Falafel.png"));
                    SecondaryIcon = 10;
                    VietnameseFoodName.Text = "Cơm xíu mại";
                    break;
            }
        }

        private void UnavaibleButton_Click(object sender, RoutedEventArgs e)
        {
            var tb = sender as ToggleButton;
            if (tb != null)
            {
                if(tb.IsChecked == false)
                {
                    tb.IsChecked = false;
                    Food.IsUnavailable = false;
                }
                else
                {
                    tb.IsChecked = true;
                    Food.IsUnavailable = true;
                }
            }
        }
    }
}
