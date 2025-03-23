using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeCicle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]  // כל הפעולות דורשות התחברות
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenus()
        {
            var menus = await _menuService.GetAllMenusAsync();
            return Ok(menus);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuById(int id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
                return NotFound("Menu not found.");
            return Ok(menu);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMenusByUserId(int userId)
        {
            var menus = await _menuService.GetMenusByUserIdAsync(userId);
            return Ok(menus);
        }

        [HttpPost]
        public async Task<IActionResult> AddMenu([FromBody] Menu menu)
        {
            await _menuService.AddMenuAsync(menu);
            return CreatedAtAction(nameof(GetMenuById), new { id = menu.Id }, menu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] Menu updatedMenu)
        {
            if (!await _menuService.UpdateMenuAsync(id, updatedMenu))
                return NotFound("Menu not found.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            if (!await _menuService.DeleteMenuAsync(id))
                return NotFound("Menu not found.");
            return NoContent();
        }

        [HttpPost("AddMenuMeal")]
        public async Task<IActionResult> AddMenuMeal([FromBody] MenuMeal menuMeal)
        {
            await _menuService.AddMenuMealAsync(menuMeal);
            return Ok();
        }

        // ✅ יצירת תפריט מותאם אישית
        [HttpPost("generateMenu1/{userId}")]
        

        public async Task<IActionResult> GenerateMenu(int userId)
        {
            try
            {
                var menu = await _menuService.GenerateMenuAsync(userId);
                return Ok(menu);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
