using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brkyzdmr.Tools.EzTween
{
    public class Sequence
    {
        private List<IEnumerator> _tweens = new List<IEnumerator>();
        private MonoBehaviour _monoBehaviour;
        private bool _isPlaying;

        public Sequence(MonoBehaviour monoBehaviour)
        {
            this._monoBehaviour = monoBehaviour;
        }

        public Sequence Append(IEnumerator tween)
        {
            _tweens.Add(tween);
            return this;
        }

        public Sequence Append<T>(TweenConfig<T> config)
        {
            _tweens.Add(Tween.DoTweenCoroutine(config));
            return this;
        }

        public Sequence Insert<T>(float atPosition, TweenConfig<T> tween)
        {
            _tweens.Insert(Mathf.Clamp((int)(atPosition * _tweens.Count), 0, _tweens.Count), Tween.DoTweenCoroutine(tween));
            return this;
        }

        public void Play()
        {
            if (_isPlaying) return;
            _isPlaying = true;
            _monoBehaviour.StartCoroutine(PlaySequence());
        }

        public void PlayConcurrent()
        {
            if (_isPlaying) return;
            _isPlaying = true;
            _monoBehaviour.StartCoroutine(PlayConcurrentSequence());
        }

        private IEnumerator PlaySequence()
        {
            foreach (var tween in _tweens)
            {
                yield return tween;
            }

            _isPlaying = false;
        }

        private IEnumerator PlayConcurrentSequence()
        {
            List<Coroutine> runningCoroutines = new List<Coroutine>();

            // Start all tweens simultaneously
            foreach (var tween in _tweens)
            {
                runningCoroutines.Add(_monoBehaviour.StartCoroutine(tween));
            }

            // Wait for all tweens to finish
            yield return _monoBehaviour.StartCoroutine(WaitForAllCoroutines(runningCoroutines));
            _isPlaying = false;
        }

        private IEnumerator WaitForAllCoroutines(List<Coroutine> coroutines)
        {
            foreach (var coroutine in coroutines)
            {
                Debug.Log("Coroutine: " + coroutine);
                yield return coroutine;
            }
        }
    }
}