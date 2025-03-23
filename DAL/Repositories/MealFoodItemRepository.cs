using Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeCicle.DAL.Data;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class MealFoodItemRepository : IMealFoodItemRepository
    {
        private readonly ApplicationDbContext _context;

        public MealFoodItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MealFoodItem>> GetAllMealFoodItemsAsync()
        {
            return await _context.MealFoodItems
                .Include(mf => mf.Meal)
                .Include(mf => mf.FoodItem)
                .ToListAsync();
        }

        public async Task<List<MealFoodItem>> GetMealFoodItemsByMealIdAsync(int mealId)
        {
            return await _context.MealFoodItems
                .Where(mf => mf.MealId == mealId)
                .Include(mf => mf.FoodItem)
                .ToListAsync();
        }

        public async Task<List<MealFoodItem>> GetMealFoodItemsByFoodItemIdAsync(int foodItemId)
        {
            return await _context.MealFoodItems
                .Where(mf => mf.FoodItemId == foodItemId)
                .Include(mf => mf.Meal)
                .ToListAsync();
        }

        public async Task AddMealFoodItemAsync(MealFoodItem mealFoodItem)
        {
            await _context.MealFoodItems.AddAsync(mealFoodItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMealFoodItemAsync(int id)
        {
            var entity = await _context.MealFoodItems.FindAsync(id);
            if (entity != null)
            {
                _context.MealFoodItems.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // פונקציה לשליפת כל המוצרים עבור ארוחה נתונה
         public async Task<List<MealFoodItem>> GetFoodItemsForMeal(int mealId)
        {
            return await _context.MealFoodItems
                .Where(mf => mf.MealId == mealId) // שליפת כל הערכים של הארוחה
                .ToListAsync();
        }
    }
}
