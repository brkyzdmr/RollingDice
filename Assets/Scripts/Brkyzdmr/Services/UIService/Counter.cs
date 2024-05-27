using System;
using Brkyzdmr.Services.EventService;
using RollingDice.Runtime.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brkyzdmr.Services.UIService
{
    [Serializable]
    public abstract class Counter : MonoBehaviour
    {
        [SerializeField] protected TMP_Text countText;
        [SerializeField] protected Button increaseButton;
        [SerializeField] protected Button decreaseButton;

        protected CounterHandler CounterHandler;
        protected IEventService EventService;

        protected virtual void Awake()
        {
            EventService = Services.GetService<IEventService>();
            CounterHandler = new CounterHandler(0, 50);
            CounterHandler.OnValueZero.AddListener(OnValueZero);
            CounterHandler.OnValueMax.AddListener(OnValueMax);
            CounterHandler.OnValueValid.AddListener(OnValueValid);
        }

        protected virtual void Start()
        {
            increaseButton.onClick.AddListener(() => ChangeCount(1));
            decreaseButton.onClick.AddListener(() => ChangeCount(-1));
            UpdateUI();
        }

        protected virtual void ChangeCount(int change)
        {
            int newValue = CounterHandler.value + change;
            EventService.Get<OnDiceCountChanged>().Execute(newValue);
        }

        protected void UpdateCountText(int value)
        {
            CounterHandler.SetValue(value);
            countText.text = CounterHandler.value.ToString();
        }

        public virtual void UpdateUI()
        {
            countText.text = CounterHandler.value.ToString();
        }

        protected void OnValueZero()
        {
            decreaseButton.interactable = false;
        }

        protected void OnValueMax()
        {
            increaseButton.interactable = false;
        }

        protected void OnValueValid()
        {
            increaseButton.interactable = true;
            decreaseButton.interactable = true;
        }
    }
}