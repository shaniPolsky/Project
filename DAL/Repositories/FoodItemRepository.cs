using LifeCicle.DAL.Data;
using LifeCicle.DAL.Repositories.Interfaces;
using Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Enums.Enums;

namespace LifeCicle.DAL.Repositories
{
    public class FoodItemRepository : IFoodItemRepository
    {
        private readonly ApplicationDbContext _context;

        public FoodItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FoodItem>> GetAllFoodItemsAsync()
        {
            return await _context.FoodItems.ToListAsync();
        }

        public async Task<FoodItem> GetFoodItemByIdAsync(int id)
        {
            return await _context.FoodItems.FindAsync(id); // שליפה לפי ID
        }

            public async Task AddFoodItemAsync(FoodItem foodItem)
        {
            await _context.FoodItems.AddAsync(foodItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFoodItemAsync(FoodItem foodItem)
        {
            _context.FoodItems.Update(foodItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFoodItemAsync(int id)
        {
            var item = await _context.FoodItems.FindAsync(id);
            if (item != null)
            {
                _context.FoodItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<FoodItem>> GetFilteredProductsByPreferencesAsync(Preferences preferences)
        {
            var foodItems = await _context.FoodItems.ToListAsync(); // מביא את הנתונים מהדאטהבייס

            return foodItems
                .Where(p => FilterBySensitivity(preferences, p)) // מבצע סינון בזיכרון
                .ToList();
        }

        private bool FilterBySensitivity(Preferences preferences, FoodItem product)
        {
            return CheckSensitivity(preferences.IsSensitiveToGluten, product.IsGlutenFree, product.Protein) &&
                   CheckSensitivity(preferences.IsSensitiveToDairy, product.IsDairyFree, product.Protein) &&
                   CheckSensitivity(preferences.IsSensitiveToNuts, product.IsNutFree, product.Protein) &&
                   CheckSensitivity(preferences.IsSensitiveToPeanuts, product.IsPeanutsFree, product.Protein) &&
                   CheckSensitivity(preferences.IsSensitiveToSoy, product.IsSoyFree, product.Protein) &&
                   CheckSensitivity(preferences.IsSensitiveToEggs, product.IsEggFree, product.Protein) &&
                   CheckSensitivity(preferences.IsSensitiveToSesame, product.IsSesameFree, product.Protein) &&
                   CheckSensitivity(preferences.IsSensitiveToFish, product.IsFishFree, product.Protein);
        }

        private bool CheckSensitivity(SensitivityLevel sensitivity, bool? isProductFree, double? productProtein)
        {
            if (sensitivity == SensitivityLevel.None) return true;
            if (sensitivity == SensitivityLevel.Mild && productProtein < 10) return true;
            return isProductFree ?? false;
        }

        // private bool FilterByDietaryPreferences(Preferences preferences, FoodItem product)
        //  {
        //     if (preferences.DietarySuitability == null || preferences.DietarySuitability.Contains(DietarySuitability.None))
        //     {
        //         return true;
        //    }

        //     return product.DietarySuitability.Any(ds => preferences.DietarySuitability.Contains(ds));
        // }
       



    }
}
