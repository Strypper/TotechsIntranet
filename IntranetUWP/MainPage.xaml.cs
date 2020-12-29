using IntranetUWP.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IntranetUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            TheMainFrame.Navigate(typeof(FoodOrderPage));
        }

        private void NavigationViewPanel_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            Microsoft.UI.Xaml.Controls.NavigationViewItem item = args.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem;
            NavView_Navigate(item);
        }

        private void NavView_Navigate(Microsoft.UI.Xaml.Controls.NavigationViewItem item)
        {
            switch (item.Name)
            {
                case "LunchOrder":
                    TheMainFrame.Navigate(typeof(FoodOrderPage));
                    break;
                case "TeaBreak":
                    TheMainFrame.Navigate(typeof(TeaBreakPage));
                    break;
                case "PlayTime":
                    TheMainFrame.Navigate(typeof(PlayTimePage));
                    break;
                case "ChatHub":
                    TheMainFrame.Navigate(typeof(ChatHubPage));
                    break;
                case "":
                    TheMainFrame.Navigate(typeof(ChatHubPage));
                    break;
            }
        }
    }
}
