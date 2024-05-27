using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.UIService;
using Brkyzdmr.Tools.EzTween;
using RollingDice.Runtime.Event;
using UnityEngine;

namespace RollingDice.Runtime.UI
{
    public class RollDiceButtonHandler : ButtonHandler
    {
        public RollDiceButtonHandler(IEventService eventService) : base(eventService) { }

        public override void HandleButtonClicked()
        {
            EventService.Get<OnRollDiceButtonClicked>().Execute();
        }

        public void PlayScaleAnimation(Transform playButton)
        {
            // playButton.DoScale(Vector3.one * 1.12f, 0.54f)
            //     .SetEase(Ease.EaseInOut)
            //     .SetLoops(-1, Tween.LoopType.Yoyo).TweenTo();
        }
    }
}