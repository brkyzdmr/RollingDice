using System;

namespace Brkyzdmr.Services.UIService
{
    public interface ICounterHandler
    {
        int Value { get; }
        void SetMinMax(int min, int max);
        void ChechIsValueValid(int value, int remainingValue);
        event Action OnValueZero;
        event Action OnValueMax;
        event Action OnValueValid;
    }
}