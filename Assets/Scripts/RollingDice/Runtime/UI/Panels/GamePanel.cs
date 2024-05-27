using System.Collections.Generic;
using System.Linq;
using Brkyzdmr.Services;
using Brkyzdmr.Services.CoroutineService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.UIService;
using RollingDice.Runtime.Dice;
using RollingDice.Runtime.Event;
using UnityEngine;
using UnityEngine.UI;

namespace RollingDice.Runtime.UI.Panels
{
    public class GamePanel : Panel
    {
        [Header("Game Panel Settings")]
        [SerializeField] private Button rollDiceButton;
        [SerializeField] private DiceResult diceResult;
        [SerializeField] private float resultShowDuration = 2f;
        

        private IEventService _eventService;
        private ICoroutineService _coroutineService;
        private RollDiceButtonHandler _rollDiceButtonHandler;

        private void Awake()
        {
            _eventService = Services.GetService<IEventService>();
            _coroutineService = Services.GetService<ICoroutineService>();
        }

        private void Start()
        {
            _rollDiceButtonHandler = new RollDiceButtonHandler(_eventService);
            rollDiceButton.onClick.AddListener(_rollDiceButtonHandler.HandleButtonClicked);
            _rollDiceButtonHandler.PlayScaleAnimation(rollDiceButton.transform);
            SetDiceResultStatus(false);
        }
        
        private void OnEnable()
        {
            _eventService.Get<OnDiceRolled>().AddListener(SetDiceResultText);
        }

        private void OnDisable()
        {
            _eventService.Get<OnDiceRolled>().RemoveListener(SetDiceResultText);
        }

        private void SetDiceResultStatus(bool status)
        {
            diceResult.gameObject.SetActive(status);
        }

        private void SetDiceResultText(List<int> diceResults)
        {
            SetDiceResultStatus(true);
            diceResult.diceResultText.text = diceResults.Sum().ToString();
            _coroutineService.CallWithDelay(() => SetDiceResultStatus(false), resultShowDuration);
        }
    }
}