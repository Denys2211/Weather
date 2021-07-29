using Prism.Mvvm;
namespace Weather.Models
{
    public class CustomerLocation : BindableBase
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

    }
}
