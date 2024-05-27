using System;

namespace Brkyzdmr.Services.UIService
{
    public class CounterHandler : ICounterHandler
    {
        private int _value;
        private int _min;
        private int _max;

        public int Value => _value;

        public event Action OnValueZero;
        public event Action OnValueMax;
        public event Action OnValueValid;

        protected CounterHandler(int min, int max)
        {
            _min = min;
            _max = max;
        }
        
        public void SetMinMax(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public virtual void SetValue(int value)
        {
            _value = value;

            if (_value <= _min)
            {
                _value = _min;
                OnValueZero?.Invoke();
            }
            else if (_value >= _max)
            {
                _value = _max;
                OnValueMax?.Invoke();
            }
            else
            {
                OnValueValid?.Invoke();
            }
        }
    }
}