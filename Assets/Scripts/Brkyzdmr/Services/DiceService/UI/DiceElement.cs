using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.UIService;
using RollingDice.Runtime.Event;
using UnityEngine;
using TMPro;

namespace Brkyzdmr.Services.DiceService
{
    public class DiceElement : Counter
    {
        [SerializeField] private DiceValue diceValue = DiceValue.One;
        [SerializeField] private int diceValueCount = 0;
        [SerializeField] private UIDiceGuarantee diceGuarantee;
        
        protected override void Start()
        {
            base.Start();
            UpdateUI();
        }
        
        protected virtual void OnEnable()
        {
            EventService.Get<OnGuaranteeCountChanged>().AddListener(ChangeGuaranteeCount);
        }

        protected virtual void OnDisable()
        {
            EventService.Get<OnGuaranteeCountChanged>().RemoveListener(ChangeGuaranteeCount);
        }

        private void ChangeGuaranteeCount(int value)
        {
            diceValueCount = value;
            UpdateUI();
            UpdateUIMain();
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            countText.text = diceValueCount.ToString();
            
            if (diceValueCount <= 0)
            {
                OnValueZero();
            }

            if (diceGuarantee.totalCount >= diceGuarantee.diceCount)
            {
                OnValueMax();
            }
            else if (diceGuarantee.totalCount <= 0)
            {
                OnValueZero();
            }
        }

        private void UpdateUIMain()
        {
            diceGuarantee.UpdateDiceValueCount(diceValue, diceValueCount);
        }
    }
}