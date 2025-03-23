using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Service.Interfaces;
using System.Threading.Tasks;

namespace LifeCicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // כל הפעולות כאן דורשות התחברות
    public class PreferencesController : ControllerBase
    {
        private readonly IPreferencesService _preferencesService;

        public PreferencesController(IPreferencesService preferencesService)
        {
            _preferencesService = preferencesService;
        }

        // ✅ שליפת ההעדפות של המשתמש לפי ה-Id שלו
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPreferencesByUserId(int userId)
        {
            var preferences = await _preferencesService.GetPreferencesByUserIdAsync(userId);

            if (preferences == null)
            {
                return Ok(new
                {
                    Message = "User preferences not set. Default values may apply.",
                    Preferences = (object?)null
                });
            }

            return Ok(preferences);
        }


        // ✅ עדכון ההעדפות של המשתמש לפי ה-Id שלו
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdatePreferences(int userId, [FromBody] Preferences updatedPreferences)
        {
            if (updatedPreferences == null)
                return BadRequest(new { message = "Invalid preferences data." });

            var success = await _preferencesService.UpdatePreferencesAsync(userId, updatedPreferences);
            if (!success)
                return NotFound(new { message = "User preferences not found." });

            return Ok(new { message = "Preferences updated successfully." });
        }

        // ✅ סינון מוצרים מותאמים להעדפות המשתמש
        [HttpGet("filterProducts/{userId}")]
        public async Task<IActionResult> FilterProducts(int userId)
        {
            var filteredProducts = await _preferencesService.FilterProductsAsync(userId); // הוספת await

            if (filteredProducts == null || !filteredProducts.Any())
                return NotFound("No matching products found.");

            return Ok(filteredProducts);
        }
    }
}
