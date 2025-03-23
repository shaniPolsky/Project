using LifeCicle.DAL.Repositories;
using Models.Entities;
using Service.Interfaces;
using System.Threading.Tasks;
using static Models.Enums.Enums;
using Models.Enums;


namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPreferencesRepository _preferencesRepository; // הוספנו את ההעדפות

        public UserService(IUserRepository userRepository, IPreferencesRepository preferencesRepository)
        {
            _userRepository = userRepository;
            _preferencesRepository = preferencesRepository;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            Console.WriteLine("העניחמלחכסגבעיולבעכנמחךלןארגבהניחוארבעינחעאי");
            return await _userRepository.GetUsersAsync();

        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<bool> RegisterUserAsync(User user)
        {
            if (user == null) return false;

            // 🔹 שלב 1: שמירת המשתמש תחילה כדי לקבל את ה-ID
            await _userRepository.AddUserAsync(user);
            await _userRepository.SaveChangesAsync(); // שמירה כדי לקבל את ה-ID

            // 🔹 שלב 2: אם אין העדפות, ניצור ברירת מחדל ונקשר ל-ID של המשתמש
            if (user.Preferences == null)
            {
                user.Preferences = new Preferences
                {
                    UserId = user.Id,  // קישור ה-ID שנשמר עכשיו
                    Diet = DietType.None,
                    Goal = UserGoal.MaintainWeight,
                    Stress = StressLevel.Medium,
                    MedicalCondition = MedicalCondition.None,
                    Activity = ActivityLevel.Sedentary,
                    MealsPerDay = MealFrequency.ThreeMeals,
                    Sleep = SleepHours.SixToEight,
                    Water = WaterIntake.Medium,
                    Sensitivity = SensitivityLevel.None,
                    Gender = Gender.Male,

                    // 🔹 רמות רגישות לסוגי מזון ספציפיים (Enums)
                    IsSensitiveToGluten = SensitivityLevel.None,
                    IsSensitiveToDairy = SensitivityLevel.None,
                    IsSensitiveToNuts = SensitivityLevel.None,
                    IsSensitiveToPeanuts = SensitivityLevel.None,
                    IsSensitiveToSoy = SensitivityLevel.None,
                    IsSensitiveToEggs = SensitivityLevel.None,
                    IsSensitiveToSesame = SensitivityLevel.None,
                    IsSensitiveToFish = SensitivityLevel.None
                };
            }

            // 🔹 שלב 3: שמירת ההעדפות אחרי שה-ID של המשתמש כבר קיים
            await _preferencesRepository.AddPreferencesAsync(user.Preferences);
            await _preferencesRepository.SaveChangesAsync();

            return true;
        }






        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            if (user == null || user.Id != id)
                return false;

            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return false;

            // ✅ עדכון פרטי המשתמש
            existingUser.Email = user.Email;
            existingUser.UserPassword = user.UserPassword;
            existingUser.UserName = user.UserName;
            existingUser.Age = user.Age;
            existingUser.UserHeight = user.UserHeight;
            existingUser.UserWeight = user.UserWeight;
            existingUser.TargetWeight = user.TargetWeight;

            await _userRepository.UpdateUserAsync(existingUser);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                return false;

            // ✅ מחיקת ההעדפות של המשתמש לפני המחיקה שלו
            await _preferencesRepository.DeletePreferencesByUserIdAsync(id);

            // ✅ מחיקת המשתמש
            await _userRepository.DeleteUserAsync(id);
            return true;
        }

        public async Task<double> CalculateTDEE(User user)
        {
            if (user == null)
                throw new ArgumentException("User cannot be null");

            double bmr = (user.Preferences?.Gender == Gender.Male)
                ? 88.362 + (13.397 * user.UserWeight) + (4.799 * user.UserHeight) - (5.677 * user.Age)
                : 447.593 + (9.247 * user.UserWeight) + (3.098 * user.UserHeight) - (4.330 * user.Age);

            double activityMultiplier = user.Preferences?.Activity switch
            {
                ActivityLevel.SuperActive => 1.725,
                ActivityLevel.ModeratelyActive => 1.55,
                ActivityLevel.LightlyActive => 1.2,
                _ => 1.2
            };

            double tdee = bmr * activityMultiplier;

            if (user.Preferences != null)
            {
                if (user.Preferences.Sleep == SleepHours.FourToSix || user.Preferences.Sleep == SleepHours.LessThan4)
                    tdee *= 0.9;

                if (user.Preferences.Stress == StressLevel.High)
                    tdee *= 0.9;

                if (user.Preferences.Stress == StressLevel.Medium)
                    tdee *= 0.95;

                if (user.Preferences.Water == WaterIntake.Low)
                    tdee *= 0.92;

                if (user.Preferences.Water == WaterIntake.Medium)
                    tdee *= 0.95;

                if (user.Preferences.MedicalCondition != MedicalCondition.None)
                    tdee *= 0.9;
            }

            return tdee;  // מחזיר רק את ה-TDEE
        }

        public List<double> CalculateCaloriesPerMeal(double totalCalories, int mealCount)
        {
            List<double> caloriesPerMeal = new List<double>();

            double breakfastRatio = 0.3;
            double lunchRatio = 0.4;
            double dinnerRatio = 0.2;
            double snackRatio = 0.1;

            if (mealCount <= 3)
            {
                caloriesPerMeal.Add(totalCalories * breakfastRatio);
                caloriesPerMeal.Add(totalCalories * lunchRatio);
                caloriesPerMeal.Add(totalCalories * dinnerRatio);
            }
            else
            {
                double snackCalories = (totalCalories * snackRatio) / (mealCount - 3);
                caloriesPerMeal.Add(totalCalories * breakfastRatio);
                caloriesPerMeal.Add(totalCalories * lunchRatio);
                caloriesPerMeal.Add(totalCalories * dinnerRatio);
                for (int i = 0; i < mealCount - 3; i++)
                {
                    caloriesPerMeal.Add(snackCalories);
                }
            }

            return caloriesPerMeal;
        }

        public async Task<double> CalculateMealCaloriesAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return 0;

            double totalCalories = await CalculateTDEE(user);
            return totalCalories / 3; // חלוקת הקלוריות ל-3 ארוחות לדוגמה
        }

    }


}
