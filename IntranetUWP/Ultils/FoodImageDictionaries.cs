using System.Collections.Generic;

namespace IntranetUWP.Ultils
{
    public class FoodImageDictionaries
    {
        public static Dictionary<int, string> getPrimaryFood()
        {
            var primaryFood = new Dictionary<int, string>()
                {
                    { 1, "ms-appx:///Assets/FoodAssets/Rice.png"},
                    { 2, "ms-appx:///Assets/FoodAssets/Bread.png"},
                    { 3, "ms-appx:///Assets/FoodAssets/Spagheti.png"},
                    { 4, "ms-appx:///Assets/FoodAssets/Noodle.png"},
                    { 5, "ms-appx:///Assets/FoodAssets/LunchFood.png"}
                };
            return primaryFood;
        }

        public static Dictionary<int, string> getSecondaryFood()
        {
            var secondaryFood = new Dictionary<int, string>()
                {
                    { 6, "ms-appx:///Assets/FoodAssets/Meat.png"},
                    { 7, "ms-appx:///Assets/FoodAssets/Chicken.png"},
                    { 8, "ms-appx:///Assets/FoodAssets/Egg.png"},
                    { 9, "ms-appx:///Assets/FoodAssets/Shrimp.png"},
                    { 10, "ms-appx:///Assets/FoodAssets/Falafel.png"},
                    { 11, null }
                };
            return secondaryFood;
        }
    }
}
