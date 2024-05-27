using System.Collections;
using Brkyzdmr.Services.CoroutineService;
using Brkyzdmr.Services.ObjectPoolService;
using UnityEngine;

namespace Brkyzdmr.Services.ParticleService
{
    public class ParticleService : Service, IParticleService
    {
        private readonly IObjectPoolService _objectPoolService;
        private readonly ICoroutineService _coroutineService;

        public ParticleService()
        {
            _objectPoolService = Services.GetService<IObjectPoolService>();
            _coroutineService = Services.GetService<ICoroutineService>();
        }

        public async void PlayParticle(string particleId, Vector3 position)
        {
            var particleGameObject = await _objectPoolService.Spawn(particleId, position);
            
            if (particleGameObject != null)
            {
                PlayAllParticleSystemsAndDespawn(particleGameObject, particleId);
            }
        }

        private void PlayAllParticleSystemsAndDespawn(GameObject particleGameObject, string poolType)
        {
            float maxDuration = 0f;
            foreach (var particleSystem in particleGameObject.GetComponentsInChildren<ParticleSystem>())
            {
                particleSystem.Play();
                if (particleSystem.main.duration > maxDuration)
                {
                    maxDuration = particleSystem.main.duration;
                }
            }

            _coroutineService.StartCoroutine(DespawnAfterDelay(maxDuration, poolType, particleGameObject));

        }
        
        private IEnumerator DespawnAfterDelay(float delay, string poolType, GameObject soundGameObject)
        {
            yield return new WaitForSeconds(delay);
            _objectPoolService.Despawn(poolType, soundGameObject);
        }
    }
}