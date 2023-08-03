using System.Threading.Tasks;

namespace Weather.Services.SnackBarSevice
{
    public interface ISnackBarService
    {
        Task ShowSnackBar(string message);
    }
}
