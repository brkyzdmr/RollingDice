using System.Collections.Generic;
using System.Threading.Tasks;
using Brkyzdmr.Services.AssetLoaderService;
using UnityEngine;

namespace Brkyzdmr.Services.ObjectPoolService
{
    public interface IObjectPoolService
    {
        public Task InitializePools(List<ObjectPool> pools, IAssetLoaderService assetLoaderService);

        public List<GameObject> GetObjectsByType(string poolType);
        public Task<GameObject> Spawn(string poolType, Vector3? position = null,
            Quaternion? rotation = null,
            Transform parent = null);

        public void Despawn(string poolType, GameObject obj);
    }
}