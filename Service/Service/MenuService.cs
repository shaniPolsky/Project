using DAL.Repositories;
using DAL.Repositories.Interfaces;
using LifeCicle.DAL.Repositories.Interfaces;
using LifeCicle.DAL.Repositories;
using Models.Entities;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuMealRepository _menuMealRepository;
        private readonly IMealService _mealService;
        private readonly IMealFoodItemRepository _mealFoodItemRepository;

        public MenuService(
            IUserRepository userRepository,
            IMealRepository mealRepository,
            IMenuRepository menuRepository,
            IMenuMealRepository menuMealRepository,
            IMealService mealService,
            IMealFoodItemRepository mealFoodItemRepository
            )
        {
            _userRepository = userRepository;
            _mealRepository = mealRepository;
            _menuRepository = menuRepository;
            _menuMealRepository = menuMealRepository;
            _mealService = mealService;
            _mealFoodItemRepository = mealFoodItemRepository;
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            return await _menuRepository.GetAllMenusAsync();
        }

        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await _menuRepository.GetMenuByIdAsync(id);
        }

        public async Task<List<Menu>> GetMenusByUserIdAsync(int userId)
        {
            return await _menuRepository.GetMenusByUserIdAsync(userId);
        }

        public async Task AddMenuAsync(Menu menu)
        {
            await _menuRepository.AddMenuAsync(menu);
        }

        public async Task<bool> UpdateMenuAsync(int id, Menu updatedMenu)
        {
            var existingMenu = await _menuRepository.GetMenuByIdAsync(id);
            if (existingMenu == null)
                return false;

            existingMenu.MenuName = updatedMenu.MenuName;
            existingMenu.Calories = updatedMenu.Calories;
            existingMenu.Description = updatedMenu.Description;
            existingMenu.MenuStatus = updatedMenu.MenuStatus;
            existingMenu.MenuDuration = updatedMenu.MenuDuration;
            existingMenu.UpdatedAt = System.DateTime.UtcNow;

            await _menuRepository.UpdateMenuAsync(existingMenu);
            return true;
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            var existingMenu = await _menuRepository.GetMenuByIdAsync(id);
            if (existingMenu == null)
                return false;

            await _menuRepository.DeleteMenuAsync(id);
            return true;
        }

        public async Task AddMenuMealAsync(MenuMeal menuMeal)
        {
            if (menuMeal == null)
            {
                throw new ArgumentNullException(nameof(menuMeal));
            }

            await _menuMealRepository.AddMenuMealsAsync(new List<MenuMeal> { menuMeal });
            await _menuMealRepository.SaveChangesAsync();
        }

     
        // ✅ יצירת תפריט למשתמש
        public async Task<Menu> GenerateMenuAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            var meals = await _mealService.GenerateMealsForUserAsync(userId);
            if (meals == null || !meals.Any()) return null;

            double totalCalories = meals.Sum(m => m.Calories);

            // יצירת התפריט
            var menu = new Menu
            {
                UserId = userId,
                MenuName = "Custom Plan",
                Calories = (int)totalCalories,
                MenuDuration = 7,
                Description = "Personalized meal plan",
                MenuStatus = "Active"
            };

            await _menuRepository.AddMenuAsync(menu);
            await _menuRepository.SaveChangesAsync(); // שמירה כדי לקבל ID לתפריט
            
           
            // יצירת קשרים בין התפריט לבין הארוחות
           
            var menuMeals = meals.Select(meal => new MenuMeal
            {
                MenuId = menu.Id, // קישור התפריט לארוחה
                MealId = meal.Id
            }).ToList();


            Console.WriteLine($"📌 Trying to save {menuMeals.Count} MenuMeal entries...");

            foreach (var mm in menuMeals)
            {
                Console.WriteLine($"🔹 MenuMeal -> MenuId: {mm.MenuId}, MealId: {mm.MealId}");
            }

            await _menuMealRepository.AddMenuMealsAsync(menuMeals);
            await _menuMealRepository.SaveChangesAsync();            // שמירת הקשרים בטבלה MenuMeal  // עכשיו ה-Id מתעדכן ב-DB!

            // 🔥 מחזיר את התפריט עם כל המידע, כולל הארוחות והמזון שבכל ארוחה
            return await GetFullMenuAsync(menu.Id);
        }




        // ✅ פונקציה שמחזירה תפריט מלא כולל הארוחות והמזון שבכל ארוחה
        public async Task<Menu> GetFullMenuAsync(int menuId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (menu == null) return null;

            var menuMeals = await _menuMealRepository.GetMealsByMenuIdAsync(menuId);
            foreach (var menuMeal in menuMeals)
            {
                menuMeal.Meal = await _mealRepository.GetMealByIdAsync(menuMeal.MealId);
                menuMeal.Meal.MealFoodItems = await _mealFoodItemRepository.GetMealFoodItemsByFoodItemIdAsync(menuMeal.MealId);
            }

            menu.MenuMeals = menuMeals;
            return menu;
        }

    }
}
    

