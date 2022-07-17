using IntranetUWP.Helpers;
using IntranetUWP.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;


namespace IntranetUWP.UserControls
{
    public sealed partial class MemberCard : UserControl
    {
        public readonly string getProjectsByUserIdDataUrl = "UserProject/GetProjectByUser";
        public delegate void ClearSelectionMemberCardEventHandler(int userId);
        public delegate void DisableMemberCardEventHandler(int userId);
        public int UserId
        {
            get { return (int)GetValue(UserIdProperty); }
            set { SetValue(UserIdProperty, value); }
        }

        public static readonly DependencyProperty UserIdProperty =
            DependencyProperty.Register("UserId", typeof(int), typeof(MemberCard), null);

        public string ProfilePictureUrl
        {
            get { return (string)GetValue(ProfilePictureUrlProperty); }
            set { SetValue(ProfilePictureUrlProperty, value); }
        }

        public static readonly DependencyProperty ProfilePictureUrlProperty =
            DependencyProperty.Register("ProfilePictureUrl", typeof(string), typeof(MemberCard), new PropertyMetadata(null));

        public string EmployeeName
        {
            get { return (string)GetValue(EmployeeNameProperty); }
            set { SetValue(EmployeeNameProperty, value); }
        }

        public static readonly DependencyProperty EmployeeNameProperty =
            DependencyProperty.Register("EmployeeName", typeof(string), typeof(MemberCard), new PropertyMetadata(null));



        public bool Company
        {
            get { return (bool)GetValue(CompanyProperty); }
            set { SetValue(CompanyProperty, value); }
        }

        public static readonly DependencyProperty CompanyProperty =
            DependencyProperty.Register("Company", typeof(bool), typeof(MemberCard), new PropertyMetadata(true));



        public ObservableCollection<FoodDTO> FoodList
        {
            get { return (ObservableCollection<FoodDTO>)GetValue(FoodListProperty); }
            set { SetValue(FoodListProperty, value); }
        }

        public static readonly DependencyProperty FoodListProperty =
            DependencyProperty.Register("FoodList", typeof(ObservableCollection<FoodDTO>), typeof(MemberCard), new PropertyMetadata(null));

        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();

        public int SelectedFood
        {
            get { return (int)GetValue(SelectedFoodProperty); }
            set { SetValue(SelectedFoodProperty, value); }
        }

        public static readonly DependencyProperty SelectedFoodProperty =
            DependencyProperty.Register("SelectedFood", typeof(int), typeof(MemberCard), new PropertyMetadata(null));


        //EventHandler
        public event ClearSelectionMemberCardEventHandler ClearSelection;
        public event DisableMemberCardEventHandler DisableUser;
        
        public MemberCard()
        {
            this.InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedFood != -1 && FoodList != null)
            {
                var selectedFood = FoodList.Where(f => f.id == SelectedFood).FirstOrDefault();
                SelectFoodCombobox.SelectedItem = selectedFood != null ? selectedFood : null;
            }
            SelectFoodCombobox.SelectionChanged += SelectFoodCombobox_SelectionChanged;

            var projects = await httpHelper.GetByIdAsync<List<ProjectDTO>>(getProjectsByUserIdDataUrl, UserId);
            if (projects != null) ProjectsText.Text = projects.Count == 0 ? "Not assigned to project yet" : string.Join(", ", projects.Select(t => t.ProjectName));
        }

        private async void SelectFoodCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var food = SelectFoodCombobox.SelectedItem as FoodDTO;
            if(food != null)
            {
                var createupdateUserFoodDTO = new CreateUpdateUserFoodDTO();
                createupdateUserFoodDTO.userId = UserId;
                createupdateUserFoodDTO.foodId = food.id;
                await httpHelper.CreateAsync<UserFoodDTO>("UserFood/Create", createupdateUserFoodDTO);
            }
        }
        private void SwipeControl_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e) => FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        private void ClearSelection_Click(object sender, RoutedEventArgs e) => ClearSelection?.Invoke(UserId);
        private void DisableUser_Click(object sender, RoutedEventArgs e) => DisableUser?.Invoke(UserId);
        private void RemoveSelectionSwipe_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args) => ClearSelection?.Invoke(UserId);
        private void DisableUserSwipe_Invoked(SwipeItem sender, SwipeItemInvokedEventArgs args) => DisableUser?.Invoke(UserId);

        private void ProjectCard_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Avatar.Scale = new Vector3(1.08f, 1.08f, 0);

            EmployeeNameText.Translation = new Vector3(5, 0, 0);
            ProjectsText.Translation = new Vector3(10, 0, 0);
            ProjectsText.Opacity = 1;
            CompanyText.Translation = new Vector3(5, 0, 0);
        }

        private void ProjectCard_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Avatar.Scale = new Vector3(1, 1, 0);

            EmployeeNameText.Translation = new Vector3(0, 10, 0);
            ProjectsText.Translation = new Vector3(0, 0, 0);
            ProjectsText.Opacity = 0;
            CompanyText.Translation = new Vector3(0, -10, 0);
        }
    }
}
