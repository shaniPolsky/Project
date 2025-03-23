using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeCicle.DAL.Repositories.Interfaces
{
    public interface IFoodItemRepository
    {
        Task<List<FoodItem>> GetAllFoodItemsAsync();
        Task<FoodItem?> GetFoodItemByIdAsync(int id);
        Task AddFoodItemAsync(FoodItem foodItem);
        Task UpdateFoodItemAsync(FoodItem foodItem);
        Task DeleteFoodItemAsync(int id);
        Task<List<FoodItem>> GetFilteredProductsByPreferencesAsync(Preferences preferences);
        
    }
}
