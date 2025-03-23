using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IFoodItemService
    {
        Task<List<FoodItem>> GetAllFoodItemsAsync();
        Task<FoodItem?> GetFoodItemByIdAsync(int id);
        Task AddFoodItemAsync(FoodItem foodItem);
        Task UpdateFoodItemAsync(FoodItem foodItem);
        Task DeleteFoodItemAsync(int id);

    

    }
}
