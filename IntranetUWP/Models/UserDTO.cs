using System;
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
            data.Add(new UserDTO() { UserName = "Đỗ Thanh Hưng", Company = true });
            data.Add(new UserDTO() { UserName = "Đỗ Thanh Hưng", Company = true });
            data.Add(new UserDTO() { UserName = "Đỗ Thanh Hưng", Company = true });
            data.Add(new UserDTO() { UserName = "Đỗ Thanh Hưng", Company = true });
            return data;
        }
    }
}
