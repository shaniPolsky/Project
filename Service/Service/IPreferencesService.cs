using Models.Entities;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IPreferencesService
    {
        Task<List<FoodItem>> FilterProductsAsync(int userId);

        Task<Preferences?> GetPreferencesByUserIdAsync(int userId);
        Task<bool> UpdatePreferencesAsync(int userId, Preferences updatedPreferences);

      
    }
}
