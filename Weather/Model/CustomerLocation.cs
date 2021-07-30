using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace Weather.Models
{
    public class CustomerLocation : INotifyPropertyChanged
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string Name { get; set; }
        private bool _isSelected;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); } 
        }

    }
}
