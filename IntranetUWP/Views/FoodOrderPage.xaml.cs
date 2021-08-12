using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.UserControls;
using IntranetUWP.ViewModels.Commands;
using IntranetUWP.ViewModels.PagesViewModel;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace IntranetUWP.Views
{
    public sealed partial class FoodOrderPage : Page
    {
        public FoodOrderPageViewModel vm = new FoodOrderPageViewModel();
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        private UserDTO personalData = new UserDTO();
        private int usersFoodData;
        private int FirstClick = 0;

        public FoodOrderPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var userFoodData = await httpHelper.GetAsync<ObservableCollection<UserFoodDTO>>(vm.getUserSelectedFoodDataUrl);
            var foodData = await httpHelper.GetAsync<ObservableCollection<FoodDTO>>(vm.getFoodsDataUrl);
            var usersData = await httpHelper.GetAsync<List<UserDTO>>(vm.getUsersDataUrl);
            usersFoodData = userFoodData == null ? 0 : userFoodData.Count;
            if (App.localSettings.Values["UserId"] != null)
            {
                personalData = usersData.Where(u => u.id == (int)App
                                        .localSettings.Values["UserId"]).FirstOrDefault();
            }
            //Init user local values
            if (App.localSettings.Values["FirstName"] != null)
            {
                FirstWelcomeMessage.Text = "Welcome " + App.localSettings.Values["FirstName"] + "!";
            }
            else FirstWelcomeMessage.Text = "Welcome to Intranet ordering system";
            if (App.localSettings.Values["FoodId"] != null && (int)App.localSettings.Values["FoodId"] != 0)
            {
                var mainFood = foodData.Where(f => f.id == (int)App.localSettings.Values["FoodId"]).FirstOrDefault();
                if (mainFood != null)
                {
                    PickedFoodText.Text = "You pick :";
                    FoodIndexText.Text = (foodData.IndexOf(mainFood) + 1).ToString();
                    FoodNameText.Text = mainFood.foodEnglishName != null ? mainFood.foodEnglishName : "";
                    MainFoodImage.Source = new BitmapImage(new Uri(vm._mainFoods[mainFood.mainIcon]));
                    if (mainFood.secondaryIcon != 11 && mainFood.secondaryIcon != null)
                        SecondaryFoodImage.Source = new BitmapImage(new Uri(vm._secondaryFoods[mainFood.secondaryIcon]));
                    else SecondaryFoodImage.Source = null;
                }
            }
            else
            {
                PickedFoodText.Text = "Pick these below dishes";
                MainFoodImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/LunchFood.png"));
            }
            FadeIconIn.Begin();
        }
        private void FoodCard_ToggleClick(int foodId, bool isToggled)
        {
            WorkingBar.Visibility = Visibility.Visible;
            if (App.localSettings.Values["FoodId"] != null && (int)App.localSettings.Values["FoodId"] != 0)
            {
                //Case: Deselect current toggled food
                if ((int)App.localSettings.Values["FoodId"] == foodId)
                {
                    usersFoodData--;

                    var deselectedFood = vm.Foods.Where(f => f.id == (int)App.localSettings.Values["FoodId"]).FirstOrDefault();
                    deselectedFood.IsSelected = false;

                    deselectedFood.NumberOfSelectedUser--;
                    if (deselectedFood.NumberOfSelectedUser == -1)
                        deselectedFood.Percentage = 0;
                    else deselectedFood.Percentage = (deselectedFood.NumberOfSelectedUser / usersFoodData) * 100;

                    foreach (FoodDTO f in vm.Foods)
                        if (f.id != deselectedFood.id)
                        {
                            if (f.NumberOfSelectedUser != 0)
                            {
                                f.Percentage = (f.NumberOfSelectedUser / (usersFoodData)) * 100;
                            }
                        }

                    App.localSettings.Values["FoodId"] = 0;
                    vm.NumberOfFood--;
                    //Add to non selected users
                    vm.NonSelectedUser.Add(personalData);

                    vm.UserFoods
                        .Remove(vm.UserFoods
                        .Where(uf => uf.user.id == personalData.id)
                        .FirstOrDefault());

                    //Add user food to iDealogic List
                    if ((bool)App.localSettings.Values["Company"] == true)
                    {
                        vm.iDealogicUsersFood
                            .Remove(vm.iDealogicUsersFood
                            .Where(uf => uf.user.id == personalData.id)
                            .FirstOrDefault());
                    }
                    //Add user food to Devinition List
                    else
                    {
                        vm.DevinitionUsersFood
                            .Remove(vm.DevinitionUsersFood
                            .Where(uf => uf.user.id == personalData.id)
                            .FirstOrDefault());
                    }

                    PickedFoodText.Text = "Please select below dishes";
                    FoodIndexText.Text = "0";
                    FoodNameText.Text = "";
                    MainFoodImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/FoodAssets/LunchFood.png"));
                    SecondaryFoodImage.Source = null;
                    FadeIconIn.Begin();
                }
                //Case: From one toggled food to different food
                else
                {
                    if (usersFoodData != 0)
                    {
                        //Deselect current food
                        vm.UserFoods
                            .Remove(vm.UserFoods
                            .Where(uf => uf.user.id == personalData.id)
                            .FirstOrDefault());

                        //If user is iDealogic then remove the UserFood of iDealogic
                        if ((bool)App.localSettings.Values["Company"] == true)
                        {
                            vm.iDealogicUsersFood
                                .Remove(vm.iDealogicUsersFood
                                .Where(uf => uf.user.id == personalData.id)
                                .FirstOrDefault());
                        }
                        //If user is Devinition then remove the UserFood of Devinition
                        else
                        {
                            vm.DevinitionUsersFood
                                .Remove(vm.DevinitionUsersFood
                                .Where(uf => uf.user.id == personalData.id)
                                .FirstOrDefault());
                        }
                        //Get the new selected food
                        var newSelectedFood = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
                        //Get the previous selected food
                        var previousSelectedFood = vm.Foods.Where(f => f.id == (int)App.localSettings.Values["FoodId"]).FirstOrDefault();
                        //Register the new selected food
                        newSelectedFood.IsSelected = true;
                        newSelectedFood.NumberOfSelectedUser++;
                        newSelectedFood.Percentage = (newSelectedFood.NumberOfSelectedUser / usersFoodData) * 100;
                        //Write down to local storage of the app
                        App.localSettings.Values["FoodId"] = foodId;
                        //Unregister the previous food
                        previousSelectedFood.IsSelected = false;
                        previousSelectedFood.NumberOfSelectedUser--;
                        previousSelectedFood.Percentage = (previousSelectedFood.NumberOfSelectedUser / usersFoodData) * 100;

                        //Render the UI
                        PickedFoodText.Text = "You pick :";
                        FoodIndexText.Text = newSelectedFood.itemNo.ToString();
                        FoodNameText.Text = newSelectedFood.foodEnglishName;
                        MainFoodImage.Source = new BitmapImage(new Uri(vm._mainFoods[newSelectedFood.mainIcon]));
                        if (newSelectedFood.secondaryIcon != 11 && newSelectedFood.secondaryIcon != null) 
                            SecondaryFoodImage.Source = new BitmapImage(new Uri(vm._secondaryFoods[newSelectedFood.secondaryIcon]));
                        else SecondaryFoodImage.Source = null;
                        FadeIconIn.Begin();

                        //Add to UserFood list
                        vm.UserFoods.Insert(0, new UserFoodDTO()
                        {
                            user = personalData,
                            food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault(),
                            foodList = vm.Foods
                        });

                        //Add user food to iDealogic List
                        if ((bool)App.localSettings.Values["Company"] == true)
                        {
                            vm.iDealogicUsersFood.Insert(0, new UserFoodDTO()
                            {
                                user = personalData,
                                food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault(),
                                foodList = vm.Foods
                            });
                        }
                        //Add user food to Devinition List
                        else
                        {
                            vm.DevinitionUsersFood.Insert(0, new UserFoodDTO()
                            {
                                user = personalData,
                                food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault(),
                                foodList = vm.Foods
                            });
                        }
                    }
                }
            }
            //No local food have choosen
            else
            {
                if (isToggled == true)
                {
                    usersFoodData++;
                    FoodImageContainer.Opacity = 0;
                    FoodGridView.SelectedItem = null;
                    vm.UserFoods.Insert(0, new UserFoodDTO()
                    {
                        user = personalData,
                        food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault(),
                        foodList = vm.Foods
                    });
                    //Add user food to iDealogic List
                    if ((bool)App.localSettings.Values["Company"] == true)
                    {
                        vm.iDealogicUsersFood.Insert(0, new UserFoodDTO()
                        {
                            user = personalData,
                            food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault(),
                            foodList = vm.Foods
                        });
                    }
                    //Add user food to Devinition List
                    else
                    {
                        vm.DevinitionUsersFood.Insert(0, new UserFoodDTO()
                        {
                            user = personalData,
                            food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault(),
                            foodList = vm.Foods
                        });
                    }
                    foreach (FoodDTO dto in vm.Foods) if (dto.id != foodId)
                            dto.Percentage = (dto.NumberOfSelectedUser / (usersFoodData)) * 100;
                        else
                        {
                            dto.IsSelected = true;

                            if (dto.NumberOfSelectedUser == -1)
                                dto.NumberOfSelectedUser++;

                            dto.NumberOfSelectedUser = dto.NumberOfSelectedUser + 1;
                            if (usersFoodData != 0)
                            {
                                dto.Percentage = (dto.NumberOfSelectedUser / (usersFoodData)) * 100;
                            }
                            else dto.Percentage = 0;
                           App.localSettings.Values["FoodId"] = foodId;
                            vm.NumberOfFood++;
                            vm.NonSelectedUser.Remove(
                                vm.NonSelectedUser.Where(u => u.id == personalData.id).FirstOrDefault());
                        }

                    var mainFood = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
                    PickedFoodText.Text = "You pick :";
                    FoodIndexText.Text = mainFood.itemNo.ToString();
                    FoodNameText.Text = mainFood.foodEnglishName;
                    MainFoodImage.Source = new BitmapImage(new Uri(vm._mainFoods[mainFood.mainIcon]));
                    if (mainFood.secondaryIcon != 11 && mainFood.secondaryIcon != null) 
                        SecondaryFoodImage.Source = new BitmapImage(new Uri(vm._secondaryFoods[mainFood.secondaryIcon]));
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
        private void FoodGridView_LostFocus(object sender, RoutedEventArgs e) 
        { 
            FoodGridView.SelectedItem = null; 
            EditFood.IsEnabled = false; 
            DeleteFood.IsEnabled = false; 
        }
        private async void FoodCard_DeleteSwipe(int foodId)
        {
            var food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
            vm.SelectedFood = food;
            vm.askBeforeDeleteFoodCommand.Execute(null);
        }
        private async void FoodCard_EditSwipe(int foodId)
        {
            var food = vm.Foods.Where(f => f.id == foodId).FirstOrDefault();
            CreateFood editFoodDialog = new CreateFood();
            editFoodDialog.Food = food;
            editFoodDialog.PrimaryButtonClick += vm.EditFoodDialog_PrimaryButtonClick;
            await editFoodDialog.ShowAsync();
        }
        private void FoodGridView_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            if (FoodGridView.SelectedItem == null)
            {
                MenuFlyout myFlyout = new MenuFlyout();
                MenuFlyoutItem copyFromClipBoard = new MenuFlyoutItem
                {
                    Text = "Paste from clipboard",
                    Command = vm.getFoodFromClipboard,
                    Icon = new FontIcon()
                    {
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        Glyph = "\xF0E3"
                    }
                };

                MenuFlyoutItem deleteAllFoodList = new MenuFlyoutItem
                {
                    Text = "Delete all food",
                    Command = vm.deleteAllFoodCommand,
                    Icon = new SymbolIcon(Symbol.Delete),
                    Foreground = new SolidColorBrush(Colors.Red)
                };

                MenuFlyoutItem printUserFoodSheet = new MenuFlyoutItem
                {
                    Text = "Print Food Sheet",
                    Command = new RelayCommand(async () => await PrintSheetOperation()),
                    Icon = new SymbolIcon(Symbol.Print)
                };

                MenuFlyoutItem exportToWordFormat = new MenuFlyoutItem
                {
                    Text = "Export food ordering sheet",
                    Icon = new ImageIcon() { 
                        Source = new BitmapImage(
                                 new Uri("ms-appx:///Assets/AppLogoAssets/WordLogo.png"))
                    }
                };

                MenuFlyoutSeparator seperator = new MenuFlyoutSeparator();

                myFlyout.Items.Add(copyFromClipBoard);
                myFlyout.Items.Add(deleteAllFoodList);
                myFlyout.Items.Add(seperator);
                myFlyout.Items.Add(printUserFoodSheet);
                myFlyout.Items.Add(exportToWordFormat);

                //the code can show the flyout in your mouse click 
                myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
            }
        }
        private void RefreshPage_Click(object sender, RoutedEventArgs e) => Frame.Navigate(this.GetType());
        private async void MemberCard_ClearSelection(int userId)
        {
            var userFood = vm.UserFoods.Where(uf => uf.user.id == userId).FirstOrDefault();
            var deleteResult = await httpHelper.RemoveAsync(vm.deleteUserFoodDataUrl, userFood.id);
            if (deleteResult == true)
            {
                vm.UserFoods.Remove(userFood);
                usersFoodData--;
                foreach (FoodDTO dto in vm.Foods)
                {
                    if (dto.id == userFood.food.id)
                    {
                        if (dto.NumberOfSelectedUser != 0)
                        {
                            dto.NumberOfSelectedUser--;
                            if (vm.UserFoods.Count != 0)
                            {
                                dto.Percentage = ((dto.NumberOfSelectedUser) / vm.UserFoods.Count) * 100;
                            } else dto.Percentage = 0;
                        }
                        else dto.Percentage = 0;
                    }
                    else
                    {
                        if (dto.NumberOfSelectedUser != 0)
                            dto.Percentage = (dto.NumberOfSelectedUser / vm.UserFoods.Count) * 100;
                        else dto.Percentage = 0;
                    }
                    //Refactor this shit out
                    if ((int)App.localSettings.Values["UserId"] == userId)
                    {
                        dto.IsSelected = false;
                        App.localSettings.Values["FoodId"] = 0;
                    }
                }
            }
            else Debug.Write("Delete operation error");
        }
        private void MemberCard_DisableUser(int userId)
        {

        }

        private async void PrintSheet_Click(object sender, RoutedEventArgs e)
        {
            await PrintSheetOperation();
        }
        private async Task PrintSheetOperation()
        {
            var printFoodList = new ObservableCollection<ExtendedFoodDTO>();
            if (FirstClick == 0)
            {
                foreach (KeyValuePair<int, int> entry in vm._foodCount.OrderBy(x => x.Key))
                {
                    var foodInDic = vm.Foods.Where(f => f.id == entry.Key).FirstOrDefault();
                    printFoodList.Add(new ExtendedFoodDTO()
                    {
                        foodName = foodInDic.foodName,
                        foodEnglishName = foodInDic.foodEnglishName,
                        MainFoodUrl = vm._mainFoods[foodInDic.mainIcon],
                        SecondaryFoodUrl = foodInDic.secondaryIcon != null
                                           ? vm._secondaryFoods[foodInDic.secondaryIcon]
                                           : null,
                        NumberOfSelectedUser = foodInDic.NumberOfSelectedUser,
                        itemNo = foodInDic.itemNo
                    });
                }
            }
            var printHelper = new PrintHelper(container);

            var stackPanel = new StackPanel();
            stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

            var TableListView = new ListView();
            TableListView.ItemTemplate = (DataTemplate)Resources["FoodList"];
            TableListView.ItemsSource = printFoodList;
            TableListView.HorizontalAlignment = HorizontalAlignment.Center;

            var iDealogicImage = new Image();
            iDealogicImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/iDealogic.png"));
            iDealogicImage.HorizontalAlignment = HorizontalAlignment.Center;
            iDealogicImage.Margin = new Thickness() { Top = 5, Bottom = 5 };
            iDealogicImage.Width = 100;

            var iDealogicListView = new ListView();
            iDealogicListView.ItemTemplate = (DataTemplate)Resources["EmployeeList"];
            iDealogicListView.ItemsSource = vm.iDealogicUsersFood;

            var sumOfiDealogicOrder = new TextBlock()
            {
                Text = "TÔNG IDEALOGIC: " + vm.iDealogicUsersFood.Count.ToString(),
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 216, 56, 62)),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var DevinitionImage = new Image();
            DevinitionImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Devinition.png"));
            DevinitionImage.HorizontalAlignment = HorizontalAlignment.Center;
            DevinitionImage.Margin = new Thickness() { Top = 5, Bottom = 5 };
            DevinitionImage.Width = 100;

            var DevinitionListView = new ListView();
            DevinitionListView.ItemTemplate = (DataTemplate)Resources["EmployeeList"];
            DevinitionListView.ItemsSource = vm.DevinitionUsersFood;

            var sumOfDevinitionOrder = new TextBlock()
            {
                Text = "TÔNG DEVINITION: " + vm.DevinitionUsersFood.Count.ToString(),
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 21, 81, 113)),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            stackPanel.Children.Add(TableListView);

            stackPanel.Children.Add(new AppBarSeparator());

            stackPanel.Children.Add(iDealogicImage);
            stackPanel.Children.Add(iDealogicListView);
            stackPanel.Children.Add(sumOfiDealogicOrder);

            stackPanel.Children.Add(new AppBarSeparator());

            stackPanel.Children.Add(DevinitionImage);
            stackPanel.Children.Add(DevinitionListView);
            stackPanel.Children.Add(sumOfDevinitionOrder);

            printHelper.AddFrameworkElementToPrint(stackPanel);

                await printHelper.ShowPrintUIAsync("Your physical paper");
        }

    }
    public class ExtendedFoodDTO : FoodDTO
    {
        public string MainFoodUrl { get; set; }
        public string SecondaryFoodUrl { get; set; } = String.Empty;
    }
}
