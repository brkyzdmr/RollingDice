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
        public event Action OnValueNotValid;

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

        public void ChechIsValueValid(int value, int remainingValue)
        {
            _value = value;
            
            if (value == 0 && remainingValue == 0)
            {
                OnValueNotValid?.Invoke();
            }
            else if (value > _min && remainingValue > 0)
            {
                OnValueValid?.Invoke();
            }
            else if (value < _max && remainingValue > 0)
            {
                OnValueValid?.Invoke();
            }
            else if (_value == _min)
            {
                OnValueZero?.Invoke();
            }
            else if (_value == _max)
            {
                OnValueMax?.Invoke();
            }
        }
    }
}