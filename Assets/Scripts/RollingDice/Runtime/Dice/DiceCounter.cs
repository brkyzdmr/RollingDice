using System;
using Brkyzdmr.Services;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.DiceService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.UIService;
using RollingDice.Runtime.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RollingDice.Runtime.Dice
{
    public class DiceCounter : MonoBehaviour
    {
        [SerializeField] protected TMP_Text countText;
        [SerializeField] protected Button increaseButton;
        [SerializeField] protected Button decreaseButton;
        
        private IEventService _eventService;
        private IConfigService _configService;
        private ICounterHandler _counterHandler;
        private int _remainingDiceCount;
        private int _maxDiceCount;

        private void Awake()
        {
            _eventService = Services.GetService<IEventService>();
            _configService = Services.GetService<IConfigService>();
            _counterHandler = new ButtonCounterHandler(increaseButton, decreaseButton, 0, 2);
        }
        
        private void Start()
        {
            increaseButton.onClick.AddListener(() => ChangeCount(1));
            decreaseButton.onClick.AddListener(() => ChangeCount(-1));
            UpdateUI();
        }

        private void OnEnable()
        {
            _eventService.Get<OnDiceCountChanged>().AddListener(UpdateCountText);
            _eventService.Get<OnGameConfigLoaded>().AddListener(OnGameConfigLoaded);
        }

        private void OnDisable()
        {
            _eventService.Get<OnDiceCountChanged>().RemoveListener(UpdateCountText);
            _eventService.Get<OnGameConfigLoaded>().RemoveListener(OnGameConfigLoaded);
        }

        private void OnGameConfigLoaded()
        {
            _counterHandler.SetMinMax(0, _configService.gameConfig.maxDiceCount);
            _maxDiceCount = _configService.gameConfig.maxDiceCount;
        }

        private void ChangeCount(int change)
        {
            int newValue = _counterHandler.Value + change < 0 ? 0 : _counterHandler.Value + change;
            
            _eventService.Get<OnDiceCountChanged>().Execute(newValue);
        }

        private void UpdateCountText(int value)
        {
            _remainingDiceCount = _maxDiceCount - value;
            _counterHandler.ChechIsValueValid(value, _remainingDiceCount);
            UpdateUI();
        }

        private void UpdateUI()
        {
            countText.text = _counterHandler.Value.ToString();
        }
    }
}
