using IntranetUWP.Models;
using System.Threading.Tasks;

namespace IntranetUWP.Contracts
{
    public interface ILocalUsersService
    {
        Task<UserDTO> GetLocalUserAsync(string userGuid);
        Task          SaveLocalUserAsync(UserDTO currentUser);
        Task          RemoveLocalUserAsync(string userGuid);
        void          RemoveLocalUser(string userGuid);
    }
}
