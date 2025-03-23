using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeCicle.DAL.Repositories.Interfaces
{
    public interface IMealRepository
    {
        Task<List<Meal>> GetAllMealsAsync();
        Task<Meal?> GetMealByIdAsync(int id);
        Task<List<Meal?>> GetMealsByUserIdAsync(int userId);
        Task AddMealAsync(Meal meal);
        Task UpdateMealAsync(Meal meal);
        Task DeleteMealAsync(int id);
        Task<IEnumerable<FoodItem>> GetProductsForMealAsync(int mealId);
        
    }
}
