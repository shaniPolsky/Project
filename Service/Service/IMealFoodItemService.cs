using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IMealFoodItemService
    {
        Task<List<MealFoodItem>> GetAllMealFoodItemsAsync();
        Task<MealFoodItem?> GetMealFoodItemByIdAsync(int mealId, int foodItemId);
        Task AddMealFoodItemAsync(MealFoodItem mealFoodItem);
        Task DeleteMealFoodItemAsync(int mealId, int foodItemId);
    }
}
