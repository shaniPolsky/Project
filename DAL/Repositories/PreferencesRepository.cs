using LifeCicle.DAL.Data;
using Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LifeCicle.DAL.Repositories
{
    public class PreferencesRepository : IPreferencesRepository
    {
        private readonly ApplicationDbContext _context;

        public PreferencesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Preferences?> GetPreferencesByUserIdAsync(int userId)
        {
            return await _context.Preferences.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddPreferencesAsync(Preferences preferences)
        {
            await _context.Preferences.AddAsync(preferences);
            await _context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync(); // ⬅ שמירה למסד נתונים
        }

        public async Task UpdatePreferencesAsync(Preferences preferences)
        {
            _context.Preferences.Update(preferences);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePreferencesByUserIdAsync(int userId)
        {
            var preferences = await _context.Preferences.FirstOrDefaultAsync(p => p.UserId == userId);
            if (preferences != null)
            {
                _context.Preferences.Remove(preferences);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<List<FoodItem>> GetFilteredProductsByPreferencesAsync(Preferences preferences)
        {
            return await _context.FoodItems.ToListAsync();
        }

    }
}
