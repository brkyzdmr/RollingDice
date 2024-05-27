using System;
using Brkyzdmr.Services;
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

        private CounterHandler _counterHandler;
        private IEventService _eventService;

        private void Awake()
        {
            _eventService = Services.GetService<IEventService>();
            _counterHandler = new CounterHandler(0, 50);
            _counterHandler.OnValueZero += OnValueZero;
            _counterHandler.OnValueMax += OnValueMax;
            _counterHandler.OnValueValid += OnValueValid;
        }
        
        private void Start()
        {
            increaseButton.onClick.AddListener(() => ChangeCount(1));
            decreaseButton.onClick.AddListener(() => ChangeCount(-1));
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
            _eventService.Get<OnDiceCountChanged>().AddListener(UpdateCountText);
        }

        private void OnDisable()
        {
            _eventService.Get<OnDiceCountChanged>().RemoveListener(UpdateCountText);
        }

        private void ChangeCount(int change)
        {
            int newValue = _counterHandler.Value + change;
            _eventService.Get<OnDiceCountChanged>().Execute(newValue);
            _eventService.Get<OnGuaranteeCountChanged>().Execute();
        }

        private void UpdateCountText(int value)
        {
            _counterHandler.ChangeValue(value);
            countText.text = _counterHandler.Value.ToString();
        }

        private void UpdateUI()
        {
            countText.text = _counterHandler.Value.ToString();
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
