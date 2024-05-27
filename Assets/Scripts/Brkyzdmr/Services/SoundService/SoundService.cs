using System.Collections;
using Brkyzdmr.Services.CoroutineService;
using Brkyzdmr.Services.ObjectPoolService;
using UnityEngine;

namespace Brkyzdmr.Services.SoundService
{
    public class SoundService : Service, ISoundService
    {
        private readonly IObjectPoolService _objectPoolService;
        private readonly ICoroutineService _coroutineService;

        public SoundService()
        {
            _objectPoolService = Services.GetService<IObjectPoolService>();
            _coroutineService = Services.GetService<ICoroutineService>();
        }

        public async void PlaySoundAndDespawn(string poolType, Vector3 position)
        {
            var soundGameObject = await _objectPoolService.Spawn(poolType);
            soundGameObject.transform.position = position;
            var audioSource = soundGameObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
                _coroutineService.StartCoroutine(DespawnAfterDelay(audioSource.clip.length, poolType, soundGameObject));
            }
        }
        
        private IEnumerator DespawnAfterDelay(float delay, string poolType, GameObject soundGameObject)
        {
            yield return new WaitForSeconds(delay);
            _objectPoolService.Despawn(poolType, soundGameObject);
        }
    }
}
