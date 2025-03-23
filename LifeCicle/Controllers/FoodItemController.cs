using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Threading.Tasks;
using Models.Entities;
using System.Collections.Generic;

namespace LifeCicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemService _foodItemService;

        public FoodItemController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoodItems()
        {
            var foodItems = await _foodItemService.GetAllFoodItemsAsync();
            return Ok(foodItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodItemById(int id)
        {
            var foodItem = await _foodItemService.GetFoodItemByIdAsync(id);
            if (foodItem == null) return NotFound();
            return Ok(foodItem);
        }

    }
}
