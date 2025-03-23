using LifeCicle.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using static Models.Enums.Enums;

namespace LifeCicle.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync(); // ⬅ שמירה למסד נתונים
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<double> CalculateTDEE(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // חישוב BMR לפי מין
            double bmr = (10 * user.UserWeight) + (6.25 * user.UserHeight) - (5 * user.Age);
            if (user.Preferences?.Gender == Gender.Male)
                bmr += 5;
            else
                bmr -= 161;

            // קביעת מקדם פעילות
            double activityMultiplier = user.Preferences?.Activity switch
            {
                ActivityLevel.Sedentary => 1.2,
                ActivityLevel.LightlyActive => 1.375,
                ActivityLevel.ModeratelyActive => 1.55,
                ActivityLevel.VeryActive => 1.725,
                ActivityLevel.SuperActive => 1.9,
                _ => 1.2 // ברירת מחדל למכניעת שגיאות
            };

            return await Task.FromResult(bmr * activityMultiplier);
        }


    }
}
