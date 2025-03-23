using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models.Entities
{
    [Table("Meals")]
    public class Meal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string MealName { get; set; } = string.Empty; // שם הארוחה

        [Required]
        public string MealTime { get; set; } = string.Empty; // שעת הארוחה

        public int Calories { get; set; } // קלוריות לארוחה

        public string MealRestrictions { get; set; } = string.Empty; // אילוצים נוספים

        [Required]
        public Enums.Enums.MealType MealType { get; set; } // סוג הארוחה (בוקר/צהריים/ערב)
       
        // קשר למשתמש (Many-to-One)
        [Required]
        public int UserId { get; set; } // בדקי שהטיפוס תואם ל-User.Id
        [ForeignKey("UserId")]
        public virtual User ?User { get; set; } 


        // קשר למתכונים (Many-to-Many)
        // public virtual List<Recipe> Recipes { get; set; } = new List<Recipe>();

        // קשר לתפריטים (Many-to-Many)
        public  List<MenuMeal> MenuMeals { get; set; } = new List<MenuMeal>();

        // קשר למוצרי מזון (Many-to-Many)
        [JsonIgnore]
        public List<MealFoodItem> MealFoodItems { get; set; } = new List<MealFoodItem>();

       
    }
}
