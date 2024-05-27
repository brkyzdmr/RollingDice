using UnityEngine;
using UnityEngine.Events;

namespace Brkyzdmr.Services.UIService
{
    public class CounterHandler
    {
        private int _value;
        private int _minValue;
        private int _maxValue;

        public UnityEvent OnValueZero = new UnityEvent();
        public UnityEvent OnValueMax = new UnityEvent();
        public UnityEvent OnValueValid = new UnityEvent();

        public int value
        {
            get => _value;
            private set
            {
                _value = Mathf.Clamp(value, _minValue, _maxValue);
                InvokeEvents();
            }
        }

        public CounterHandler(int minValue, int maxValue, int initialValue = 0)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            value = initialValue;
        }

        
        public void SetValue(int newValue)
        {
            value = newValue;
        }
        
        public void Increment()
        {
            value++;
        }

        public void Decrement()
        {
            value--;
        }

        private void InvokeEvents()
        {
            OnValueValid.Invoke();

            if (_value <= _minValue)
            {
                OnValueZero.Invoke();
            }

            if (_value >= _maxValue)
            {
                OnValueMax.Invoke();
            }
        }
    }

}