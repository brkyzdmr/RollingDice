using System.Collections.Generic;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using RollingDice.Runtime.Event;
using UnityEngine;

namespace Brkyzdmr.Services.DiceService
{
    public class DiceGuarantee : MonoBehaviour
    {
        private List<int> _diceGuaranteeCount;
        private List<DiceElement> _diceElementList;

        private IDiceService _diceService;
        private IEventService _eventService;
        private IConfigService _configService;
        public int diceCount;
        public int totalCount;

        private void Awake()
        {
            _diceService = Services.GetService<IDiceService>();
            _eventService = Services.GetService<IEventService>();
            _configService = Services.GetService<IConfigService>();

            _diceGuaranteeCount = new List<int>(new int[6]);
            _diceElementList = new List<DiceElement>(6);
        }

        private void OnEnable()
        {
            _eventService.Get<OnDiceCountChanged>().AddListener(ChangeDiceCount);
            _eventService.Get<OnGameConfigLoaded>().AddListener(InitializeGuaranteeCount);
        }

        private void OnDisable()
        {
            _eventService.Get<OnDiceCountChanged>().RemoveListener(ChangeDiceCount);
            _eventService.Get<OnGameConfigLoaded>().RemoveListener(InitializeGuaranteeCount);
        }

        private void InitializeGuaranteeCount()
        {
            for (int i = 0; i < 6; i++)
            {
                _diceGuaranteeCount[i] = 0;
            }
        }

        private void ChangeDiceCount(int count)
        {
            diceCount = count;
        }

        public void UpdateDiceValueCount(DiceValue diceValue, int value)
        {
            if ((int)diceValue < 0 || (int)diceValue >= _diceGuaranteeCount.Count)
                return;

            _diceGuaranteeCount[(int)diceValue] = value;

            // Recalculate total count
            totalCount = 0;
            foreach (var count in _diceGuaranteeCount)
            {
                totalCount += count;
            }

            UpdateUIElement();
            UpdateGuarantee();
        }

        private void UpdateUIElement()
        {
            foreach (var uiElement in _diceElementList)
            {
                uiElement.UpdateUI();
            }
        }

        private void UpdateGuarantee()
        {
            int remainingDice = _diceService.diceCount - 1;

            for (int i = 0; i < _diceGuaranteeCount.Count; i++)
            {
                for (int j = 0; j < _diceGuaranteeCount[i]; j++)
                {
                    if (remainingDice < 0) break;
                    _diceService.targetedResult[remainingDice] = (DiceValue)i;
                    remainingDice--;
                }
            }

            // Fill the rest with Any dice
            while (remainingDice >= 0)
            {
                _diceService.targetedResult[remainingDice] = DiceValue.Any;
                remainingDice--;
            }
        }
    }
}
