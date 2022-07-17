using IntranetUWP.Contracts;
using IntranetUWP.Helpers;
using IntranetUWP.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Threading.Tasks;

namespace IntranetUWP.Services
{
    public class ToolkitObjectStorageServices : ILocalUsersService
    {
        private ApplicationDataStorageHelper _appDataStorageHelper;
        public ToolkitObjectStorageServices(UserSerializer serializer)
        {
            _appDataStorageHelper = ApplicationDataStorageHelper.GetCurrent(serializer);
        }

        public async Task<UserDTO> GetLocalUserAsync(string userGuid)
        {
            return _appDataStorageHelper.Read<UserDTO>(userGuid);
        }

        public void RemoveLocalUser(string userGuid)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLocalUserAsync(string userGuid)
        {
            throw new NotImplementedException();
        }

        public async Task SaveLocalUserAsync(UserDTO currentUser)
        {
            _appDataStorageHelper.Save(currentUser.Guid, currentUser);
        }
    }
}
