using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LifeCicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // כל הפעולות דורשות התחברות
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        // ✅ שליפת כל הארוחות
        [HttpGet]
        public async Task<IActionResult> GetAllMeals()
        {
            var meals = await _mealService.GetAllMealsAsync();
            return Ok(meals);
        }

        // ✅ שליפת ארוחה לפי מזהה
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMealById(int id)
        {
            var meal = await _mealService.GetMealByIdAsync(id);
            if (meal == null)
                return NotFound("Meal not found.");

            return Ok(meal);
        }

        // ✅ שליפת כל הארוחות של משתמש לפי מזהה משתמש
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMealsByUserId(int userId)
        {
            var meals = await _mealService.GetMealsByUserIdAsync(userId);
            return Ok(meals);
        }

        // ✅ הוספת ארוחה חדשה
        [HttpPost]
        public async Task<IActionResult> AddMeal([FromBody] Meal meal)
        {
            if (meal == null)
                return BadRequest("Invalid meal data.");

            await _mealService.AddMealAsync(meal);
            return CreatedAtAction(nameof(GetMealById), new { id = meal.Id }, meal);
        }

        // ✅ עדכון ארוחה קיימת לפי מזהה
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeal(int userId, [FromBody] Meal updatedMeal)
        {
            if (updatedMeal == null)
                return BadRequest("Invalid meal data.");

            var success = await _mealService.UpdateMealAsync(updatedMeal);
            if (!success)
                return NotFound("Meal not found.");

            return NoContent();
        }

        // ✅ מחיקת ארוחה לפי מזהה
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            var success = await _mealService.DeleteMealAsync(id);
            if (!success)
                return NotFound("Meal not found.");

            return NoContent();
        }

        [HttpPost("defineMeals/{id}")]
        public async Task<IActionResult> DefineMealsForUser(int id)
        {
            var meals = await _mealService.GenerateMealsForUserAsync(id);
            if (meals == null || !meals.Any()) return NotFound("Failed to generate meals.");
            return Ok(meals);
        }

        // ✅ חישוב קלוריות לכל ארוחה
        [HttpPost("calculateCaloriesPerMeal/{userId}")]
        public async Task<IActionResult> CalculateCaloriesPerMeal(int userId)
        {
            var mealCalories = await _mealService.CalculateCaloriesPerMealAsync(userId);
            if (mealCalories == null) return NotFound("No meals found for the user.");
            return Ok(mealCalories);
        }

        [HttpGet("{mealId}/products")]
        public async Task<IActionResult> GetMealProducts(int mealId)
        {
            try
            {
                var products = await _mealService.GetProductsForMealAsync(mealId);
                if (products == null || !products.Any())
                {
                    return NotFound("No products found for this meal.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // פונקציה לשליפת המוצרים עבור ארוחה
        [HttpGet("GetFoodItemsForMeal/{mealId}")]
        public async Task<IActionResult> GetFoodItemsForMeal(int mealId)
        {
            try
            {
                var foodItems = await _mealService.GetFoodItemsForMealAsync(mealId);
                return Ok(foodItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}
