using Models.Entities;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> RegisterUserAsync(User user);
        Task<bool> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        Task<double> CalculateMealCaloriesAsync(int id);
        Task<double> CalculateTDEE(User user);
    }
}
