using Foundation;
using System;
using System.Runtime.InteropServices;
using Weather.iOS.Services;
using Weather.Services.VibrationService;
using Xamarin.Forms;

[assembly: Dependency(typeof(VibrationService))]
namespace Weather.iOS.Services
{
    public class VibrationService : IVibrator
    {
        public bool CanVibrate => true;

        [DllImport("/System/Library/Frameworks/AudioToolbox.framework/AudioToolbox")]
        private static extern void AudioServicesPlaySystemSoundWithVibration(uint inSystemSoundID, NSObject arg,
            IntPtr pattert);

        public void Vibrate(TypeVibration typeVibration = TypeVibration.Double, int milliseconds = 50)
        {
            var dictionary = new NSMutableDictionary();
            NSMutableArray vibePattern;

            if (typeVibration == TypeVibration.Standart)
            {
                vibePattern = new NSMutableArray
                {
                    NSNumber.FromBoolean(true),
                    NSNumber.FromInt32(milliseconds),
                    NSNumber.FromBoolean(false),
                    NSNumber.FromInt32(milliseconds/2)
                };
            }
            else if (typeVibration == TypeVibration.Double)
            {
                vibePattern = new NSMutableArray
                {
                    NSNumber.FromBoolean(true),
                    NSNumber.FromInt32(milliseconds),
                    NSNumber.FromBoolean(false),
                    NSNumber.FromInt32(milliseconds/2),
                    NSNumber.FromBoolean(true),
                    NSNumber.FromInt32(milliseconds)
                };
            }
            else
            {
                vibePattern = new NSMutableArray
                {
                    NSNumber.FromBoolean(true),
                    NSNumber.FromInt32(milliseconds),
                    NSNumber.FromBoolean(false),
                    NSNumber.FromInt32(milliseconds/2),
                    NSNumber.FromBoolean(true),
                    NSNumber.FromInt32(milliseconds),
                    NSNumber.FromBoolean(false),
                    NSNumber.FromInt32(milliseconds/2),
                    NSNumber.FromBoolean(true),
                    NSNumber.FromInt32(milliseconds)
                };
            }

           
            dictionary.Add(NSObject.FromObject("VibePattern"), vibePattern);
            dictionary.Add(NSObject.FromObject("Intensity"), NSNumber.FromInt32(1));
            AudioServicesPlaySystemSoundWithVibration(4095U, null, dictionary.Handle);
        }
    }
}