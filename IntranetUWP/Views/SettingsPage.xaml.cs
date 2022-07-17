using IntranetUWP.Models;
using IntranetUWP.RefitInterfaces;
using Refit;
using Syncfusion.UI.Xaml.Charts;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;


namespace IntranetUWP.Views
{
    public sealed partial class SettingsPage : Page
    {
        public ObservableCollection<Model>           Collection     { get; set; } = new ObservableCollection<Model>();
        public ObservableCollection<DeveloperStat>   DeveloperStats { get; set; } = new ObservableCollection<DeveloperStat>();
        public ObservableCollection<ContributionDTO> Contributions  { get; set; } = new ObservableCollection<ContributionDTO>();
        public UserDTO                               UserProfile    { get; set; } = new UserDTO();
        private readonly IContributionData contributionData = RestService.For<IContributionData>(App.BaseUrl);
        private readonly IUserData         userData         = RestService.For<IUserData>(App.BaseUrl);
        public SettingsPage()
        {
            this.InitializeComponent();


            Collection.Add(new Model() { Country = "US", YValue = 15 });
            Collection.Add(new Model() { Country = "Russia", YValue = 20 });
            Collection.Add(new Model() { Country = "Ukraine", YValue = 16 });
            Collection.Add(new Model() { Country = "VietNam", YValue = 25 });
            Collection.Add(new Model() { Country = "Bruh", YValue = 1 });
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var contributions = await contributionData.GetAllContributions();
            UserProfile       = await userData.Get(App.localSettings.Values["UserGuid"].ToString());
            foreach (var skill in UserProfile.Skills)
            {
                DeveloperStats.Add(new DeveloperStat() { StatName = skill.Name, YValue = skill.SkillValue });
            };
            contributions.ForEach(contribution => Contributions.Add(contribution));
        }
    }

    public class Model
    {
        public string Country { get; set; }
        public double YValue { get; set; }

    }
    public class DeveloperStat
    {
        public string StatName { get; set; }
        public double YValue { get; set; }
    }
}
