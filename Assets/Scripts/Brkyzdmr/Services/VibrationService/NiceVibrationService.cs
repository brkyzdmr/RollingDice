// using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Brkyzdmr.Services.VibrationService
{
    public class NiceVibrationService : Service, IVibrationService
    {
        public void PlayHaptic(HapticType hapticType)
        {
            Debug.Log("Haptic Played: " + hapticType);
            // MMVibrationManager.Haptic(MapToNiceVibrationsHapticType(hapticType));
        }

        public void StopAllHaptics()
        {
            // MMVibrationManager.StopAllHaptics();
        }

        // private HapticTypes MapToNiceVibrationsHapticType(HapticType hapticType)
        // {
        //     return (HapticTypes)System.Enum.Parse(typeof(HapticTypes), hapticType.ToString());
        // }
    }
}