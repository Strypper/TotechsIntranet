using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IntranetUWP.Models
{
    public class FoodDTO : BaseDTO, INotifyPropertyChanged
    {
        public string foodName { get; set; }
        public string foodEnglishName { get; set; }
        public int mainIcon { get; set; } = 5;
        public int? secondaryIcon { get; set; } = 11;
        //public double percentage { get; set; }
        public double numberOfSelectedUser { get; set; }

        private double percentage;

        public double Percentage
        {
            get { return percentage; }
            set 
            { 
                percentage = value;
                OnPropertyChanged();
            }
        }


        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class DemoFoodData
    {
        //public static ObservableCollection<FoodDTO> getData()
        //{
        //    var data = new ObservableCollection<FoodDTO>();
        //    data.Add(new FoodDTO() {FoodId = 1 , FoodName = "Cơm gà", FoodEnglishName = "Chicken rice", IsSelected = true });
        //    data.Add(new FoodDTO() {FoodId = 2 , FoodName = "Cơm cá", FoodEnglishName = "Fish rice"});
        //    data.Add(new FoodDTO() {FoodId = 3 , FoodName = "Mỳ ý", FoodEnglishName = "Spagheti" });
        //    data.Add(new FoodDTO() {FoodId = 4 , FoodName = "Cơm bò kho", FoodEnglishName = "Vietnamese traditional BBQ rice" });
        //    data.Add(new FoodDTO() {FoodId = 5 , FoodName = "Cơm gà", FoodEnglishName = "Chicken rice" });
        //    data.Add(new FoodDTO() {FoodId = 6 , FoodName = "Cơm cá", FoodEnglishName = "Fish rice" });
        //    data.Add(new FoodDTO() {FoodId = 7 , FoodName = "Mỳ ý", FoodEnglishName = "Spagheti" });
        //    data.Add(new FoodDTO() {FoodId = 8 , FoodName = "Cơm bò kho", FoodEnglishName = "Vietnamese traditional BBQ rice" });
        //    data.Add(new FoodDTO() {FoodId = 9 , FoodName = "Cơm gà", FoodEnglishName = "Chicken rice" });
        //    data.Add(new FoodDTO() {FoodId = 10 , FoodName = "Cơm cá", FoodEnglishName = "Fish rice" });
        //    data.Add(new FoodDTO() {FoodId = 11 , FoodName = "Mỳ ý", FoodEnglishName = "Spagheti" });
        //    data.Add(new FoodDTO() {FoodId = 12 , FoodName = "Cơm bò kho", FoodEnglishName = "Vietnamese traditional BBQ rice" });
        //    return data;
        //}
    }
}
