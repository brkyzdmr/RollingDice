using Brkyzdmr.Services.DiceService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.UIService;
using Brkyzdmr.Tools.EzTween;
using RollingDice.Runtime.Event;
using UnityEngine;

namespace RollingDice.Runtime.UI
{
    public class RollDiceButtonHandler : ButtonHandler
    {
        private readonly IDiceService _diceService;

        public RollDiceButtonHandler(IEventService eventService, IDiceService diceService) : base(eventService)
        {
            _diceService = diceService;
            eventService.Get<OnAvatarMoveCompleted>().AddListener(() => SetButtonStatus(true));
        }

        public override void HandleButtonClicked()
        {
            if (_diceService.diceCount <= 0) { return; }
            EventService.Get<OnRollDiceButtonClicked>().Execute();
            SetButtonStatus(false);
        }

        public void PlayScaleAnimation(Transform playButton)
        {
            // playButton.DoScale(Vector3.one * 1.12f, 0.54f)
            //     .SetEase(Ease.EaseInOut)
            //     .SetLoops(-1, Tween.LoopType.Yoyo).TweenTo();
        }
    }
}