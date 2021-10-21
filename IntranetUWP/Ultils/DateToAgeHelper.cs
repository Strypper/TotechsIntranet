using System;
namespace IntranetUWP.Ultils
{
    public static class DateToAgeHelper
    {
        public static Int32 GetAge(this DateTime? dateOfBirth)
        {
            var today = DateTime.UtcNow;
            var dateOfBirthValue = dateOfBirth ?? today;
            var age = today.Year - dateOfBirthValue.Year;
            return age;
        }
    }
}
