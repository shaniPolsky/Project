using Models.Entities;
using System.Threading.Tasks;

namespace LifeCicle.DAL.Repositories
{
    public interface IPreferencesRepository
    {
        Task<Preferences?> GetPreferencesByUserIdAsync(int userId);
        Task AddPreferencesAsync(Preferences preferences);
        Task SaveChangesAsync(); // ⬅ הוספת הפונקציה
        Task UpdatePreferencesAsync(Preferences preferences);
        Task DeletePreferencesByUserIdAsync(int userId);
        Task<List<FoodItem>> GetFilteredProductsByPreferencesAsync(Preferences preferences);
    }
}
