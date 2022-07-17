using IntranetUWP.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntranetUWP.RefitInterfaces
{
    public interface IProjectData
    {
        [Get("/Project/GetAll")]
        Task<List<ProjectDTO>> GetAllProjects();

        [Get("/Project/Get/{id}")]
        Task<ProjectDTO> Get(int id);

        [Get("/Project/GetAllProjectsWithMembers")]
        Task<List<ProjectDTO>> GetAllProjectsWithMembers();
    }
}
