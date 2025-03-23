using LifeCicle.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MenuMealRepository : IMenuMealRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuMealRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuMeal>> GetAllMenuMealsAsync()
        {
            return await _context.MenuMeal.ToListAsync();
        }

        public async Task AddMenuMealAsync(MenuMeal menuMeal)
        {
            _context.MenuMeal.Add(menuMeal);
            await _context.SaveChangesAsync();
        }

        public async Task AddMenuMealsAsync(List<MenuMeal> menuMeals)
        {
            await _context.MenuMeal.AddRangeAsync(menuMeals);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<MenuMeal>> GetMealsByMenuIdAsync(int menuId)
        {
            return await _context.MenuMeal
                                 .Where(mm => mm.MenuId == menuId)
                                 .ToListAsync();
        }

    }
}
