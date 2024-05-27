using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brkyzdmr.Services.AssetLoaderService;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Brkyzdmr.Services.ObjectPoolService
{
    [Serializable]
    public class ObjectPool
    {
        public int MaximumInstances => maximumInstances;

        public string PoolType
        {
            get => poolType;
            set => poolType = value;
        }

        public string assetPath;
        public int maximumInstances;
        public string poolType;

        private Queue<GameObject> _inactiveObjects;
        private GameObject _poolContainer;
        private IAssetLoaderService _assetLoaderService;
        private List<GameObject> _poolObjects;

        public ObjectPool(string assetPath, int maximumInstances, string poolType)
        {
            this.assetPath = assetPath;
            this.maximumInstances = maximumInstances;
            this.poolType = poolType;
        }

        public List<GameObject> GetPoolObjects()
        {
            return _poolObjects;
        }

        public async Task InitializePool(IAssetLoaderService assetLoaderService)
        {
            _assetLoaderService = assetLoaderService;
            _inactiveObjects = new Queue<GameObject>();
            _poolContainer = new GameObject($"[Pool - {poolType}]");
            _poolObjects = new List<GameObject>();
            // Object.DontDestroyOnLoad(_poolContainer);

            for (int i = 0; i < maximumInstances; i++)
            {
                var instance = await CreateNewInstance();
                _poolObjects.Add(instance);
                DeactivateAndEnqueue(instance);
            }
        }

        private async Task<GameObject> CreateNewInstance() 
        {
            var prefab = await _assetLoaderService.LoadAsset<GameObject>(assetPath);
            var instance = Object.Instantiate(prefab);
            instance.transform.SetParent(_poolContainer.transform, true); 
            return instance; 
        }

        private void DeactivateAndEnqueue(GameObject instance)
        {
            instance.SetActive(false);
            
            _inactiveObjects.Enqueue(instance);
        }

        public async Task<GameObject> GetNextObject()
        {
            if (_inactiveObjects.Count == 0)
            {
                Debug.LogWarning($"[ObjectPool] {poolType} - Ran out of instances. Instantiating new one.");
                var newInstance = await CreateNewInstance();
                DeactivateAndEnqueue(newInstance);
            }

            var nextObject = _inactiveObjects.Dequeue();
            nextObject.SetActive(true);
            return nextObject;
        }

        public void ReturnObjectToPool(GameObject obj)
        {
            obj.transform.SetParent(_poolContainer.transform);
            _inactiveObjects.Enqueue(obj);
        }
    }
}