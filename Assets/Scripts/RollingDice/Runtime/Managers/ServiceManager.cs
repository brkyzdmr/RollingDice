using Brkyzdmr.Generics.Singletons;
using Brkyzdmr.Services;
using Brkyzdmr.Services.AnimationRecorderService;
using Brkyzdmr.Services.AssetLoaderService;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.CoroutineService;
using Brkyzdmr.Services.DiceService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.InputService;
using Brkyzdmr.Services.ObjectPoolService;
using Brkyzdmr.Services.ParticleService;
using Brkyzdmr.Services.SaveService;
using Brkyzdmr.Services.SoundService;
using Brkyzdmr.Services.UIService;
using Brkyzdmr.Services.VibrationService;
using UnityEngine;

namespace RollingDice.Runtime.Managers
{
    public class ServiceManager : PersistentSingleton<SaveManager>
    {
        protected override void Awake()
        {
            CreateServices();
        }
        
        private void CreateServices()
        {
            Services.RegisterService<ICoroutineService>(new CoroutineService());
            Services.RegisterService<IEventService>(new EventService());
            Services.RegisterService<IVibrationService>(new NiceVibrationService());
            Services.RegisterService<ISaveService>(new PlayerPrefsSaveService());
            Services.RegisterService<IAssetLoaderService>(new AddressablesLoaderService());
            Services.RegisterService<IDiceService>(new DiceService());
            Services.RegisterService<IConfigService>(new ConfigService());
            Services.RegisterService<IInputService>(new UnityInputService());
            Services.RegisterService<IObjectPoolService>(new ObjectPoolService());
            Services.RegisterService<IUIService>(new UIService());
            // Services.RegisterService<IGoalService>(new GoalService());
            // Services.RegisterService<IMatchService>(new MatchService());
            Services.RegisterService<IParticleService>(new ParticleService());
            Services.RegisterService<ISoundService>(new SoundService());
            Services.RegisterService<IAnimationRecorderService>(new AnimationRecorderService());

            Debug.Log("ServiceManager: Services initialized!");
        }
    }
}