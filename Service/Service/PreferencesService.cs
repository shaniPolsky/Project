using LifeCicle.DAL.Repositories;
using Models.Entities;
using Service.Interfaces;
using System.Threading.Tasks;
using Models.Enums;
using static Models.Enums.Enums;
namespace Service.Services
{
    public class PreferencesService : IPreferencesService
    {
        private readonly IPreferencesRepository _preferencesRepository;

        public PreferencesService(IPreferencesRepository preferencesRepository)
        {
            _preferencesRepository = preferencesRepository;
        }

        public async Task<Preferences?> GetPreferencesByUserIdAsync(int userId)
        {
            return await _preferencesRepository.GetPreferencesByUserIdAsync(userId);
        }

        public async Task<bool> UpdatePreferencesAsync(int userId, Preferences updatedPreferences)
        {
            var existingPreferences = await _preferencesRepository.GetPreferencesByUserIdAsync(userId);
            if (existingPreferences == null)
                return false; // אם לא נמצאו העדפות, החזרת שגיאה

            // 🔹 עדכון כל הערכים
            existingPreferences.Diet = updatedPreferences.Diet;
            existingPreferences.Goal = updatedPreferences.Goal;
            existingPreferences.Stress = updatedPreferences.Stress;
            existingPreferences.MedicalCondition = updatedPreferences.MedicalCondition;
            existingPreferences.Activity = updatedPreferences.Activity;
            existingPreferences.MealsPerDay = updatedPreferences.MealsPerDay;
            existingPreferences.Sleep = updatedPreferences.Sleep;
            existingPreferences.Water = updatedPreferences.Water;
            existingPreferences.Sensitivity = updatedPreferences.Sensitivity;
            existingPreferences.Gender = updatedPreferences.Gender;

            // 🔹 עדכון רמות רגישות לאלרגנים
            existingPreferences.IsSensitiveToGluten = updatedPreferences.IsSensitiveToGluten;
            existingPreferences.IsSensitiveToDairy = updatedPreferences.IsSensitiveToDairy;
            existingPreferences.IsSensitiveToNuts = updatedPreferences.IsSensitiveToNuts;
            existingPreferences.IsSensitiveToPeanuts = updatedPreferences.IsSensitiveToPeanuts;
            existingPreferences.IsSensitiveToSoy = updatedPreferences.IsSensitiveToSoy;
            existingPreferences.IsSensitiveToEggs = updatedPreferences.IsSensitiveToEggs;
            existingPreferences.IsSensitiveToSesame = updatedPreferences.IsSensitiveToSesame;
            existingPreferences.IsSensitiveToFish = updatedPreferences.IsSensitiveToFish;

            // 🔹 שמירת השינויים במסד הנתונים
            await _preferencesRepository.UpdatePreferencesAsync(existingPreferences);
            await _preferencesRepository.SaveChangesAsync();

            return true; // החזרת הצלחה
        }






        public async Task<List<FoodItem>> FilterProductsByUserAsync(int userId, List<FoodItem> products)
        {
            var preferences = await GetPreferencesByUserIdAsync(userId); // הוספת await
            if (preferences == null) return new List<FoodItem>();

            return products.Where(p =>
                FilterBySensitivity(preferences, p) &&
                FilterByDiet(preferences, p)
            ).ToList();
        }

        private bool FilterBySensitivity(Preferences preferences, FoodItem product)
        {
            return CheckSensitivity(preferences.Sensitivity, product.IsGlutenFree, product.Protein) &&
                   CheckSensitivity(preferences.Sensitivity, product.IsDairyFree, product.Protein) &&
                   CheckSensitivity(preferences.Sensitivity, product.IsNutFree, product.Protein);
        }

        private bool CheckSensitivity(SensitivityLevel sensitivity, bool isProductFree, double productProtein)
        {
            if (sensitivity == SensitivityLevel.None) return true;

            if (sensitivity == SensitivityLevel.Mild && productProtein < 10) return true;

            return isProductFree;
        }

        private bool FilterByDiet(Preferences preferences, FoodItem product)
        {
            return preferences.Diet == DietType.None || product.Category == (Categorydish)preferences.Diet;
        }

        public async Task<List<FoodItem>> FilterProductsAsync(int userId)
        {
            var preferences = await _preferencesRepository.GetPreferencesByUserIdAsync(userId);
            if (preferences == null)
                return new List<FoodItem>(); // מחזיר רשימה ריקה אם אין העדפות

            return await _preferencesRepository.GetFilteredProductsByPreferencesAsync(preferences);
        }

    }
}
