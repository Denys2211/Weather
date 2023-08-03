using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Google.Android.Material.Snackbar;
using System;
using System.Threading.Tasks;
using Weather.Droid.Services;
using Weather.Services.SnackBarSevice;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(SnackBarService))]
namespace Weather.Droid.Services
{
    public  class SnackBarService : ISnackBarService
    {
        public async Task ShowSnackBar(string message)
        {
            var renderer = await GetRendererWithRetries(Application.Current.MainPage) ?? throw new ArgumentException("Provided VisualElement cannot be parent to SnackBar", nameof(Application.Current.MainPage));
            var snackbar = Snackbar.Make(renderer.View, message, Snackbar.LengthLong);

            snackbar.View.SetBackground(ContextCompat.GetDrawable(MainActivity.AppContext, Resource.Drawable.snackBar_shape));

            snackbar.Show();
        }

        static async Task<IVisualElementRenderer?> GetRendererWithRetries(VisualElement element, int retryCount = 5)
        {
            var renderer = Platform.GetRenderer(element);
            if (renderer != null || retryCount <= 0)
                return renderer;

            await Task.Delay(50);
            return await GetRendererWithRetries(element, retryCount - 1);
        }
    }
}