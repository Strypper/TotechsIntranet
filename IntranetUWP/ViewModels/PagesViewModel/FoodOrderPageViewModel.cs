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
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Style = Windows.UI.Xaml.Style;

namespace IntranetUWP.ViewModels.PagesViewModel
{
    public class FoodOrderPageViewModel : ViewModelBase
    {
        public string getFoodsDataUrl = "Food/GetAll";
        public string createFoodDataUrl = "Food/Create";
        public string createFoodListUrl = "Food/CreateMultipleFood";
        public string updateFoodDataUrl = "Food/Update";
        public string deleteFoodDataUrl = "Food/Delete";
        public string deleteAllFoodDataUrl = "Food/DeleteAll";
        public string getUsersDataUrl = "User/GetAll";
        public string getUserSelectedFoodDataUrl = "UserFood/GetAll";
        public string deleteUserFoodDataUrl = "UserFood/Delete";
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        public ICommand getAllFoodCommand { get; set; }
        public ICommand createFoodCommand { get; set; }
        public ICommand askBeforeDeleteFoodCommand { get; set; }
        public ICommand deleteAllFoodCommand { get; set; }
        public ICommand editFoodCommand { get; set; }
        public ICommand getUserCommand { get; set; }
        public ICommand getFoodFromClipboard { get; set; }
        public ICommand notifyTeamCommand { get; set; }
        public ICommand generateWordDocument { get; set; }
        private ObservableCollection<UserDTO> users = new ObservableCollection<UserDTO>();
        public ObservableCollection<UserDTO> Users { get; set; }
        public ObservableCollection<UserDTO> NonSelectedUser { get; set; }
        private ObservableCollection<FoodDTO> foods { get; set; } = new ObservableCollection<FoodDTO>();
        public ObservableCollection<FoodDTO> Foods { get; set; }
        public ObservableCollection<UserFoodDTO> UserFoods { get; set; }
        public ObservableCollection<UserFoodDTO> iDealogicUsersFood { get; set; }
        public ObservableCollection<UserFoodDTO> DevinitionUsersFood { get; set; }
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
            IsBusy = true;
            Users = new ObservableCollection<UserDTO>();
            NonSelectedUser = new ObservableCollection<UserDTO>();
            Foods = new ObservableCollection<FoodDTO>();
            Foods.CollectionChanged += Foods_CollectionChanged;
            UserFoods = new ObservableCollection<UserFoodDTO>();
            iDealogicUsersFood = new ObservableCollection<UserFoodDTO>();
            DevinitionUsersFood = new ObservableCollection<UserFoodDTO>();
            //This need to be init in a static method
            GetUserFoodsData();
            getAllFoodCommand = new RelayCommand(async () => await GetAllFood());
            createFoodCommand = new RelayCommand(async () => await CreateFood());
            askBeforeDeleteFoodCommand = new RelayCommand(async () => await AskBeforeRemove());
            deleteAllFoodCommand = new RelayCommand(async () => await RemoveAllFood());
            editFoodCommand = new RelayCommand(async () => await EditFood());
            getUserCommand = new RelayCommand(async () => await GetUserFoodsData());
            getFoodFromClipboard = new RelayCommand(async () => await PasteFoodFromClipboard());
            notifyTeamCommand = new RelayCommand(async () => NotifyTeam());
            generateWordDocument = new RelayCommand(async () => await GenerateWordDocument());
        }
        private async Task GetAllFood() 
            => foods = await httpHelper.GetAsync<ObservableCollection<FoodDTO>>(getFoodsDataUrl);
        private async Task CreateFood()
        {
            CreateFood createFoodDialog = new CreateFood();
            createFoodDialog.PrimaryButtonClick += CreateFoodDialog_PrimaryButtonClick;
            await createFoodDialog.ShowAsync();
        }
        private async Task EditFood()
        {
            CreateFood createFoodDialog = new CreateFood();
            createFoodDialog.Food = SelectedFood;
            createFoodDialog.PrimaryButtonClick += EditFoodDialog_PrimaryButtonClick;
            createFoodDialog.SecondaryButtonClick += (s, a) => SelectedFood = null;
            await createFoodDialog.ShowAsync();
        }
        private async Task AskBeforeRemove()
        {
            var removeFood = SelectedFood;
            var confirmDeletButtonStyle = new Style(typeof(Button));
            confirmDeletButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, Colors.Red));
            confirmDeletButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, Colors.White));
            var confirmDeleteDialog = await new ContentDialog()
            {
                Title = "🗑 Delete this food ?",
                Content = $"Check before you delete {removeFood.foodEnglishName} ?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                PrimaryButtonStyle = confirmDeletButtonStyle,
                PrimaryButtonCommand = new RelayCommand(async () =>
                {
                    IsBusy = true;
                    if (removeFood != null)
                    {
                        var food = Foods.Where(f => f == removeFood).FirstOrDefault();
                        var deleteResult = await httpHelper.RemoveAsync(deleteFoodDataUrl, food.id);
                        if (deleteResult == true) Foods.Remove(food); else Debug.Write("Delete operation error");
                    }
                    IsBusy = false;
                })
            }.ShowAsync();
        }
        private async Task RemoveAllFood()
        {
            var confirmDeletButtonStyle = new Style(typeof(Button));
            confirmDeletButtonStyle.Setters.Add(new Setter(Button.BackgroundProperty, Colors.Red));
            confirmDeletButtonStyle.Setters.Add(new Setter(Button.ForegroundProperty, Colors.White));
            var confirmDialog = await new ContentDialog()
            {
                Title = "🗑 Delete all dishes",
                Content = "Be aware that the action you are about to do is delete all food data and users selections as well. Do you wish to continue ?",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                PrimaryButtonStyle = confirmDeletButtonStyle
            }.ShowAsync();
            if (confirmDialog == ContentDialogResult.Primary)
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
        }
        private async void CreateFoodDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            IsBusy = true;
            var food = (sender as CreateFood).Food;
            var createResult = await httpHelper.CreateAsync<FoodDTO>(createFoodDataUrl, food);
            if (createResult != null)
            {
                createResult.itemNo = Foods.Count() + 1;
                Foods.Add(createResult);
            }
            else Debug.WriteLine("Create operation error");
            IsBusy = false;
        }
        public async void EditFoodDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            IsBusy = true;
            var editedFood = (sender as CreateFood).Food;
            if (editedFood != null)
            {
                var food = Foods.Where(f => f.id == editedFood.id).FirstOrDefault();
                editedFood.id = food.id;
                editedFood.itemNo = food.itemNo;
                editedFood.IsSelected = food.IsUnavailable != true ? food.IsSelected : false;
                editedFood.NumberOfSelectedUser = food.IsUnavailable != true ? food.NumberOfSelectedUser : 0;
                editedFood.Percentage = food.IsUnavailable != true ? food.Percentage : 0;
                editedFood.IsUnavailable = food.IsUnavailable;
                var updateResult = await httpHelper.UpdateAsync(updateFoodDataUrl, editedFood);

                if (food.IsUnavailable == true)
                {
                    foreach (var userFood in UserFoods.Where(uf => uf.food.id == food.id).ToArray())
                    {
                        UserFoods.Remove(userFood);
                    }
                    //Trigger back the calculation

                    foreach (var remainFood in Foods.Where(f => f.IsUnavailable == false))
                    {
                        remainFood.Percentage = (remainFood.NumberOfSelectedUser / UserFoods.Count) * 100;
                    }

                    if (food.id == (int)App.localSettings.Values["FoodId"])
                    {
                        App.localSettings.Values["FoodId"] = 0;
                    }
                }

                if (updateResult == true)
                {
                    Foods.Remove(food);
                    Foods.Insert(editedFood.itemNo - 1, editedFood);
                }
            }
            IsBusy = false;
        }
        private async Task GetAllUsers()
            => users = await httpHelper.GetAsync<ObservableCollection<UserDTO>>(getUsersDataUrl);
        private void NotifyTeam() => new ToastContentBuilder()
                .SetToastScenario(ToastScenario.Reminder)
                .AddArgument("action", "viewFoodPage")
                .AddText("🍱 Lunch food is now ready !!!!")
                .AddText($"There are {Foods.Where(f => f.IsUnavailable == false).Count()} dishes 🍽 this week")
                .AddText("Deadline: 12:00PM Thursday noon ⏰")
                .AddHeroImage(new Uri("ms-appx:///Assets/FoodAssets/FoodToast.png"))
                .AddComboBox("foodList", "Top 5 food", Foods.OrderByDescending(f => f.Percentage).Where(f => f.IsUnavailable == false)
                                                            .FirstOrDefault().id.ToString(),
                                                       Foods.OrderByDescending(f => f.Percentage).Where(f => f.IsUnavailable == false)
                                                            .Select(f => (f.id.ToString(), f.foodEnglishName)).Take(5).ToArray())
                .AddButton(new ToastButton().SetContent("Order this food").AddArgument("chosenFood", "foodList"))
                .AddAudio(new ToastAudio() { Src = new Uri("ms-appx:///Assets/AppAudio/clearly-602.mp3") })
                .Show();
        private void BindUsersBackToUI(ObservableCollection<UserDTO> usersList)
        {
            foreach (var user in usersList) { Users.Add(user); }
        }
        private void BindFoodBackToUI(ObservableCollection<FoodDTO> foodList, ObservableCollection<UserFoodDTO> userFoods)
        {
            IsBusy = true;
            var numberOfUserFood = UserFoods.Count();
            //Add food to the dictionary _foodCount
            foreach (var userFood in UserFoods)
            {
                if (userFood.food.IsUnavailable == false)
                {   
                    //Increase number of food when it's the same one
                    if (_foodCount.ContainsKey(userFood.food.id))
                    {
                        _foodCount[userFood.food.id] += 1;
                    }
                    //If not yet exist in the dictionary add it 
                    else
                    {
                        _foodCount.Add(userFood.food.id, 1);
                    }
                }
            }

            var selectedUsers = UserFoods.Select(uf => uf.user);
            var nonSelectedUser = Users.Where(u => selectedUsers.All(su => su.id != u.id)).ToList();
            if (nonSelectedUser.Count > 0)
            {
                foreach (var user in nonSelectedUser)
                {
                    NonSelectedUser.Add(user);
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
                    IsUnavailable = food.IsUnavailable,
                    usersAvatar = userFoods.Where(f => f.food.id == food.id)
                                           .Select(uf => uf.user.profilePic)
                                           .Where(i => i != App.localSettings.Values["ProfilePic"] as string)
                                           .ToList<string>()
                });
                //Refactor this bruhhhh
                //By default the server not return ItemNo so this is register all of them back
                if (UserFoods.Any(uf => uf.food.id == food.id))
                {
                    foreach (var uf in UserFoods.Where(uf => uf.food.id == food.id))
                    {
                        uf.food.itemNo = Foods.Count();
                    }
                }
            }

            IsBusy = false;
        }
        private async Task GetUserFoodsData()
        {
            if (UserFoods.Count > 0) { Users.Clear(); UserFoods.Clear(); Foods.Clear(); await Task.Delay(1000); IsBusy = false; }
            var userSelectedFoods = await httpHelper.GetAsync<ObservableCollection<UserFoodDTO>>(getUserSelectedFoodDataUrl);
            NumberOfFood = userSelectedFoods.Count();

            await GetAllUsers();
            await GetAllFood();

            foreach (var user in Users)
            {
                if (userSelectedFoods.Any(u => u.user.id == user.id) == false)
                {
                    FoodDTO food = new FoodDTO() { id = -1 };
                    userSelectedFoods.Add(new UserFoodDTO() { user = user, food = food, foodList = Foods });
                }
            };

            foreach (var userSelectedFood in userSelectedFoods)
            {
                userSelectedFood.foodList = Foods;
                //Legacy code need change
                UserFoods.Add(userSelectedFood);

                if (userSelectedFood.user.company == true)
                {
                    iDealogicUsersFood.Add(userSelectedFood);
                }
                else DevinitionUsersFood.Add(userSelectedFood); 
            };



            BindUsersBackToUI(users);
            BindFoodBackToUI(foods, userSelectedFoods);
            IsBusy = false;
        }
        private async Task PasteFoodFromClipboard()
        {
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                try
                {
                    var listText = await dataPackageView.GetTextAsync();
                    var pasteFoodDialog = new PasteFoodDialog(listText);
                    var result = await pasteFoodDialog.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        var createResult = await httpHelper.CreateAsync<ObservableCollection<FoodDTO>>(createFoodListUrl, pasteFoodDialog.FoodListDialog);
                        if(createResult != null)
                        {
                            foreach (FoodDTO importFood in createResult)
                            {
                                Foods.Add(importFood);
                            }
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
            if (file != null)
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
                        TableRow headerRow = openXMLWordHelper.createTableWordRow(new string[] { "STT", "Tên món", "Số lượng" }, true, false);
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
