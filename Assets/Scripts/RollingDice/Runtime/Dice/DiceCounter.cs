using Brkyzdmr.Services.UIService;
using RollingDice.Runtime.Event;

namespace RollingDice.Runtime.Dice
{
    public class DiceCounter : Counter
    {
        private int _oldValue;
        
        private void OnEnable()
        {
            EventService.Get<OnDiceCountChanged>().AddListener(UpdateCountText);
        }

        private void OnDisable()
        {
            EventService.Get<OnDiceCountChanged>().RemoveListener(UpdateCountText);
        }
    }
}
