using IntranetUWP.Models;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntranetUWP.RefitInterfaces
{
    public interface IUserData
    {
        [Get("/User/GetAll")]
        Task<List<UserDTO>> GetAllUsers();

        [Get("/User/Get/{Guid}")]
        Task<UserDTO> Get(string Guid);

        [Get("/User/GetUserBySingalRConnectionStringId/{singalRConnectionStringId}")]
        Task<UserDTO> GetUserBySingalRConnectionStringId(string singalRConnectionStringId);

        [Get("/User/GetUserByGuid/{guid}")]
        Task<UserDTO> GetUserByGuid(string guid);

        [Post("/User/Login")]
        Task<UserDTO> Login(LoginModel loginModel);

        [Delete("/User/Delete/{Guid}")]
        Task<HttpResponseMessage> DeleteUser(string Guid);

        [Put("/User/Update")]
        Task<HttpResponseMessage> Update(UserDTO userDTO);

        [Put("/User/UpdatePassword")]
        Task<HttpResponseMessage> UpdatePassword(UserDTO userDTO);
    }
}
