using System;

namespace Brkyzdmr.Services.UIService
{
    public interface ICounterHandler
    {
        int Value { get; }
        void SetValue(int value);
        void SetMinMax(int min, int max);
        event Action OnValueZero;
        event Action OnValueMax;
        event Action OnValueValid;
    }
}