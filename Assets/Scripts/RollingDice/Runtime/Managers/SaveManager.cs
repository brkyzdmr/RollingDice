using System.Linq;
using System.Threading.Tasks;
using Brkyzdmr.Generics.Singletons;
using Brkyzdmr.Services;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.SaveService;
using RollingDice.Runtime.Board;
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
            await _configService.LoadConfigs(GameData.GameConfigPath);
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
            
            if (_configService.currentLevelConfig.isRandom)
            {
                SetRandomLevelConfig();
            }
        }
        
        private void SetRandomLevelConfig()
        {
            for(int i=0; i < 20; i++)
            {
                var randomItemId = GetRandomItemId();
                ItemData itemData;
                
                if (randomItemId == "-")
                {
                    itemData = new ItemData
                    {
                        config = new ItemConfig("-", ""),
                        count = 0
                    };
                }
                else
                {
                    itemData = new ItemData
                    {
                        config = _configService.itemConfigs[randomItemId],
                        count = Random.Range(3, 15)
                    };
                }
                _configService.currentLevelConfig.tiles.Add(itemData.config.id);
                _configService.currentLevelConfig.rewards.Add(itemData.count);
            }
        }
        
        private string GetRandomItemId()
        {
            var keys = _configService.itemConfigs.Keys.ToList();
            keys.Add("-");

            var randomIndex = Random.Range(0, keys.Count);
            var randomItemId = keys[randomIndex];

            return randomItemId;
        }
    }
}