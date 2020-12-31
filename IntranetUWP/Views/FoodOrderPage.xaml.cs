using IntranetUWP.Models;
using IntranetUWP.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FoodOrderPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<FoodDTO> Foods { get; set; } = new ObservableCollection<FoodDTO>();
        public FoodDTO Food { get; set; } = new FoodDTO();
        public List<UserDTO> Users { get; set; }
        public FoodOrderPage()
        {
            this.InitializeComponent();
            Foods = DemoFoodData.getData();
            Users = DemoUserData.getData();
            FoodGridView.ItemsSource = Foods;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            foreach(FoodDTO dto in Foods)
            {
                if(dto.FoodId != foodId)
                {
                    dto.IsSelected = false;
                }
            }
            FoodGridView.ItemsSource = Foods;
            foreach (FoodDTO dto in Foods)
            {
                System.Diagnostics.Debug.WriteLine(dto.IsSelected);
                System.Diagnostics.Debug.WriteLine("-------------");
            }
        }
    }
}
