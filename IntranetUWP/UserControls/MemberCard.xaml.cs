﻿using IntranetUWP.Helpers;
using IntranetUWP.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class MemberCard : UserControl
    {
        public readonly string getTeamsByUserIdDataUrl = "UserTeam/GetTeamByUser";
        public delegate void ClearSelectionMemberCardEventHandler(int userId);
        public delegate void DisableMemberCardEventHandler(int userId);
        public int UserId
        {
            get { return (int)GetValue(UserIdProperty); }
            set { SetValue(UserIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for userId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserIdProperty =
            DependencyProperty.Register("UserId", typeof(int), typeof(MemberCard), null);

        public string ProfilePictureUrl
        {
            get { return (string)GetValue(ProfilePictureUrlProperty); }
            set { SetValue(ProfilePictureUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProfilePictureUrl.  This enables animation, styling, binding, etc...
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

        // Using a DependencyProperty as the backing store for Company.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompanyProperty =
            DependencyProperty.Register("Company", typeof(bool), typeof(MemberCard), new PropertyMetadata(true));



        public ObservableCollection<FoodDTO> FoodList
        {
            get { return (ObservableCollection<FoodDTO>)GetValue(FoodListProperty); }
            set { SetValue(FoodListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodListProperty =
            DependencyProperty.Register("FoodList", typeof(ObservableCollection<FoodDTO>), typeof(MemberCard), new PropertyMetadata(null));

        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();

        public int SelectedFood
        {
            get { return (int)GetValue(SelectedFoodProperty); }
            set { SetValue(SelectedFoodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedFood.  This enables animation, styling, binding, etc...
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

            var teams = await httpHelper.GetByIdAsync<List<TeamsDTO>>(getTeamsByUserIdDataUrl, UserId);
            TeamsText.Text = teams.Count == 0 ? "Not assigned to team yet" : string.Join(", ", teams.Select(t => t.TeamName));
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

        private void TeamCard_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Avatar.Scale = new Vector3(1.08f, 1.08f, 0);

            EmployeeNameText.Translation = new Vector3(5, 0, 0);
            TeamsText.Translation = new Vector3(10, 0, 0);
            TeamsText.Opacity = 1;
            CompanyText.Translation = new Vector3(5, 0, 0);
        }

        private void TeamCard_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Avatar.Scale = new Vector3(1, 1, 0);

            EmployeeNameText.Translation = new Vector3(0, 10, 0);
            TeamsText.Translation = new Vector3(0, 0, 0);
            TeamsText.Opacity = 0;
            CompanyText.Translation = new Vector3(0, -10, 0);
        }
    }
}
