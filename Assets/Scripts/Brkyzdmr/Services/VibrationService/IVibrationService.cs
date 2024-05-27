namespace Brkyzdmr.Services.VibrationService
{
    public interface IVibrationService
    {
        public void PlayHaptic(HapticType hapticType);
        public void StopAllHaptics();
    }
}