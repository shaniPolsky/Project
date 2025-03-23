using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Models.Enums.Enums;

namespace Models.Entities
{
    public class Preferences
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // 🔹 קשר למשתמש
        // 🔹 קשר לטבלת העדפות (Preferences) 

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public virtual User ?User { get; set; } // 🔹 קשר ישיר למשתמש

        // 🔹 סוגי תזונה (Enum במקום Boolean)
        public DietType Diet { get; set; } = DietType.None;

        // 🔹 מטרת המשתמש (Enum)
        public UserGoal Goal { get; set; } = UserGoal.MaintainWeight;

        // 🔹 רמות סטרס (Enum)
        public StressLevel Stress { get; set; } = StressLevel.Medium;

        // 🔹 מגבלות רפואיות (Enum)
        public MedicalCondition MedicalCondition { get; set; } = MedicalCondition.None;

        // 🔹 רמות פעילות גופנית (Enum)
        public ActivityLevel Activity { get; set; } = ActivityLevel.Sedentary;

        // 🔹 מספר ארוחות ביום (Enum)
        public MealFrequency MealsPerDay { get; set; } = MealFrequency.ThreeMeals;

        // 🔹 שעות שינה (Enum)
        public SleepHours Sleep { get; set; } = SleepHours.SixToEight;

        // 🔹 כמות מים ביום (Enum)
        public WaterIntake Water { get; set; } = WaterIntake.Medium;

        // 🔹 רמת רגישות כללית (Enum)
        public SensitivityLevel Sensitivity { get; set; } = SensitivityLevel.None;

        // 🔹 מין (Enum)
        public Gender Gender { get; set; } = Gender.Male;

        // 🔹 רמות רגישות לסוגי מזון ספציפיים (Enums חדשים שהוספת)
        public SensitivityLevel IsSensitiveToGluten { get; set; } = SensitivityLevel.None;
        public SensitivityLevel IsSensitiveToDairy { get; set; } = SensitivityLevel.None;
        public SensitivityLevel IsSensitiveToNuts { get; set; } = SensitivityLevel.None;
        public SensitivityLevel IsSensitiveToPeanuts { get; set; } = SensitivityLevel.None;
        public SensitivityLevel IsSensitiveToSoy { get; set; } = SensitivityLevel.None;
        public SensitivityLevel IsSensitiveToEggs { get; set; } = SensitivityLevel.None;
        public SensitivityLevel IsSensitiveToSesame { get; set; } = SensitivityLevel.None;
        public SensitivityLevel IsSensitiveToFish { get; set; } = SensitivityLevel.None;
    }
}
