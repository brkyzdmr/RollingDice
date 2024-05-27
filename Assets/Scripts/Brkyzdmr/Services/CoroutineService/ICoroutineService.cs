using System;
using System.Collections;
using UnityEngine;

namespace Brkyzdmr.Services.CoroutineService
{
    public interface ICoroutineService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
        void CallWithDelay(Action action, float delay, Action onComplete = null);
    }
}