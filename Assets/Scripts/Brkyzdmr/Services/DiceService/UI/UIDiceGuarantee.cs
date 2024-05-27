using System;
using System.Collections.Generic;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Services.EventService;
using RollingDice.Runtime.Event;
using UnityEngine;

namespace Brkyzdmr.Services.DiceService
{
    public class UIDiceGuarantee : MonoBehaviour
    {
        private  List<int> _diceGuaranteeCount;
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

            _diceGuaranteeCount = new List<int>();
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
                _diceGuaranteeCount.Add((int)DiceValue.Any);
            }
        }

        private void ChangeDiceCount(int count)
        {
            diceCount = count; 
        }

        public void UpdateDiceValueCount(DiceValue diceValue, int value)
        {
            _diceGuaranteeCount[(int)diceValue] = value;

            //Count all elements
            totalCount = 0;
            foreach (var diceGuarantee in _diceGuaranteeCount)
            {
                totalCount += diceGuarantee;
            }

            UpdateUIElement();
            UpdateGuarantee();
        }

        private void UpdateUIElement()
        {
            foreach (var ui in _diceElementList)
            {
                ui.UpdateUI();
            } 
        }

        /// <summary>
        /// Put the needed dice into the DiceManager
        /// </summary>
        private void UpdateGuarantee()
        {
            int diceCount = _diceService.diceCount - 1;

            Debug.Log("UpdateGuarantee: _diceGuaranteeCount:" + _diceGuaranteeCount.Count);
            

            //For every diceGuarantee on the list
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < _diceGuaranteeCount[i]; j++)
                {
                    _diceService.targetedResult[diceCount] = (DiceValue)i;
                    diceCount--;
                }
            }

            //Fill the rest with Any dice
            for (int i = diceCount; i > 0; i--)
            {
                _diceService.targetedResult[i] = DiceValue.Any;
            }
        }
    }
}