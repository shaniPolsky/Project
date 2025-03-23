using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class MealFoodItem
    {
        [Key]
        [Column(Order = 1)]
        public int MealId { get; set; }

        [ForeignKey("MealId")]
        public virtual Meal ?Meal { get; set; }

        [Key]
        [Column(Order = 2)]
        public int FoodItemId { get; set; }

        [ForeignKey("FoodItemId")]
        public virtual FoodItem ?FoodItem { get; set; }
    }
}
