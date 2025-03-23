using LifeCicle.DAL.Data;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            return await _context.Menus
            .Include(m => m.MenuMeals) // קישור לטבלת החיבורים
            .ThenInclude(mm => mm.Meal) // טעינת הארוחות שמשויכות לתפריט
            .ToListAsync();
        }

        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await _context.Menus
           .Include(m => m.MenuMeals)
           .ThenInclude(mm => mm.Meal)
           .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Menu>> GetMenusByUserIdAsync(int userId)
        {
            return await _context.Menus.Where(m => m.UserId == userId).ToListAsync();
        }

        public async Task AddMenuAsync(Menu menu)
        {
            await _context.Menus.AddAsync(menu);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMenuAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMenuAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync() // ✨ הוספת המתודה החסרה
        {
            await _context.SaveChangesAsync();
        }
    }
}
