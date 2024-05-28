using Brkyzdmr.Generics.Singletons;
using Brkyzdmr.Services;
using Brkyzdmr.Services.AnimationRecorderService;
using Brkyzdmr.Services.DiceService;
using Brkyzdmr.Services.EventService;
using RollingDice.Runtime.Event;
using UnityEngine;
using UnityEngine.Serialization;

namespace RollingDice.Runtime.Managers
{
    public class DiceManager : PersistentSingleton<SaveManager>
    {
        [Header("Dice")]
        public GameObject dicePrefab;
        public Transform startPositionRoot;
        
        [Header("Simulation")]
        public int frameLength = 300;
        public float simulationSpeed = 2f;

        private IDiceService _diceService;
        private IEventService _eventService;
        private IAnimationRecorderService _animationRecorderService;

        protected override void Awake()
        {
            _diceService = Services.GetService<IDiceService>();
            _eventService = Services.GetService<IEventService>();

            _animationRecorderService = Services.GetService<IAnimationRecorderService>();
        }
        private void Start()
        {
            _diceService.InitializeStartPositions(startPositionRoot);
            _animationRecorderService.SetSimulationParameters(frameLength, simulationSpeed);
        }
        
        private void OnEnable()
        {
            _eventService.Get<OnRollDiceButtonClicked>().AddListener(ThrowTheDice);
            _eventService.Get<OnDiceCountChanged>().AddListener(ChangeDiceCount);
        }

        private void OnDisable()
        {
            _eventService.Get<OnRollDiceButtonClicked>().RemoveListener(ThrowTheDice);
            _eventService.Get<OnDiceCountChanged>().RemoveListener(ChangeDiceCount);
        }

        private void ChangeDiceCount(int diceCount)
        {
            _diceService.diceCount = diceCount;
        }

        private void ThrowTheDice()
        {
            if (_diceService.diceCount <= 0) { return; }
            
            _diceService.RollTheDice(dicePrefab);
        }
    }
}


