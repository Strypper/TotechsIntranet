using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntranetUWP.Models
{
    public class FoodDTO
    {
        public string FoodName { get; set; }
        public string FoodEnglishName { get; set; }
        public int MainIcon { get; set; }
        public int? SecondaryIcon { get; set; }
        public decimal Percentage { get; set; }
    }

    public class DemoFoodData
    {
        public static ObservableCollection<FoodDTO> getData()
        {
            var data = new ObservableCollection<FoodDTO>();
            data.Add(new FoodDTO() { FoodName = "Cơm gà", FoodEnglishName = "Chicken rice" });
            data.Add(new FoodDTO() { FoodName = "Cơm cá", FoodEnglishName = "Fish rice" });
            data.Add(new FoodDTO() { FoodName = "Mỳ ý", FoodEnglishName = "Spagheti" });
            data.Add(new FoodDTO() { FoodName = "Cơm bò kho", FoodEnglishName = "Vietnamese traditional BBQ rice" });
            data.Add(new FoodDTO() { FoodName = "Cơm gà", FoodEnglishName = "Chicken rice" });
            data.Add(new FoodDTO() { FoodName = "Cơm cá", FoodEnglishName = "Fish rice" });
            data.Add(new FoodDTO() { FoodName = "Mỳ ý", FoodEnglishName = "Spagheti" });
            data.Add(new FoodDTO() { FoodName = "Cơm bò kho", FoodEnglishName = "Vietnamese traditional BBQ rice" });
            data.Add(new FoodDTO() { FoodName = "Cơm gà", FoodEnglishName = "Chicken rice" });
            data.Add(new FoodDTO() { FoodName = "Cơm cá", FoodEnglishName = "Fish rice" });
            data.Add(new FoodDTO() { FoodName = "Mỳ ý", FoodEnglishName = "Spagheti" });
            data.Add(new FoodDTO() { FoodName = "Cơm bò kho", FoodEnglishName = "Vietnamese traditional BBQ rice" });
            return data;
        }
    }
}
