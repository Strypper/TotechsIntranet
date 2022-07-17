using IntranetUWP.Contracts;
using IntranetUWP.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace IntranetUWP.Services
{
    public class StorageFileServices : ILocalUsersService
    {
        public StorageFileServices()
        {
        }
        public async Task<UserDTO> GetLocalUserAsync(string userGuid)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync("UserJsonData");
            var textData = await FileIO.ReadTextAsync(file);
            return JsonConvert.DeserializeObject<UserDTO>(textData);
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
            var userJsonData = await ApplicationData.Current.LocalFolder.CreateFileAsync("UserJsonData", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(userJsonData, JsonConvert.SerializeObject(currentUser));
        }
    }
}
