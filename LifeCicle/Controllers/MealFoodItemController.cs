using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Service.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealFoodItemController : ControllerBase
    {
        private readonly MealFoodItemService _mealFoodItemService;

        public MealFoodItemController(MealFoodItemService mealFoodItemService)
        {
            _mealFoodItemService = mealFoodItemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MealFoodItem>>> GetAllMealFoodItems()
        {
            return Ok(await _mealFoodItemService.GetAllMealFoodItemsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddMealFoodItem(MealFoodItem mealFoodItem)
        {
            await _mealFoodItemService.AddMealFoodItemAsync(mealFoodItem);
            return CreatedAtAction(nameof(GetAllMealFoodItems),
                new { mealId = mealFoodItem.MealId, foodItemId = mealFoodItem.FoodItemId },
                mealFoodItem);
        }

    }
}
