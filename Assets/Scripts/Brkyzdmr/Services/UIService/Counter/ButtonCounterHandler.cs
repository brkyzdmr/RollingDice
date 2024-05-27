using UnityEngine.UI;

namespace Brkyzdmr.Services.UIService
{
    public class ButtonCounterHandler : CounterHandler
    {
        private readonly Button _increaseButton;
        private readonly Button _decreaseButton;

        public ButtonCounterHandler(Button increaseButton, Button decreaseButton, int min, int max) : base(min, max)
        {
            _increaseButton = increaseButton;
            _decreaseButton = decreaseButton;
            
            OnValueZero += OnOnValueZero;
            OnValueValid += OnOnValueValid;
            OnValueMax += OnOnValueMax;
        }

        private void OnOnValueZero()
        {
            _decreaseButton.interactable = false;
        }

        private void OnOnValueValid()
        {
            _increaseButton.interactable = true;
            _decreaseButton.interactable = true;
        }

        private void OnOnValueMax()
        {
            _increaseButton.interactable = false;
        }
    }
}