using Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> GetAllMenusAsync();
        Task<Menu?> GetMenuByIdAsync(int id);
        Task<List<Menu>> GetMenusByUserIdAsync(int userId);
        Task AddMenuAsync(Menu menu);
        Task<bool> UpdateMenuAsync(int id, Menu updatedMenu);
        Task<bool> DeleteMenuAsync(int id);
        Task AddMenuMealAsync(MenuMeal menuMeal);
        Task<Menu> GenerateMenuAsync(int userId);
       
    }
}
