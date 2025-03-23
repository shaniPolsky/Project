using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Reflection.Emit;

namespace LifeCicle.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Preferences> Preferences { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Menu> Menus { get; set; }



        public DbSet<MenuMeal> MenuMeal { get; set; } // הוספת טבלת ביניים
        public DbSet<MealFoodItem> MealFoodItems { get; set; } // ✅ הוספנו כאן את הטבלה החדשה של הקשרים


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 קשר של 1 ל-1 בין משתמש להעדפות (User -> Preferences)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Preferences)
                .WithOne(p => p.User)
                .HasForeignKey<Preferences>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // אם מוחקים משתמש, נמחקות גם ההעדפות שלו

            modelBuilder.Entity<MenuMeal>()
                .HasKey(mm => new { mm.MenuId, mm.MealId });

            modelBuilder.Entity<MenuMeal>()
                .HasOne(mm => mm.Menu)
                .WithMany(m => m.MenuMeals)
                .HasForeignKey(mm => mm.MenuId);

            modelBuilder.Entity<MenuMeal>()
                .HasOne(mm => mm.Meal)
                .WithMany(m => m.MenuMeals)
                .HasForeignKey(mm => mm.MealId);

            // הגדרת מפתח ראשי מורכב
            modelBuilder.Entity<MealFoodItem>()
                .HasKey(mf => new { mf.MealId, mf.FoodItemId });


        }
    }
}
