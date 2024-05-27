using UnityEngine;

namespace Brkyzdmr.Services.ParticleService
{
    public interface IParticleService
    {
        void PlayParticle(string particleId, Vector3 position);
    }
}