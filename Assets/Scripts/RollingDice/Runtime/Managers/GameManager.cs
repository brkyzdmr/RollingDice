using Brkyzdmr.Generics.Singletons;
using Brkyzdmr.Services;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using RollingDice.Runtime.Event;
using UnityEngine.SceneManagement;

namespace RollingDice.Runtime.Managers
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        private IConfigService _configService;
        private IEventService _eventService;

        protected override void Awake()
        {
            _configService = Services.GetService<IConfigService>();
            _eventService = Services.GetService<IEventService>();
            
            _eventService.Get<OnGameSceneLoaded>().Execute();
        }
        
        private void OnEnable()
        {
            _eventService.Get<OnGameConfigLoaded>().AddListener(OnGameConfigLoaded);
            _eventService.Get<OnLevelRestart>().AddListener(OnLevelRestart);
        }

        private void OnDisable()
        {
            _eventService.Get<OnGameConfigLoaded>().RemoveListener(OnGameConfigLoaded);
            _eventService.Get<OnLevelRestart>().RemoveListener(OnLevelRestart);
        }
        
        private void OnLevelRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        private void OnGameConfigLoaded()
        {
            _eventService.Get<OnDiceCountChanged>().Execute(_configService.currentLevelConfig.diceCount);
        }
    }
}