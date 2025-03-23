using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IMenuMealRepository
    {
        Task<List<MenuMeal>> GetAllMenuMealsAsync();
        Task AddMenuMealAsync(MenuMeal menuMeal);
        Task AddMenuMealsAsync(List<MenuMeal> menuMeals);
        Task SaveChangesAsync();
        Task<List<MenuMeal>> GetMealsByMenuIdAsync(int menuId);

    }
}
