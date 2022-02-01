using Android.App;
using Android.Content;
using Android.OS;

namespace Weather.Droid
{
    [Activity(Label = "Weather", MainLauncher = true, Theme ="@style/Theme.Splash", NoHistory =true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
