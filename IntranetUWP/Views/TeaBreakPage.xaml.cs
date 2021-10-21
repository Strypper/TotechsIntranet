using IntranetUWP.Models;
using IntranetUWP.ViewModels.PagesViewModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TeaBreakPage : Page
    {
        public FoodOrderPageViewModel vm = new FoodOrderPageViewModel();
        public ObservableCollection<TeamsDTO> teams { get; set; }
        public TeaBreakPage()
        {
            this.InitializeComponent();
            this.DataContext = vm;

            teams = DemoTeamsData.getData();
        }

        //private void Canvas_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        //{
        //    var position = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
        //    Debug.WriteLine(position.X - Window.Current.Bounds.X);
        //    Debug.WriteLine(position.Y - Window.Current.Bounds.Y);
        //    Canvas.SetLeft(YellowRec, position.X - Window.Current.Bounds.X - 100);
        //    Canvas.SetTop(YellowRec, position.Y - Window.Current.Bounds.Y - 200);
        //}

        //private void YellowRec_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        //{
        //    YellowRec.Scale = new Vector3(1.2f, 1.2f, 1);
        //    YellowRec.Translation = new Vector3(10, 10, 0);
        //}

        //private void YellowRec_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        //{
        //    YellowRec.Scale = new Vector3(1.0f, 1.0f, 1);
        //    YellowRec.Translation = new Vector3(0, 0, 0);
        //}
    }
}
