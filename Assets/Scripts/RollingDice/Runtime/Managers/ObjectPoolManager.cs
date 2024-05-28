using System.Collections.Generic;
using Brkyzdmr.Generics.Singletons;
using Brkyzdmr.Services;
using Brkyzdmr.Services.AssetLoaderService;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.ObjectPoolService;
using UnityEngine;
using UnityEngine.Serialization;

namespace RollingDice.Runtime.Managers
{
    public class ObjectPoolManager : PersistentSingleton<SaveManager>
    {
        [SerializeField] private List<ObjectPool> objectPool;

        private IObjectPoolService _objectPoolService;
        private IAssetLoaderService _assetLoaderService;
        private IConfigService _configService;
        private IEventService _eventService;

        protected override void Awake()
        {
            _objectPoolService = Services.GetService<IObjectPoolService>();
            _assetLoaderService = Services.GetService<IAssetLoaderService>();
            _eventService = Services.GetService<IEventService>();
            InitializePools();
        }
        
        private void OnEnable()
        {
            // _eventService.Get<OnGameConfigLoaded>().AddListener(InitializePools);
        }

        private void OnDisable()
        {
            // _eventService.Get<OnGameConfigLoaded>().RemoveListener(InitializePools);
        }

        private async void InitializePools()
        {
            await _objectPoolService.InitializePools(objectPool, _assetLoaderService);
            
            // _eventService.Get<OnPoolsInitialized>().Execute();
        }
    }
}