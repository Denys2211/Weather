using System.Threading.Tasks;
using UIKit;
using Weather.iOS.Services;
using Weather.Services.SnackBarSevice;
using Xamarin.Forms;

[assembly: Dependency(typeof(SnackBarService))]
namespace Weather.iOS.Services
{
    internal class SnackBarService : ISnackBarService
    {
        private SnackBarBuilder _snackBar;

        public Task ShowSnackBar(string message)
        {
            _snackBar?.Dismiss();

             _snackBar = new SnackBarBuilder
            {
                Message = message,
                Icon = UIImage.FromBundle("icon_about@2xWhite")
            };


            _snackBar.Show();

            return Task.CompletedTask;
        }
    }
}