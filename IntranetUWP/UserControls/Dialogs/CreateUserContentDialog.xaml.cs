using IntranetUWP.Helpers;
using IntranetUWP.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class CreateUserContentDialog : ContentDialog
    {
        public readonly string RegisterUrl = "https://intranetapi.azurewebsites.net/api/User/Create";
        public string getProjectsDataUrl = "Project/GetAll";
        HttpClient httpClient = new HttpClient();
        private IntranetHttpHelper httpHelper = new IntranetHttpHelper();
        private StorageFile userPhoto, cardPhoto;
        private string AzureProfileImageUrl, ProfileCardImageUrl;
        public CreateUserContentDialog()
        {
            this.InitializeComponent();
        }
        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var projects = await httpHelper.GetAsync<IList<ProjectDTO>>(getProjectsDataUrl);
            ProjectsFinder.SuggestedItemsSource = projects;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            WorkingBar.ShowError  = false;
            WorkingBar.Visibility = Visibility.Visible;
            AzureProfileImageUrl = await saveCroppedImage(userPhoto, AvatarCropper, true);
            ProfileCardImageUrl  = await saveCroppedImage(cardPhoto, CardCropper, false);
            RegistingModel signUpInfo = new RegistingModel()
            {
                UserName       = UserName.Text,
                Password       = "Welkom112!!@",
                FirstName      = FirstName.Text,
                MiddleName     = MiddleName.Text,
                LastName       = LastName.Text,
                DateOfBirth    = DateOfBirthPicker.Date.DateTime,
                Gender         = BoyToggle.IsChecked == true ? true : false,
                ProfilePic     = AzureProfileImageUrl,
            };
            var content = new StringContent(JsonConvert.SerializeObject(signUpInfo), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(RegisterUrl, content);
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
 
            }
        }

        private async void AvatarUploadImage_OpenFileEventHandler(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            userPhoto = await picker.PickSingleFileAsync();
            cardPhoto = userPhoto;
            if (userPhoto != null)
            {
                await CardCropper.LoadImageFromFile(cardPhoto);
                await AvatarCropper.LoadImageFromFile(userPhoto);
            }
        }

        private async Task<string> saveCroppedImage(StorageFile storageFile, ImageCropper imageCropper, bool isAvatar)
        {
            using (var fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                await imageCropper.SaveAsync(fileStream, BitmapFileFormat.Png);
            }
            return await saveUserPhoto(storageFile, isAvatar);
        }

        private async Task<string> saveUserPhoto(StorageFile photo, bool isAvatar)
        {
            if (photo == null)
            {
                Debug.WriteLine("No Photo Attach");
                this.Title = "\uE114";
                return null;
            }
            else
            {
                CloudStorageAccount storageAccount = createStorageAccountFromConnectionString();
                CloudBlobClient blobClient         = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container       = blobClient.GetContainerReference(isAvatar ? "avatarstorage" : "cardpics");
                await container.CreateIfNotExistsAsync();
                CloudBlockBlob blob = container.GetBlockBlobReference(photo.Name);
                await blob.UploadFromFileAsync(photo);
                return blob.Uri.AbsoluteUri;
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

        private void ProjectsFinder_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

        }
    }
}
