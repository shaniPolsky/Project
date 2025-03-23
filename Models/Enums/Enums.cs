using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enums
{
    public class Enums
    {
        // 🔹 סוגי תזונה (כפי שהיה ב-Java)
        public enum DietType
        {
            None = 0,
            Vegetarian = 1,
            Vegan = 2,
            GlutenFree = 3,
            LactoseFree = 4,
            NutAllergy = 5,
            Keto = 6,
            Paleo = 7
        }

        // 🔹 מטרות המשתמש (כפי שהיה ב-Java)
        public enum UserGoal
        {
            MaintainWeight = 0,
            LoseWeight = 1,
            GainMuscle = 2,
            ImproveEndurance = 3,
            ReduceBodyFat = 4
        }

        public enum StressLevel
        {
            Low = 1,
            Medium = 2,
            High = 3,
            Extreme = 4
        }

        public enum MedicalCondition
        {
            None = 0,
            Diabetes = 1,
            HighBloodPressure = 2,
            HeartDisease = 3,
            ThyroidIssues = 4
        }

        // 🔹 רמות פעילות גופנית (מה שהיה ב-Java)
        public enum ActivityLevel
        {
            Sedentary = 0,      // לא פעיל
            LightlyActive = 1,  // פעילות קלה (1-2 ימים בשבוע)
            ModeratelyActive = 2, // פעילות בינונית (3-4 ימים בשבוע)
            VeryActive = 3,     // פעילות גבוהה (5-6 ימים בשבוע)
            SuperActive = 4     // אתלט מקצועי
        }

        // 🔹 מספר הארוחות ביום (מה שהיה ב-Java)
        public enum MealFrequency
        {
            TwoMeals = 2,
            ThreeMeals = 3,
            FourMeals = 4,
            FiveMeals = 5,
            SixMeals = 6
        }

        // 🔹 כמות מים ביום (כוסות)
        public enum WaterIntake
        {
            Low = 4,   // מעט מאוד
            Medium = 8, // ממוצע
            High = 12  // הרבה מים
        }

        // 🔹 שעות שינה (ממוצע שעות בלילה)
        public enum SleepHours
        {
            LessThan4 = 1,
            FourToSix = 2,
            SixToEight = 3,
            MoreThanEight = 4
        }

        // 🔹 רמות רגישות (למשל לאלרגנים)
        public enum SensitivityLevel
        {
            None = 0,
            Mild = 1,
            Moderate = 2,
            Severe = 3
        }

        // 🔹 מין (Gender)
        public enum Gender
        {
            Male = 0,
            Female = 1,
            Other = 2
        }

        public enum Categorydish
        {
            Dish =0,
            SideDish =1 , // עם underscore
            Dairy = 2,
            Meat = 3,
            Fish = 4,
            Desserts = 5,
            BreakfastItems = 6,
            Snacks = 7,
            AdditionOfVegetables = 8,
            Salad = 9
        }

        public enum MealType
        {
            Breakfast = 1, // ארוחת בוקר
            Lunch = 2,     // ארוחת צהריים
            Dinner = 3,    // ארוחת ערב
            Snack = 4      // חטיף
        }

    }
}