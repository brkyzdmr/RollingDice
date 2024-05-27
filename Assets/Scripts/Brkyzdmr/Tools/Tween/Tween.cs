using System;
using System.Collections;
using Brkyzdmr.Tools.EzTween;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Brkyzdmr.Tools.EzTween
{
    public class TweenConfig<T>
    {
        public Transform Target { get; set; }
        public T From { get; set; }
        public T To { get; set; }
        public float duration { get; set; }
        public AnimationCurve curve { get; set; } = EaseCurve.Get(Ease.Linear);
        public Tween.UpdateType updateType { get; set; } = Tween.UpdateType.Update;
        public Action<T> onUpdate { get; set; }
        public Action onComplete { get; set; }
        public Tween.LoopType LoopType = Tween.LoopType.Restart;

        public int Loops = 1;
        public float Delay;
        public bool IsRelative;

        public TweenConfig<T> SetRelative(bool relative)
        {
            IsRelative = relative;
            return this;
        }

        public TweenConfig<T> SetEase(Ease ease)
        {
            curve = EaseCurve.Get(ease);
            return this;
        }     
        
        public TweenConfig<T> SetDelay(float delay)
        {
            Debug.Log("SetDelay");

            this.Delay = delay;
            return this;
        }
        public TweenConfig<T> SetLoops(int loopCount, Tween.LoopType loopType = Tween.LoopType.Restart)
        {
            this.Loops = loopCount;
            this.LoopType = loopType;
            return this;
        }
        
        public TweenConfig<T> SetUpdateType(Tween.UpdateType updateType)
        {
            this.updateType = updateType;
            return this;
        }
        

        public TweenConfig<T> OnComplete(Action onComplete)
        {
            this.onComplete += onComplete;
            return this;
        }

        public TweenConfig<T> OnUpdate(Action<T> onUpdate)
        {
            this.onUpdate += onUpdate;
            return this;
        }
    }
    
    public static class Tween
    {
        public enum UpdateType
        {
            Update,
            FixedUpdate
        }
        
        public enum LoopType
        {
            Restart,
            Yoyo,
            Incremental
        }
        
        public static TweenConfig<T> TweenTo<T>(this TweenConfig<T> config)
        {
            var monoBehaviour = config.Target.GetComponent<MonoBehaviour>();
            if (monoBehaviour == null)
            {
                throw new MissingComponentException("Target must have a MonoBehaviour component to start coroutines.");
            }
            monoBehaviour.StartCoroutine(DoTweenCoroutine(config));
            return config;
        }

        public static IEnumerator DoTweenCoroutine<T>(TweenConfig<T> config)
        {
            if (config.Delay > 0)
            {
                yield return new WaitForSeconds(config.Delay);
            }
            
            if (config.IsRelative)
            {
                if (typeof(T) == typeof(Vector3))
                {
                    config.To = (T)(object)((Vector3)(object)config.From + (Vector3)(object)config.To);
                }
                else if (typeof(T) == typeof(Quaternion))
                {
                    config.To = (T)(object)((Quaternion)(object)config.From * (Quaternion)(object)config.To);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported type for relative tweening.");
                }
            }

            int loopCount = 0;
            while (config.Loops == -1 || loopCount < config.Loops)
            {
                yield return ExecuteTween(config);

                switch (config.LoopType)
                {
                    case LoopType.Restart:
                        break; // Do nothing, just restart
                    case LoopType.Yoyo:
                        (config.From, config.To) = (config.To, config.From); // Swap From and To
                        break;
                    case LoopType.Incremental:
                        if (typeof(T) == typeof(Vector3))
                        {
                            var increment = (Vector3)(object)config.To - (Vector3)(object)config.From;
                            config.From = (T)(object)((Vector3)(object)config.From + increment);
                            config.To = (T)(object)((Vector3)(object)config.To + increment);
                        }
                        else if (typeof(T) == typeof(Quaternion))
                        {
                            var increment = (Quaternion)(object)config.To *
                                            Quaternion.Inverse((Quaternion)(object)config.From);
                            config.From = (T)(object)((Quaternion)(object)config.From * increment);
                            config.To = (T)(object)((Quaternion)(object)config.To * increment);
                        }

                        break;
                }

                loopCount++;
            }

            config.onComplete?.Invoke();
        }

        private static IEnumerator ExecuteTween<T>(TweenConfig<T> config)
        {
            if (config.From is Vector3 && config.To is Vector3)
            {
                yield return Interpolates.Lerp((Vector3)(object)config.From, (Vector3)(object)config.To, config.duration,
                    (value) =>
                    {
                        config.onUpdate?.Invoke((T)(object)value);
                    }, config.updateType);
            }
            else if (config.From is Quaternion && config.To is Quaternion)
            {
                yield return Interpolates.Lerp((Quaternion)(object)config.From, (Quaternion)(object)config.To, config.duration,
                    (rot) =>
                    {
                        config.onUpdate?.Invoke((T)(object)rot);
                    }, config.updateType);
            }
            else
            {
                throw new InvalidOperationException("Unsupported type for TweenTo method.");
            }
        }

        public static IEnumerator ThenCoroutine(this IEnumerator first, IEnumerator second)
        {
            yield return first;
            yield return second;
        }
    }
}