using LifeCicle.DAL.Data;
using LifeCicle.DAL.Repositories.Interfaces;
using Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeCicle.DAL.Repositories
{
    public class MealRepository : IMealRepository
    {
        private readonly ApplicationDbContext _context;

        public MealRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Meal>> GetAllMealsAsync()
        {
            return await _context.Meals.ToListAsync();
        }

        public async Task<Meal?> GetMealByIdAsync(int id)
        {
            return await _context.Meals
            //.Include(m => m.Recipes) // אם מתכונים קיימים, בטלי את ההערה
           .Include(m => m.MenuMeals)  // הוספת קשר לטבלת החיבורים
           .ThenInclude(mm => mm.Menu) // טעינת המידע מהתפריטים
           .Include(m => m.MealFoodItems)
           .ThenInclude(mf => mf.FoodItem)
           .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Meal>> GetMealsByUserIdAsync(int userId)
        {
            return await _context.Meals
                .Where(m => m.UserId == userId)
                .Include(m => m.MealFoodItems) // טוען את קשר הרבים-לרבים
                .ThenInclude(mf => mf.FoodItem) // מביא גם את פרטי המזון עצמם
                .ToListAsync();
        }

        public async Task AddMealAsync(Meal meal)
        {
            await _context.Meals.AddAsync(meal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMealAsync(Meal meal)
        {
            _context.Meals.Update(meal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMealAsync(int id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if (meal != null)
            {
                _context.Meals.Remove(meal);
                await _context.SaveChangesAsync();
            }
        }

        // פונקציה ב-Repository לשאילת המוצרים הקשורים לארוחה
     
        public async Task<IEnumerable<FoodItem>> GetProductsForMealAsync(int mealId)
        {
            // מניחים שיש לך טבלת Many-to-Many המקשרת בין Meals ו-FoodItems
            var mealProducts = await _context.MealFoodItems
                .Where(mf => mf.MealId == mealId)
                .Include(mf => mf.FoodItem)  // מבצע חיבור לטבלת FoodItem
                .Select(mf => mf.FoodItem)  // מחזיר את כל מוצרי המזון של הארוחה
                .ToListAsync();

            return mealProducts;
        }


    }
}
