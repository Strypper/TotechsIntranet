namespace IntranetUWP.RefitInterfaces
{
    public interface IUserFoodData
    {
        [Get("/UserFood/GetAll")]
        Task<List<UserFoodDTO>> GetAllUsers();

        [Delete("/UserFood/Delete")]
        Task<UserFoodDTO> DeleteUserFood(int id);
    }
}
