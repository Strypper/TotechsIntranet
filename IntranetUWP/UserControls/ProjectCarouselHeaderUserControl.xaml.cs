using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace IntranetUWP.UserControls
{
    public sealed partial class ProjectCarouselHeaderUserControl : UserControl
    {
        public ObservableCollection<string> CarouselPhotos { get; set; } = new ObservableCollection<string>();
        public ProjectCarouselHeaderUserControl()
        {
            this.InitializeComponent();
            CarouselPhotos.Add("ms-appx:///Assets/DemoPurpose/Others/Projects/ChatHub.png");
            CarouselPhotos.Add("ms-appx:///Assets/DemoPurpose/Others/Projects/FoodPicker.png");
            CarouselPhotos.Add("ms-appx:///Assets/DemoPurpose/Others/Projects/GamePage.png");
            CarouselPhotos.Add("ms-appx:///Assets/DemoPurpose/Others/Projects/MemberDetail.png");
            CarouselPhotos.Add("ms-appx:///Assets/DemoPurpose/Others/Projects/MemberPage.png");
            CarouselPhotos.Add("ms-appx:///Assets/DemoPurpose/Others/Projects/WelcomePage.png");
        }
    }
}
