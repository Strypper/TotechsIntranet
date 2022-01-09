using IntranetUWP.Ultils;
using System;
using System.Collections.ObjectModel;

namespace IntranetUWP.Models
{
    public class UserDTO : BaseDTO
    {
        public string userName { get; set; }
        public string firstName { get; set; } = String.Empty;
        public string middleName { get; set; } = String.Empty;
        public string lastName { get; set; } = String.Empty;
        public string profilePic { get; set; }
        public bool company { get; set; }
        public string role { get; set; }
        public string level { get; set; }
        public string bio { get; set; }
        public string phoneNumber { get; set; }

        public string country { get; set; } = String.Empty;
        public string former { get; set; } = String.Empty;
        public string hobby { get; set; } = String.Empty;

        private string _specialAward;

        public string specialAward
        {
            get { return _specialAward; }
            set 
            { 
                _specialAward = value;
                OnPropertyChanged();
            }
        }

        public string relationship { get; set; } = String.Empty;
        public int? like { get; set; }
        public int? friendly { get; set; }
        public int? funny { get; set; }
        public int? enthusiastic { get; set; }

        public DateTime? dateOfBirth { get; set; }

        public ObservableCollection<SkillDTO> skills { get; set; }

        public int age
        {
            get => dateOfBirth.GetAge();
        }
    }

    public class RegistingModel : UserDTO
    {
        public string password { get; set; }
        public bool gender { get; set; }
    }

    public class DemoUserData
    {
        public static ObservableCollection<UserDTO> getData()
        {
            var data = new ObservableCollection<UserDTO>();
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair.jpg" });
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair1.jpg" });
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair2.jpg" });
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair3.jpg" });
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair4.jpg" });
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair5.jpg" });
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair6.jpg" });
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair7.jpg" });
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair8.jpg" });
            data.Add(new UserDTO() { profilePic = "ms-appx:///Assets/DemoPurpose/Users/MenHair9.jpg" });
            return data;
        }
    }
}
