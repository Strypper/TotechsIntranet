using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace IntranetUWP.UserControls
{
    public sealed partial class FoodCard : UserControl
    {
        public string FoodName
        {
            get { return (string)GetValue(FoodNameProperty); }
            set { SetValue(FoodNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodNameProperty =
            DependencyProperty.Register("FoodName", typeof(string), typeof(FoodCard), null);



        public string FoodEnglishName
        {
            get { return (string)GetValue(FoodEnglishNameProperty); }
            set { SetValue(FoodEnglishNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FoodEnglishName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FoodEnglishNameProperty =
            DependencyProperty.Register("FoodEnglishName", typeof(string), typeof(FoodCard), null);
        public FoodCard()
        {
            this.InitializeComponent();
        }
    }
}
