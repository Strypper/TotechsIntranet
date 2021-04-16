using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntranetUWP.Models
{
    public class UserDTO : BaseDTO
    {
        public string userName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string profilePic { get; set; }
        public bool company { get; set; }
        public string age { get; set; }
    }

    public class RegistingModel : UserDTO
    {
        public string password { get; set; }
        public bool gender { get; set; }
    }

    public class DemoUserData
    {
        //public static List<UserDTO> getData()
        //{
        //    var data = new List<UserDTO>();
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair.jpg" });
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair1.jpg" });
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair2.jpg" });
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair3.jpg"});
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair4.jpg" });
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair5.jpg" });
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair6.jpg" });
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair7.jpg" });
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair8.jpg" });
        //    data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair9.jpg" });
        //    return data;
        //}
    }
}
