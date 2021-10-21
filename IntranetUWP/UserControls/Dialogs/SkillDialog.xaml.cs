using IntranetUWP.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace IntranetUWP.UserControls.Dialogs
{
    public sealed partial class SkillDialog : ContentDialog
    {
        public ObservableCollection<SkillDTO> Skills { get; set; } = new ObservableCollection<SkillDTO>();
        public SkillDialog()
        {
            this.InitializeComponent();
        }
        
        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            SkillName.Focus(FocusState.Programmatic);
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void AddSkill_Click(object sender, RoutedEventArgs e)
        {
            Skills.Add(new SkillDTO() 
            {
                Name = SkillName.Text,
                SkillValue = SkillValue.Value
            });
            SkillName.Text = "";
            SkillValue.Value = 0;
            SkillName.Focus(FocusState.Programmatic);
        }

        public static bool EnableAddSkillBySkillValue(double skillValue)
        {
            return skillValue > 0f ? true : false;
        }

        private void DeleteSkill_Swipe(SwipeItem sender, SwipeItemInvokedEventArgs args)
        {
            var removeSkill = (SkillDTO)args.SwipeControl.DataContext;
            Skills.Remove(Skills.Where(s => s.Name == removeSkill.Name).FirstOrDefault());
        }

        private void DeleteSkill_Click(object sender, RoutedEventArgs e)
        {
            var removeSkill = (SkillDTO)((FrameworkElement)sender).DataContext;
            Skills.Remove(Skills.Where(s => s.Name == removeSkill.Name).FirstOrDefault());
        }
    }
}
