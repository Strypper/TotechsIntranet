using IntranetUWP.Models;
using Refit;
using System.Threading.Tasks;

namespace IntranetUWP.RefitInterfaces
{
    public interface ITotechsIdentity
    {
        [Post("/Access/Login")]
        Task<IdentityResultDTO> Authenticate(LoginModel loginModel);
    }
}