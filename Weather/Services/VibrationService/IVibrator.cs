namespace Weather.Services.VibrationService
{
    public interface IVibrator
    {
        void Vibrate(TypeVibration typeVibration = TypeVibration.Double, int milliseconds = 50);
        bool CanVibrate { get; }
    }
}
