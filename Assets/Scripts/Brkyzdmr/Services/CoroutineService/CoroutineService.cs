using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Brkyzdmr.Services.CoroutineService
{
    public class CoroutineService : Service, ICoroutineService
    {
        private CoroutineRunner _coroutineRunner;

        public CoroutineService()
        {
            var runnerObject = new GameObject("CoroutineRunner");
            _coroutineRunner = runnerObject.AddComponent<CoroutineRunner>();
            Object.DontDestroyOnLoad(runnerObject);
        }
        
        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return _coroutineRunner.StartCoroutine(coroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            _coroutineRunner.StopCoroutine(coroutine);
        }

        public void CallWithDelay(Action action, float delay, Action onComplete = null)
        {
            _coroutineRunner.StartCoroutine(CallWithDelayCoroutine(action, delay, onComplete));
        }

        private IEnumerator CallWithDelayCoroutine(Action action, float delay, Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
            onComplete?.Invoke();
        }
    }
}