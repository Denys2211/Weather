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
        public Task ShowSnackBar(string message)
        {
            var snackBar = new SnackBarBuilder
            {
                Message = message,
                Icon = UIImage.FromBundle("icon_about@2xWhite")
            };


            snackBar.Show();

            return Task.CompletedTask;
        }
    }
}