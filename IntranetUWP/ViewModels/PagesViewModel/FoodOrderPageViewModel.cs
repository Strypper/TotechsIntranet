using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.UserControls;
using IntranetUWP.ViewModels.Commands;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace IntranetUWP.ViewModels.PagesViewModel
{
    public class FoodOrderPageViewModel : ViewModelBase
    {
        public string getFoodsDataUrl = "Food/GetAll";
        public string createFoodDataUrl = "Food/Create";
        public string deleteFoodDataUrl = "Food/Delete";
        public string deleteAllFoodDataUrl = "Food/DeleteAll";
        public string getUsersDataUrl = "User/GetAll";
        public string getUserSelectedFoodDataUrl = "UserFood/GetAll";
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public ICommand getAllFoodCommand { get; set; }
        public ICommand createFoodCommand { get; set; }
        public ICommand deleteFoodCommand { get; set; }
        public ICommand deleteAllFoodCommand { get; set; }
        public ICommand getUserCommand { get; set; }
        public ICommand getFoodFromClipboard { get; set; }
        public ICommand notifyTeamCommand { get; set; }
        public ICommand generateWordDocument { get; set; }
        private ObservableCollection<UserDTO> users = new ObservableCollection<UserDTO>();
        public ObservableCollection<UserDTO> Users { get; set; }
        private ObservableCollection<FoodDTO> foods { get; set; } = new ObservableCollection<FoodDTO>();
        public ObservableCollection<FoodDTO> Foods { get; set; }
        private ObservableCollection<UserFoodDTO> userFoods { get; set; } = new ObservableCollection<UserFoodDTO>();
        public ObservableCollection<UserFoodDTO> UserFoods { get; set; }
        public FoodDTO SelectedFood { get; set; }

        private bool deleteAllFoodButtonState = false;

        public bool DeleteAllFoodButtonState
        {
            get { return deleteAllFoodButtonState; }
            set { deleteAllFoodButtonState = value; OnPropertyChanged("DeleteAllFoodButtonState"); }
        }

        private int numberOfFood;

        public int NumberOfFood
        {
            get { return numberOfFood; }
            set { numberOfFood = value; OnPropertyChanged("NumberOfFood"); }
        }
        

        public readonly IDictionary<int, string> _mainFoods = new Dictionary<int, string>
        {
            { 1, "ms-appx:///Assets/FoodAssets/Rice.png"},
            { 2, "ms-appx:///Assets/FoodAssets/Bread.png"},
            { 3, "ms-appx:///Assets/FoodAssets/Spagheti.png"},
            { 4, "ms-appx:///Assets/FoodAssets/Noodle.png"},
            { 5, "ms-appx:///Assets/FoodAssets/LunchFood.png"}
        };
        public readonly IDictionary<int?, string> _secondaryFoods = new Dictionary<int?, string>
        {
            { 6, "ms-appx:///Assets/FoodAssets/Meat.png"},
            { 7, "ms-appx:///Assets/FoodAssets/Chicken.png"},
            { 8, "ms-appx:///Assets/FoodAssets/Egg.png"},
            { 9, "ms-appx:///Assets/FoodAssets/Shrimp.png"},
            { 10, "ms-appx:///Assets/FoodAssets/Falafel.png"},
            { 11, null }
        };

        public IDictionary<int, int> _foodCount = new Dictionary<int, int>();


        public FoodOrderPageViewModel()
        {
            Users = new ObservableCollection<UserDTO>();
            Foods = new ObservableCollection<FoodDTO>();
            Foods.CollectionChanged += Foods_CollectionChanged;
            UserFoods = new ObservableCollection<UserFoodDTO>();
            //This need to be init in a static method
            GetUserFoodsData();
            getAllFoodCommand = new RelayCommand(async() => await GetAllFood());
            createFoodCommand = new RelayCommand(async() => await CreateFood());
            deleteFoodCommand = new RelayCommand(async () => await RemoveFood());
            deleteAllFoodCommand = new RelayCommand(async () => await RemoveAllFood());
            getUserCommand = new RelayCommand(async () => await GetUserFoodsData());
            getFoodFromClipboard = new RelayCommand(async () => await PasteFoodFromClipboard());
            notifyTeamCommand = new RelayCommand(async () => NotifyTeam());
            generateWordDocument = new RelayCommand(async () => await GenerateWordDocument());
        }

        private async Task GetAllFood() => foods = await httpHelper.GetAsync<ObservableCollection<FoodDTO>>(getFoodsDataUrl);
        private void BindFoodBackToUI(ObservableCollection<FoodDTO> foodList, ObservableCollection<UserFoodDTO> userFoods)
        {
            IsBusy = true;
            var numberOfUserFood = UserFoods.Count();
            foreach (var userFood in UserFoods)
            {
                if (_foodCount.ContainsKey(userFood.food.id))
                {
                    _foodCount[userFood.food.id] += 1;
                }
                else
                {
                    _foodCount.Add(userFood.food.id, 1);
                }
            }
            foreach (var food in foodList)
            {
                double valuePercent = 0;
                int numberOfSelectedUser = 0;
                if (_foodCount.ContainsKey(food.id))
                {
                    valuePercent = _foodCount[food.id] / (double)numberOfUserFood;
                    numberOfSelectedUser = _foodCount[food.id];
                }
                Foods.Add(new FoodDTO()
                {
                    id = food.id,
                    itemNo = Foods.Count() + 1,
                    foodName = food.foodName,
                    foodEnglishName = food.foodEnglishName,
                    mainIcon = food.mainIcon,
                    secondaryIcon = food.secondaryIcon,
                    Percentage = valuePercent * 100,
                    NumberOfSelectedUser = numberOfSelectedUser,
                    usersAvatar = userFoods.Where(f => f.food.id == food.id).Select(uf => uf.user.profilePic)
                                           .Where(i => i != App.localSettings.Values["ProfilePic"] as string).ToList<string>()
                });
            }
            IsBusy = false;
        }

        private async Task CreateFood()
        {
            CreateFood createFoodDialog = new CreateFood();
            createFoodDialog.PrimaryButtonClick += CreateFoodDialog_PrimaryButtonClick;
            await createFoodDialog.ShowAsync();
        }

        private async void CreateFoodDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            IsBusy = true;
            var food = (sender as CreateFood).Food;
            var createResult = await httpHelper.CreateAsync<FoodDTO>(createFoodDataUrl , food);
            if (createResult != null) 
            {
                createResult.itemNo = Foods.Count() + 1;
                Foods.Add(createResult);
            } else Debug.WriteLine("Create operation error");
            IsBusy = false;
        }

        private async Task RemoveFood()
        {
            IsBusy = true;
            var food = Foods.Where(f => f == SelectedFood).FirstOrDefault();
            var deleteResult = await httpHelper.RemoveAsync(deleteFoodDataUrl, food.id);
            if (deleteResult == true) Foods.Remove(food); else Debug.Write("Delete operation error");
            IsBusy = false;
        }

        private async Task RemoveAllFood()
        {
            IsBusy = true;
            var response = await httpHelper.DeleteAsync(deleteAllFoodDataUrl);
            if (response.StatusCode.ToString() == "NoContent")
            {
                Foods.ToList().All(i => Foods.Remove(i));
                App.localSettings.Values["FoodId"] = 0;
            }
            else Debug.WriteLine("Delete all operation error");
            IsBusy = false;
        }

        private async Task GetAllUsers()
            => users = await httpHelper.GetAsync<ObservableCollection<UserDTO>>(getUsersDataUrl);

        private void NotifyTeam() => new ToastContentBuilder()
                .SetToastScenario(ToastScenario.Reminder)
                .AddArgument("action", "viewFoodPage")
                .AddText("🍱 Lunch food is now ready !!!!")
                .AddText($"There are {Foods.Count} dishes 🍽 this week")
                .AddText("Deadline: 12:00PM Thursday noon ⏰")
                .AddHeroImage(new Uri("ms-appx:///Assets/FoodAssets/FoodToast.png"))
                .AddComboBox("foodList", "Top 5 food", Foods.OrderByDescending(f => f.Percentage).FirstOrDefault().id.ToString(),
                                                       Foods.OrderByDescending(f => f.Percentage).Select(f => (f.id.ToString(), f.foodEnglishName)).Take(5).ToArray())
                .AddButton(new ToastButton().SetContent("Order this food").AddArgument("chosenFood", "foodList"))
                .AddAudio(new ToastAudio() { Src = new Uri("ms-appx:///Assets/AppAudio/clearly-602.mp3") })
                .Show();

        private void BindUsersBackToUI(ObservableCollection<UserDTO> usersList)
        {
            foreach (var user in usersList) { Users.Add(user); }
        }

        private async Task GetUserFoodsData()
        {
            if (UserFoods.Count > 0) { Users.Clear(); UserFoods.Clear(); Foods.Clear(); await Task.Delay(1000); IsBusy = false; }
            var userSelectedFoods = await httpHelper.GetAsync<ObservableCollection<UserFoodDTO>>(getUserSelectedFoodDataUrl);
            NumberOfFood = userSelectedFoods.Count();

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

            BindUsersBackToUI(users);
            BindFoodBackToUI(foods, userSelectedFoods);
        }

        private async Task PasteFoodFromClipboard()
        {
            IsBusy = true;
            var dataPackageView = Clipboard.GetContent();
            IsBusy = false;
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                try
                {
                    var listText = await dataPackageView.GetTextAsync();
                    var pasteFoodDialog = new PasteFoodDialog(listText);
                    var result = await pasteFoodDialog.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        foreach (FoodDTO importFood in pasteFoodDialog.FoodListDialog)
                        {
                            Foods.Add(importFood);
                        }
                    }
                    else pasteFoodDialog.Hide();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error retrieving Text format from Clipboard: " + ex.Message);
                }
            }
            else
            {
                Debug.WriteLine(" + Environment.NewLine + ");
            }
        }

        private async Task GenerateWordDocument()
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("DOCX", new List<string>() { ".docx" });
            savePicker.SuggestedFileName = "FoodOrderSheet";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if(file != null)
            {
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        Body docBody = new Body();
                        Paragraph companyOrderingHeader = new Paragraph(new Run(new Text("Danh sách ăn trưa iDealogic và Devinition")) { RunProperties = new RunProperties() { Bold = new Bold() } });
                        OpenXMLWordHelper openXMLWordHelper = new OpenXMLWordHelper();
                        Table tbl = openXMLWordHelper.createWordTable();
                        TableRow headerRow = openXMLWordHelper.createTableWordRow(new string[] { "STT" , "Tên món", "Số lượng" }, true, false);
                        tbl.AppendChild(headerRow);
                        foreach (KeyValuePair<int, int> entry in _foodCount.OrderBy(x => x.Key))
                        {
                            TableRow foodRow = openXMLWordHelper.createTableWordRow(new string[] { Foods.Where(f => f.id == entry.Key).Select(food => food.itemNo).FirstOrDefault().ToString(), 
                                                                                                   Foods.Where(f => f.id == entry.Key).Select(food => food.foodName).FirstOrDefault(), 
                                                                                                   entry.Value.ToString() }, false, false);
                            tbl.AppendChild(foodRow);
                        }
                        TableRow footerTable = openXMLWordHelper.createTableWordRow(new string[] { "Tổng ", _foodCount.Sum(f => f.Value).ToString() }, false, true);
                        tbl.Append(footerTable);
                        docBody.AppendChild(companyOrderingHeader);
                        docBody.Append(tbl);
                        mainPart.Document = new Document(docBody);
                    }
                }
            }
        }

        private void Foods_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => DeleteAllFoodButtonState = Foods.Count > 0 ? true : false;
    }
}
