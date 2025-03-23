using Models.Entities;
using DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Interfaces;

namespace Service.Services
{
    public class MealFoodItemService : IMealFoodItemService
    {
        private readonly IMealFoodItemRepository _mealFoodItemRepository;

        public MealFoodItemService(IMealFoodItemRepository mealFoodItemRepository)
        {
            _mealFoodItemRepository = mealFoodItemRepository;
        }

        public async Task<List<MealFoodItem>> GetAllMealFoodItemsAsync()
        {
            return await _mealFoodItemRepository.GetAllMealFoodItemsAsync();
        }

        public async Task<List<MealFoodItem>> GetMealFoodItemsByMealIdAsync(int mealId)
        {
            return await _mealFoodItemRepository.GetMealFoodItemsByMealIdAsync(mealId);
        }

        public async Task<List<MealFoodItem>> GetMealFoodItemsByFoodItemIdAsync(int foodItemId)
        {
            return await _mealFoodItemRepository.GetMealFoodItemsByFoodItemIdAsync(foodItemId);
        }

        public async Task AddMealFoodItemAsync(MealFoodItem mealFoodItem)
        {
            await _mealFoodItemRepository.AddMealFoodItemAsync(mealFoodItem);
        }

        public async Task DeleteMealFoodItemAsync(int id)
        {
            await _mealFoodItemRepository.DeleteMealFoodItemAsync(id);
        }

        public Task<MealFoodItem?> GetMealFoodItemByIdAsync(int mealId, int foodItemId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMealFoodItemAsync(int mealId, int foodItemId)
        {
            throw new NotImplementedException();
        }

       

    }
}
