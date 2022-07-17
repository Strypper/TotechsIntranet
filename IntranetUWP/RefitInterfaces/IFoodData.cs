using IntranetUWP.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntranetUWP.RefitInterfaces
{
    public interface IFoodData
    {
        [Get("/Food/GetAll")]
        Task<List<FoodDTO>> GetAllFoods();

        [Get("/Food/Get/{id}")]
        Task<FoodDTO> Get(int id);
    }
}
