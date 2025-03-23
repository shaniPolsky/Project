using DAL.Repositories.Interfaces;
using LifeCicle.DAL.Repositories;
using LifeCicle.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Models.Enums.Enums;

namespace Service.Services
{
    public class MealService : IMealService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IFoodItemRepository _FoodItemRepository;
        private readonly IPreferencesRepository _preferencesRepository;
        private readonly IMealFoodItemRepository _mealFoodItemRepository;



        public MealService(
            IUserRepository userRepository,
            IMealRepository mealRepository,
            IFoodItemRepository FoodItemRepository,
            IPreferencesRepository preferencesRepository,
            IMealFoodItemRepository mealFoodItemRepository
            )
        {
            _userRepository = userRepository;
            _mealRepository = mealRepository;
            _FoodItemRepository = FoodItemRepository;
            _preferencesRepository = preferencesRepository;
            _mealFoodItemRepository = mealFoodItemRepository;

        }

        public async Task<List<Meal>> GetAllMealsAsync()
        {
            return await _mealRepository.GetAllMealsAsync();
        }

        public async Task<Meal?> GetMealByIdAsync(int id)
        {
            return await _mealRepository.GetMealByIdAsync(id);
        }

        public async Task<List<Meal>> GetMealsByUserIdAsync(int userId)
        {
            return await _mealRepository.GetMealsByUserIdAsync(userId);
        }


        public async Task<bool> AddMealAsync(Meal meal)
        {
            await _mealRepository.AddMealAsync(meal);
            return true;
        }

        public async Task<bool> UpdateMealAsync(Meal meal)
        {
            await _mealRepository.UpdateMealAsync(meal);
            return true;
        }

        public async Task<bool> DeleteMealAsync(int id)
        {
            await _mealRepository.DeleteMealAsync(id);
            return true;
        }

        // ✅ יצירת ארוחות מותאמות אישית למשתמש
        public async Task<List<Meal>> GenerateMealsForUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            var preferences = await _preferencesRepository.GetPreferencesByUserIdAsync(userId);
            if (preferences == null) return null;

            var filteredProducts = await _FoodItemRepository.GetFilteredProductsByPreferencesAsync(preferences);
            if (filteredProducts == null || !filteredProducts.Any()) return null;

            var mealCaloriesList = await CalculateCaloriesPerMealAsync(user.Id);

            List<Meal> meals = new List<Meal>();
            HashSet<int> usedProductIds = new HashSet<int>();

            // יצירת ארוחות לפי החישוב
            meals.Add(CreateMeal("Breakfast", filteredProducts, mealCaloriesList[0], userId, usedProductIds));
            meals.Add(CreateMeal("Lunch", filteredProducts, mealCaloriesList[1], userId, usedProductIds));
            meals.Add(CreateMeal("Dinner", filteredProducts, mealCaloriesList[2], userId, usedProductIds));

            // שמירה במסד הנתונים
            foreach (var meal in meals)
            {
                await _mealRepository.AddMealAsync(meal);
            }

            return meals;
        }

        // ✅ חישוב קלוריות לכל ארוחה
        public async Task<List<double>> CalculateCaloriesPerMealAsync(int userId)
        {
            // שליפת המשתמש מהמאגר
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            // חישוב הקלוריות היומיות באמצעות TDEE
            double totalCalories = await _userRepository.CalculateTDEE(user);
            int mealCount = 3; // ברירת מחדל: 3 ארוחות ביום

            List<double> mealCalories = new List<double>();

            // יחסים לחלוקת קלוריות
            double breakfastRatio = 0.3;
            double lunchRatio = 0.4;
            double dinnerRatio = 0.2;
            double snackRatio = 0.1;

            if (mealCount <= 3)
            {
                mealCalories.Add(totalCalories * breakfastRatio);
                mealCalories.Add(totalCalories * lunchRatio);
                mealCalories.Add(totalCalories * dinnerRatio);
            }
            else
            {
                double snackCalories = totalCalories * snackRatio / (mealCount - 3);
                mealCalories.Add(totalCalories * breakfastRatio);
                mealCalories.Add(totalCalories * lunchRatio);
                mealCalories.Add(totalCalories * dinnerRatio);

                for (int i = 0; i < mealCount - 3; i++)
                {
                    mealCalories.Add(snackCalories);
                }
            }

            return mealCalories;
        }

        // ✅ יצירת ארוחה על פי סוג הארוחה
        private Meal CreateMeal(string mealType, List<FoodItem> filteredProducts, double targetCalories, int userId, HashSet<int> usedProductIds)
        {
            var meal = new Meal
            {
                MealType = Enum.Parse<MealType>(mealType, true),
                Calories = 0, // נתחיל עם 0 ונצבור קלוריות בהמשך
                UserId = userId
            };

            List<FoodItem> selectedProducts = new List<FoodItem>();

            List<Categorydish> allowedCategories = DefineAllowedCategories(mealType);

            foreach (var category in allowedCategories)
            {
                var availableProducts = filteredProducts
                    .Where(p => p.Category == category && !usedProductIds.Contains(p.Id) && p.Calories <= targetCalories)
                    .ToList();

                if (!availableProducts.Any()) continue;

                var randomIndex = new Random().Next(availableProducts.Count);
                var selectedProduct = availableProducts[randomIndex];

                selectedProducts.Add(selectedProduct);
                usedProductIds.Add(selectedProduct.Id);
                meal.Calories += selectedProduct.Calories;

                // עדכון היעד הקלורי כדי להימנע מהוספת יותר מדי קלוריות
                targetCalories -= selectedProduct.Calories;

                // אם הגענו ליעד הקלורי, נפסיק להוסיף מוצרים
                if (meal.Calories >= targetCalories) break;
            }

            // ✅ שמירת קשר בטבלת רבים לרבים
            meal.MealFoodItems = selectedProducts
                .Select(product => new MealFoodItem { FoodItemId = product.Id, Meal = meal })
                .ToList();

            return meal;
        }


        // ✅ הגדרת קטגוריות לכל ארוחה
        private List<Categorydish> DefineAllowedCategories(string mealType)
        {
            switch (mealType.ToUpper())
            {
                case "BREAKFAST": return new List<Categorydish> { Categorydish.Dairy, Categorydish.BreakfastItems, Categorydish.Salad };
                case "LUNCH": return new List<Categorydish> { Categorydish.Meat, Categorydish.SideDish, Categorydish.Fish };
                case "DINNER": return new List<Categorydish> { Categorydish.Dish, Categorydish.Salad, Categorydish.Dairy };
                case "SNACK": return new List<Categorydish> { Categorydish.Snacks, Categorydish.Desserts };
                default: return new List<Categorydish>();
            }
        }
        // פונקציה ב-Service לשאילת המוצרים עבור ארוחה על פי ה-ID שלה
     
        public async Task<IEnumerable<FoodItem>> GetProductsForMealAsync(int mealId)
        {
            // קריאה לפונקציה ב-Repository כדי לקבל את המוצרים של הארוחה
            return await _mealRepository.GetProductsForMealAsync(mealId);
        }


        // פונקציה שמקבלת ID של ארוחה ומחזירה את כל המוצרים של אותה ארוחה
        public async Task<List<FoodItem>> GetFoodItemsForMealAsync(int mealId)
        {
            try
            {
                // 1. שלוף את כל רשומות ה-RR (רבים לרבים) עבור הארוחה
                var mealFoodItems = await _mealFoodItemRepository.GetFoodItemsForMeal(mealId);

                // 2. שלוף את המוצרים מה-ID של כל ארוחה
                var foodItems = new List<FoodItem>();
                foreach (var mealFoodItem in mealFoodItems)
                {
                    var foodItem = await _FoodItemRepository.GetFoodItemByIdAsync(mealFoodItem.FoodItemId);
                    foodItems.Add(foodItem);
                }

                return foodItems; // מחזיר את רשימת המוצרים
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving food items for the meal", ex);
            }
        }

    }
}

