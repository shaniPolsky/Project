using LifeCicle.DAL.Repositories.Interfaces;
using Models.Entities;
using Service.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Models.Enums.Enums;

namespace Service.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _foodItemRepository;

        public FoodItemService(IFoodItemRepository foodItemRepository)
        {
            _foodItemRepository = foodItemRepository;
        }

        public async Task<List<FoodItem>> GetAllFoodItemsAsync()
        {
            return await _foodItemRepository.GetAllFoodItemsAsync();
        }

        public async Task<FoodItem?> GetFoodItemByIdAsync(int id)
        {
            return await _foodItemRepository.GetFoodItemByIdAsync(id);
        }

        public async Task AddFoodItemAsync(FoodItem foodItem)
        {
            await _foodItemRepository.AddFoodItemAsync(foodItem);
        }

        public async Task UpdateFoodItemAsync(FoodItem foodItem)
        {
            await _foodItemRepository.UpdateFoodItemAsync(foodItem);
        }

        public async Task DeleteFoodItemAsync(int id)
        {
            await _foodItemRepository.DeleteFoodItemAsync(id);
        }

    
        

    }
}
