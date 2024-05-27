using System.Collections.Generic;
using System.Threading.Tasks;
using Brkyzdmr.Services.AssetLoaderService;
using UnityEngine;

namespace Brkyzdmr.Services.ObjectPoolService
{
    public class ObjectPoolService : Service, IObjectPoolService
    {
        private Dictionary<string, ObjectPool> _objectPools;

        public ObjectPoolService() { }

        public async Task InitializePools(List<ObjectPool> pools, IAssetLoaderService assetLoaderService)
        {
            _objectPools = new Dictionary<string, ObjectPool>();

            foreach (var pool in pools)
            {
                await pool.InitializePool(assetLoaderService);
                _objectPools[pool.poolType] = pool;
            }
        }

        public List<GameObject> GetObjectsByType(string poolType)
        {
            return _objectPools[poolType].GetPoolObjects();
        }

        public async Task<GameObject> Spawn(string poolType, Vector3? position = null, Quaternion? rotation = null,
            Transform parent = null)
        {
            if (_objectPools == null)
            {
                Debug.LogError("Object pools not initialized.");
                return null;
            }

            if (!_objectPools.TryGetValue(poolType, out var pool))
            {
                Debug.LogError(
                    $"Pool Type {poolType} not found. Available pools: {string.Join(", ", _objectPools.Keys)}");
                return null;
            }

            var obj = await pool.GetNextObject();

            if (obj == null)
            {
                Debug.LogError($"Pool {poolType} is out of objects.");
                return null;
            }

            obj.transform.SetParent(parent);
            obj.transform.position = position ?? Vector3.zero;
            obj.transform.rotation = rotation ?? Quaternion.identity;
            obj.SetActive(true);

            return obj;
        }


        public void Despawn(string poolType, GameObject obj)
        {
            if (!_objectPools.TryGetValue(poolType, out var pool))
            {
                Debug.LogError($"Pool Type {poolType} not found.");
                return;
            }

            obj.SetActive(false);
            pool.ReturnObjectToPool(obj);
        }
    }
}