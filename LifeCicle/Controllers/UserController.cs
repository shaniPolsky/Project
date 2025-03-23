using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Service.Interfaces;

namespace LifeCicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize] // כל הפעולות כאן דורשות התחברות
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ✅ קבלת כל המשתמשים
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        // ✅ קבלת משתמש לפי ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // ✅ יצירת משתמש חדש (הרשמה)
        
   
        [HttpPost("Register")]
        // [AllowAnonymous] // לא מחייב JWT
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            if (user == null) return BadRequest("Invalid user data");

            var success = await _userService.RegisterUserAsync(user);
            if (!success) return BadRequest("Failed to register user");

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // ✅ עדכון משתמש קיים
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null || user.Id != id)
                return BadRequest("Invalid user data");

            var success = await _userService.UpdateUserAsync(id, user);
            if (!success) return NotFound();

            return NoContent();
        }

        // ✅ מחיקת משתמש
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }

        // ✅ חישוב TDEE (קלוריות יומיות מותאמות אישית)
        [HttpGet("calculateTDEE/{id}")]
        public async Task<IActionResult> CalculateTDEE(int id)
        {
            // שליפת המשתמש לפי ה-ID
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found."); // אם המשתמש לא נמצא

            // חישוב TDEE עם אובייקט המשתמש
            var tdee = _userService.CalculateTDEE(user);
            return Ok(tdee);
        }


        // ✅ חישוב קלוריות לכל ארוחה
        [HttpGet("calculateMealCalories/{id}")]
    public async Task<IActionResult> CalculateMealCalories(int id)
    {
        var mealCalories = await _userService.CalculateMealCaloriesAsync(id);
            if (mealCalories <= 0)
                return NotFound("User not found.");
            return Ok(mealCalories);
    }
    }
}
