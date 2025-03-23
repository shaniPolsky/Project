using Models.Entities;

namespace LifeCicle.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task SaveChangesAsync(); // ⬅ הוספת הפונקציה
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<double> CalculateTDEE(User user);
        
    }
}
