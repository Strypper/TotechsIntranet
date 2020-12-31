﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntranetUWP.Models
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool Company { get; set; }
    }

    public class DemoUserData
    {
        public static List<UserDTO> getData()
        {
            var data = new List<UserDTO>();
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair.jpg" });
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair1.jpg" });
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair2.jpg" });
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair3.jpg"});
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair4.jpg" });
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair5.jpg" });
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair6.jpg" });
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair7.jpg" });
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair8.jpg" });
            data.Add(new UserDTO() { ProfileImageUrl = "ms-appx:///Assets/DemoPurpose/Users/MenHair9.jpg" });
            return data;
        }
    }
}