using System.Collections.ObjectModel;

namespace IntranetUWP.Models
{
    public class UserFoodDTO : BaseDTO
    {
        public UserDTO user { get; set; }
        public FoodDTO food { get; set; }
        public ObservableCollection<FoodDTO> foodList { get; set; }
    }

    public class CreateUpdateUserFoodDTO
    {
        public int userId { get; set; }
        public int foodId { get; set; }
    }
}
