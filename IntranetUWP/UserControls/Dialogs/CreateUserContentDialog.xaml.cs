using IntranetUWP.Helpers;
using IntranetUWP.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class CreateUserContentDialog : ContentDialog
    {
        public readonly string RegisterUrl = "https://intranetapi.azurewebsites.net/api/User/Create";
        public string getTeamsDataUrl = "Team/GetAll";
        HttpClient httpClient = new HttpClient();
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        private StorageFile userPhoto;
        private string AzureProfileImageUrl;
        public CreateUserContentDialog()
        {
            this.InitializeComponent();
        }
        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var teams = await httpHelper.GetAsync<IList<TeamsDTO>>(getTeamsDataUrl);
            //TeamsFinder.ItemsSource = teams;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            WorkingBar.ShowError = false;
            WorkingBar.Visibility = Visibility.Visible;
            await saveUserPhoto(userPhoto);
            RegistingModel signUpInfo = new RegistingModel()
            {
                userName = UserName.Text,
                password = "Welkom112!!@",
                company = iDealogicToggle.IsChecked == true ? true : false,
                firstName = FirstName.Text,
                middleName = MiddleName.Text,
                lastName = LastName.Text,
                role = Role.Text,
                gender = BoyToggle.IsChecked == true ? true : false,
                age = AgeSlider.Value.ToString(),
                profilePic = AzureProfileImageUrl,
                level = Level.Text
            };
            var content = new StringContent(JsonConvert.SerializeObject(signUpInfo), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(RegisterUrl, content);
            var result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode.ToString() == "Created")
            {
                this.Hide();
            }
            else
            {
                WorkingBar.ShowError = true;
                this.Title = "Something is wrong 🤔";
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void iDealogicToggle_Click(object sender, RoutedEventArgs e)
        {
            iDealogicToggle.IsChecked = true;
            DevinitionToggle.IsChecked = false;
        }

        private void DeviToggle_Click(object sender, RoutedEventArgs e)
        {
            DevinitionToggle.IsChecked = true;
            iDealogicToggle.IsChecked = false;
        }

        private async void AvatarUploadImage_OpenCameraEventHandler(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            userPhoto = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (userPhoto == null)
            {
                // User cancelled photo capture
                return;
            }
            else
            {
                var filestream = await userPhoto.OpenAsync(FileAccessMode.Read);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(filestream);
                Avatar.AvatarImage = bitmapImage;
            }
        }

        private async void AvatarUploadImage_OpenFileEventHandler(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            userPhoto = await picker.PickSingleFileAsync();
            if (userPhoto != null)
            {
                var filestream = await userPhoto.OpenAsync(FileAccessMode.Read);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(filestream);
                Avatar.AvatarImage = bitmapImage;
            }
        }

        private async Task saveUserPhoto(StorageFile photo)
        {
            if (photo == null)
            {
                Debug.WriteLine("No Photo Attach");
                this.Title = "\uE114";
            }
            else
            {
                CloudStorageAccount storageAccount = createStorageAccountFromConnectionString();
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("avatarstorage");
                await container.CreateIfNotExistsAsync();
                CloudBlockBlob blob = container.GetBlockBlobReference(photo.Name);
                await blob.UploadFromFileAsync(photo);
                AzureProfileImageUrl = blob.Uri.AbsoluteUri;
            }
        }

        private CloudStorageAccount createStorageAccountFromConnectionString()
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=intranetblobstorages;AccountKey=yDjXV6wOfzsTw4lK2TmSrwfHiKf3n+zIDm1urADfSdJQoWbVEOmFiN+Vay5KTMdfc6tOdr/Ghf4Yfk4M78s36A==;EndpointSuffix=core.windows.net");
            }
            catch (FormatException)
            {
                WorkingBar.ShowError = true;
                throw new FormatException("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
            }
            catch (ArgumentException)
            {
                WorkingBar.ShowError = true;
                throw new ArgumentException("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
            }

            return storageAccount;
        }

    }
}
