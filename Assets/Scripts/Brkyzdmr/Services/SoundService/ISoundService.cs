using UnityEngine;

namespace Brkyzdmr.Services.SoundService
{
    public interface ISoundService
    {
        void PlaySoundAndDespawn(string poolType, Vector3 position);
    }
}