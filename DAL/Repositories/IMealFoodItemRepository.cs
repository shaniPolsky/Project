using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IMealFoodItemRepository
    {
        Task<List<MealFoodItem>> GetAllMealFoodItemsAsync();
        Task<List<MealFoodItem>> GetMealFoodItemsByMealIdAsync(int mealId);
        Task<List<MealFoodItem>> GetMealFoodItemsByFoodItemIdAsync(int foodItemId);
        Task AddMealFoodItemAsync(MealFoodItem mealFoodItem);
        Task DeleteMealFoodItemAsync(int id);
        Task<List<MealFoodItem>> GetFoodItemsForMeal(int mealId);
    }
}
