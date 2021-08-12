using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IntranetUWP.Models
{
    public class TeamsDTO : BaseDTO
    {
        public string TeamName { get; set; }
        public string Clients { get; set; }
        public string About { get; set; }
        public bool Company { get; set; }
        public int TechLead { get; set; } = 0;
        public ObservableCollection<UserDTO> Members { get; set; } = new ObservableCollection<UserDTO>();
    }

    public class DemoTeamsData
    {
        public static ObservableCollection<TeamsDTO> getData()
        {
            var costsHubAbout = "Peter Fitzpatrick offer a varied team of Legal Costs Accountants, Solicitors and Mediators of unrivalled experience. Our team of 12 is also supported by our panel of specialist legal costs barristers who have chambers within our premises.";
            var threadAbout = "Thread Legal is an innovative practice management solution, built with Microsoft, that enables lawyers to save time and increase productivity. Our software is a ‘pure cloud’ product, giving lawyers the flexibility to work on the move, and on mobile devices or laptops.";
            var data = new ObservableCollection<TeamsDTO>();
            
            data.Add(new TeamsDTO() 
            { 
                id = 1,
                TeamName = "CostsHub",
                About = costsHubAbout,
                Company = true,
                Members = DemoUserData.getData(),
            });
            data.Add(new TeamsDTO()
            {
                id = 2,
                TeamName = "Thread Legal",
                About = threadAbout,
                Company = true,
                Members = DemoUserData.getData(),
            });
            data.Add(new TeamsDTO()
            {
                id = 3,
                TeamName = "Salar",
                About = threadAbout,
                Company = true,
                Members = DemoUserData.getData(),
            });
            data.Add(new TeamsDTO()
            {
                id = 4,
                TeamName = "Pro Agenda",
                About = threadAbout,
                Company = true,
                Members = DemoUserData.getData(),
            });
            data.Add(new TeamsDTO()
            {
                id = 5,
                TeamName = "PhP Team",
                About = threadAbout,
                Company = false,
                Members = DemoUserData.getData(),
            });
            data.Add(new TeamsDTO()
            {
                id = 6,
                TeamName = ".NET Team",
                About = threadAbout,
                Company = false,
                Members = DemoUserData.getData()
            });
            data.Add(new TeamsDTO()
            {
                id = 7,
                TeamName = "Diskard",
                About = threadAbout,
                Company = false,
                Members = new ObservableCollection<UserDTO>()
            });
            return data;
        }
    }
}
