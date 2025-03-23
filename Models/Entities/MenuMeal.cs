using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities
{
    public class MenuMeal
    {
        [Key]
        [Column(Order = 1)]

        public int MenuId { get; set; }
        [JsonIgnore]
        public Menu Menu { get; set; } 

        [Key]
        [Column(Order = 2)]
        public int MealId { get; set; }
        [JsonIgnore]
        public Meal Meal { get; set; } 
    }
}
