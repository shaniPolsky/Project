using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // מזהה התפריט

        [Required]
        public string MenuName { get; set; } = string.Empty;  // שם התפריט

        public int Calories { get; set; }  // כמות קלוריות כוללת בתפריט

        public string Description { get; set; } = string.Empty;  // תיאור התפריט

        public string MenuStatus { get; set; } = string.Empty;  // סטטוס התפריט

        public int MenuDuration { get; set; }  // תקופת זמן של התפריט בימים

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // תאריך יצירת התפריט

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // תאריך עדכון התפריט

       
        // קשר למשתמש
        public int UserId { get; set; } // בדקי שהטיפוס תואם ל-User.Id
        [ForeignKey("UserId")]
        public virtual User ?User { get; set; } 


        // קשר לרשימת הארוחות בתפריט (Many-to-Many)
        public  List<MenuMeal> MenuMeals { get; set; } = new List<MenuMeal>();
    }
}
