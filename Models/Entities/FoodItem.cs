using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Models.Enums;

namespace Models.Entities
{
    [Table("FoodItem")]
    public class FoodItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // מזהה מוצר

        [Required]
        public string Name { get; set; } = string.Empty; // שם המוצר/תבשיל

        public int Calories { get; set; } // כמות קלוריות

        public double Protein { get; set; } // כמות חלבון

        public double Carbs { get; set; } // כמות פחמימות

        public double Fats { get; set; } // כמות שומן

        public double Sugars { get; set; } // כמות סוכרים

        public int ServingSize { get; set; } // גודל מנה

        public bool IsGlutenFree { get; set; } // האם מכיל גלוטן
        public bool IsDairyFree { get; set; } // האם מכיל מוצרי חלב
        public bool IsNutFree { get; set; } // האם מכיל אגוזים
        public bool IsPeanutsFree { get; set; } // האם מכיל בוטנים
        public bool IsSoyFree { get; set; } // האם מכיל סויה
        public bool IsEggFree { get; set; } // האם מכיל ביצים
        public bool IsSesameFree { get; set; } // האם מכיל שומשום
        public bool IsFishFree { get; set; } // האם מכיל דג

        public bool DiabeticFriendly { get; set; } // התאמה לסוכרתיים (Yes/No)
        public bool IsMeatFree { get; set; } // האם מכיל בשר

        public Enums.Enums.Categorydish Category { get; set; } = Enums.Enums.Categorydish.Dish;    // קטגוריה (תבשיל, ירק, בשרי וכו')

        // התאמה לתזונה (שדה Enum עם רשימה)
        // public List<DietarySuitability> DietarySuitability { get; set; } = new List<DietarySuitability>();

        // הוספת שדה לתמונה
        public string ImagePath { get; set; } = string.Empty; // נתיב לתמונה
        [JsonIgnore]
        public List<MealFoodItem> MealFoodItems { get; set; } = new List<MealFoodItem>();

    }
}
