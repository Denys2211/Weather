using Android.App;
using Android.Content;
using Android.OS;
using System;
using Weather.Droid.Services;
using Weather.Services.VibrationService;
using Xamarin.Forms;

[assembly: UsesPermission(Android.Manifest.Permission.Vibrate)]
[assembly: Dependency(typeof(VibrationService))]
namespace Weather.Droid.Services
{
    public class VibrationService : IVibrator
    {
        public bool CanVibrate
        {
            get
            {
                if ((int)Build.VERSION.SdkInt >= 11)
                {
                    using (var v = (global::Android.OS.Vibrator)global::Android.App.Application.Context.GetSystemService(Context.VibratorManagerService))
                        return v.HasVibrator;
                }
                return true;
            }
        }

        public void Vibrate(TypeVibration typeVibration = TypeVibration.Double, int milliseconds = 50)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                using var vibratorManager = global::Android.App.Application.Context.GetSystemService(Context.VibratorManagerService) as VibratorManager;

                if (!vibratorManager.DefaultVibrator.HasVibrator)
                {
                    Console.WriteLine("Android device does not have vibrator.");
                    return;
                }
                vibratorManager.DefaultVibrator.Vibrate(VibrationEffect.CreateOneShot(milliseconds, typeVibration == TypeVibration.Standart ? VibrationEffect.EffectClick : VibrationEffect.EffectTick));
            }
            else
            {

                using var v = (global::Android.OS.Vibrator)global::Android.App.Application.Context.GetSystemService(Context.VibratorService);


#if __ANDROID_11__
                if (!v.HasVibrator)
                {
                    Console.WriteLine("Android device does not have vibrator.");
                    return;
                }
#endif

                if (milliseconds < 0)
                    milliseconds = 0;

                try
                {
                    v.Vibrate(VibrationEffect.CreateOneShot(milliseconds, VibrationEffect.EffectTick));
                }
                catch
                {
                    Console.WriteLine("Unable to vibrate Android device, ensure VIBRATE permission is set.");
                }
            }
        }
    }
}