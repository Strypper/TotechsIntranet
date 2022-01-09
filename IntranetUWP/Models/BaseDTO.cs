using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IntranetUWP.Models
{
    public class BaseDTO : INotifyPropertyChanged
    {
        public int id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
