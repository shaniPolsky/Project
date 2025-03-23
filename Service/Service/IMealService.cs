using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IMealService
    {
        Task<List<Meal>> GetAllMealsAsync();
        Task<Meal?> GetMealByIdAsync(int id);
        Task<List<Meal>> GetMealsByUserIdAsync(int userId); 
        Task<bool> AddMealAsync(Meal meal);
        Task<bool> UpdateMealAsync(Meal meal);
        Task<bool> DeleteMealAsync(int id);
        Task<List<double>> CalculateCaloriesPerMealAsync(int userId);
        Task<List<Meal>> GenerateMealsForUserAsync(int userId);
        Task<IEnumerable<FoodItem >> GetProductsForMealAsync(int mealId);
        Task<List<FoodItem>> GetFoodItemsForMealAsync(int mealId);
    }
}
