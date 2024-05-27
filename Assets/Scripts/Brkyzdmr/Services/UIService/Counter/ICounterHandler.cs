using System;

namespace Brkyzdmr.Services.UIService
{
    public interface ICounterHandler
    {
        int Value { get; }
        void ChangeValue(int delta);
        event Action OnValueZero;
        event Action OnValueMax;
        event Action OnValueValid;
    }
}