using System.Threading.Tasks;
using Brkyzdmr.Generics.Singletons;
using Brkyzdmr.Services;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.SaveService;
using RollingDice.Runtime.Event;
using RollingDice.Runtime.Global;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RollingDice.Runtime.Managers
{
    public class SaveManager : PersistentSingleton<SaveManager>
    {
        private ISaveService _saveService;
        private IEventService _eventService;
        private IConfigService _configService;

        protected override async void Awake()
        {
            _saveService = Services.GetService<ISaveService>();
            _configService = Services.GetService<IConfigService>();
            _eventService = Services.GetService<IEventService>();
            await LoadGameConfig();

            SetCurrentLevelConfig();
            _eventService.Get<OnGameConfigLoaded>().Execute();
        }

        private void OnEnable()
        {
            _eventService.Get<OnNextLevel>().AddListener(OnNextLevel);
        }

        private void OnDisable()
        {
            _eventService.Get<OnNextLevel>().RemoveListener(OnNextLevel);
        }
        
        private async Task LoadGameConfig()
        {
            if (_configService.currentLevelConfig != null)  
            {
                Addressables.Release(_configService.currentLevelConfig); 
            }
            await _configService.LoadConfigs(GameData.GameConfigPath, GameData.BoardConfigsBasePath, 
                GameData.ItemConfigsBasePath, GameData.AvatarConfigsBasePath);
        }
        
        private void OnNextLevel()
        {
            _configService.SetNextLevelConfig();
            _saveService.SetString(SaveData.CurrentLevelId, _configService.currentLevelConfig.id);
        }

        private void SetCurrentLevelConfig()
        {
            var currentLevelId = _saveService.GetString(SaveData.CurrentLevelId, SaveData.DefaultLevelId);
            _configService.SetCurrentLevelConfig(currentLevelId);
            Debug.Log("SaveManager: currentLevelId - " + currentLevelId);
        }
    }
}