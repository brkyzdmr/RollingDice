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

        private CounterHandler _counterHandler;
        private IEventService _eventService;
        private IDiceService _diceService;

        private void Awake()
        {
            _diceService = Services.GetService<IDiceService>();
            _eventService = Services.GetService<IEventService>();
            _counterHandler = new CounterHandler(0, 50);
            _counterHandler.OnValueZero += OnValueZero;
            _counterHandler.OnValueMax += OnValueMax;
            _counterHandler.OnValueValid += OnValueValid;
        }

        private void Start()
        {
            increaseButton.onClick.AddListener(PlusValue);
            decreaseButton.onClick.AddListener(MinusValue);
            UpdateUI();
        }

        private void OnDestroy()
        {
            _counterHandler.OnValueZero -= OnValueZero;
            _counterHandler.OnValueMax -= OnValueMax;
            _counterHandler.OnValueValid -= OnValueValid;
        }

        private void OnEnable()
        {
            _eventService.Get<OnGuaranteeCountChanged>().AddListener(UpdateUI);
        }

        private void OnDisable()
        {
            _eventService.Get<OnGuaranteeCountChanged>().RemoveListener(UpdateUI);
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
            _counterHandler.ChangeValue(diceValueCount);

            if (diceValueCount <= 0)
            {
                OnValueZero();
            }
            else
            {
                OnValueValid();
            }

            if (diceGuarantee.totalCount >= diceGuarantee.diceCount)
            {
                OnValueMax();
            }
        }

        private void OnValueZero()
        {
            decreaseButton.interactable = false;
        }

        private void OnValueMax()
        {
            increaseButton.interactable = false;
        }

        private void OnValueValid()
        {
            increaseButton.interactable = true;
            decreaseButton.interactable = true;
        }
    }
}
