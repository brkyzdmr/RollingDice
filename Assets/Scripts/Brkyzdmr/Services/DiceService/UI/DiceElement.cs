using System;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.UIService;
using RollingDice.Runtime.Event;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Brkyzdmr.Services.DiceService
{
    public class DiceElement : MonoBehaviour
    {
        [SerializeField] private DiceValue diceValue = DiceValue.One;
        [SerializeField] private int diceValueCount = 0;
        [SerializeField] private DiceGuarantee diceGuarantee;

        [SerializeField] private TMP_Text countText;
        [SerializeField] private Button increaseButton;
        [SerializeField] private Button decreaseButton;

        private ICounterHandler _counterHandler;
        private IEventService _eventService;
        private IDiceService _diceService;

        private void Awake()
        {
            _diceService = Services.GetService<IDiceService>();
            _eventService = Services.GetService<IEventService>();
            _counterHandler = new ButtonCounterHandler(increaseButton, decreaseButton, 0, 2);
        }

        private void Start()
        {
            increaseButton.onClick.AddListener(PlusValue);
            decreaseButton.onClick.AddListener(MinusValue);
            UpdateUI();
        }

        private void OnEnable()
        {
            _eventService.Get<OnGuaranteeCountChanged>().AddListener(UpdateUI);
            _eventService.Get<OnGameConfigLoaded>().AddListener(OnGameConfigLoaded);
        }

        private void OnDisable()
        {
            _eventService.Get<OnGuaranteeCountChanged>().RemoveListener(UpdateUI);
            _eventService.Get<OnGameConfigLoaded>().RemoveListener(OnGameConfigLoaded);
        }

        private void OnGameConfigLoaded()
        {
            
        }

        public void PlusValue()
        {
            if (diceGuarantee.totalCount < _diceService.diceCount)
            {
                diceValueCount++;
                UpdateState();
            }
        }

        public void MinusValue()
        {
            if (diceValueCount > 0)
            {
                diceValueCount--;
                UpdateState();
            }
        }

        private void UpdateState()
        {
            diceGuarantee.UpdateDiceValueCount(diceValue, diceValueCount);
            UpdateUI();
            _eventService.Get<OnGuaranteeCountChanged>().Execute();
        }

        public void UpdateUI()
        {
            countText.text = diceValueCount.ToString();
            _counterHandler.SetMinMax(0, diceGuarantee.diceCount);
            _counterHandler.SetValue(diceValueCount);
        }
    }
}
