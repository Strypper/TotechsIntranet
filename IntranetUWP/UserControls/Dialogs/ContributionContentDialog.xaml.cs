using IntranetUWP.Helpers;
using IntranetUWP.Models;
using IntranetUWP.RefitInterfaces;
using Microsoft.UI.Xaml.Controls;
using Refit;
using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class ContributionContentDialog : ContentDialog
    {
        public ContributionDTO Contribution { get; set; } = new ContributionDTO();
        public UserDTO         User         { get; set; }
        private PaymentTypeDTO PaymentType  { get; set; }

        public readonly string getUserDataUrl = "User/Get";

        private readonly IUserData userData = RestService.For<IUserData>(App.BaseUrl);
        public ContributionContentDialog()
        {
            this.InitializeComponent();
        }
        private async void ContentDialog_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            User = await userData.GetUserByGuid(App.localSettings.Values["UserGuid"].ToString());
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (User != null && DonateOnDatePicker.Date.Value.DateTime != null)
            {
                Contribution.Amount      = Convert.ToDecimal(MoneyAmountNumberBox.Value);
                Contribution.Contributor = User;
                Contribution.DonateOn    = DonateOnDatePicker.Date.Value.DateTime;
                Contribution.PaymentType = PaymentType;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void PaymentMethodOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ContentBorder != null && sender is RadioButtons rb)
            {
                string colorName = rb.SelectedItem as string;
                switch (colorName)
                {
                    case "Cash":
                        ContentBorder.Background  = new SolidColorBrush(Colors.Green);
                        PaymentMethodContent.Text = "Give directly to admin cash";
                        PaymentType               = PaymentTypeDTO.Cash;
                    break;
                    case "Momo":
                        ContentBorder.Background  = new SolidColorBrush(Colors.DeepPink);
                        PaymentMethodContent.Text = "Send through this phone number: 0348164682";
                        PaymentType               = PaymentTypeDTO.Momo;
                    break;
                    case "Bank":
                        ContentBorder.Background  = new SolidColorBrush(Colors.Crimson);
                        PaymentMethodContent.Text = "Send through TECHCOMBANK with this id number: 19032916596014";
                        PaymentType               = PaymentTypeDTO.Bank;
                    break;
                }
            }

        }

        private void DonateOnDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if(sender.Date.HasValue) IsPrimaryButtonEnabled = true;
        }
    }
}
