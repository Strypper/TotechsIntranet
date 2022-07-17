using IntranetUWP.Models;
using Refit;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntranetUWP.RefitInterfaces
{
    public interface IContributionData
    {
        [Get("/Contribution/GetAll")]
        Task<List<ContributionDTO>> GetAllContributions();

        [Get("/Contribution/Get/{id}")]
        Task<ContributionDTO> Get(int id);

        [Delete("/Contribution/DeleteAll")]
        Task<HttpResponseMessage> DeleteAll();
    }
}
