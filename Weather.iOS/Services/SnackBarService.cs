using System.Threading.Tasks;
using Weather.iOS.Services;
using Weather.Services.SnackBarSevice;
using Xamarin.Forms;

[assembly: Dependency(typeof(SnackBarService))]
namespace Weather.iOS.Services
{
    internal class SnackBarService : ISnackBarService
    {
        public Task ShowSnackBar(string message)
        {
            var snackBar = new SnackBarBuilder
            {
                Message = message
            };


            snackBar.Show();

            return Task.CompletedTask;
        }
    }
}