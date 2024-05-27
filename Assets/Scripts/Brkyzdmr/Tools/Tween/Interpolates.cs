using System;
using System.Collections;
using UnityEngine;

namespace Brkyzdmr.Tools.EzTween
{
    public static class Interpolates
    {
        private static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

        /// <summary>
        /// Linearly interpolates from <paramref name="from"> to <paramref name="to"> for the given <paramref name="duration"> in seconds.
        /// Calls <paramref name="callback"> and passes the interpolated value to it in every step.
        /// <param name="from">The initial value to start from.</param>
        /// <param name="to">The target value to interpolate towards.</param>
        /// <param name="duration">The duration in seconds the interpolation is supposed to last.</param>
        /// <param name="callback">The callback called each step. The current interpolation result is passed to it.</param>
        /// <param name="updateType">Choose between interpolation in Update or FixedUpdate.</param>
        /// </summary>
        public static IEnumerator Lerp(Vector3 from,
            Vector3 to,
            float duration,
            Action<Vector3> callback,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            for (var time = Time.deltaTime; time < duration; time += Time.deltaTime)
            {
                var t = time / duration;
                callback(Vector3.Lerp(from, to, t));
                
                yield return updateType == Tween.UpdateType.FixedUpdate ? WaitForFixedUpdate : null;
            }
            callback(to);
        }
        
        /// <summary>
        /// Spherically interpolates from <paramref name="from"> to <paramref name="to"> for the given <paramref name="duration"> in seconds.
        /// Calls <paramref name="callback"> and passes the interpolated value to it in every step.
        /// <param name="from">The initial value to start from.</param>
        /// <param name="to">The target value to interpolate towards.</param>
        /// <param name="duration">The duration in seconds the interpolation is supposed to last.</param>
        /// <param name="callback">The callback called each step. The current interpolation result is passed to it.</param>
        /// <param name="updateType">Choose between interpolation in Update or FixedUpdate.</param>
        /// </summary>
        public static IEnumerator Lerp(Quaternion from,
            Quaternion to,
            float duration,
            Action<Quaternion> callback,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            for (var time = Time.deltaTime; time < duration; time += Time.deltaTime)
            {
                var t = time / duration;
                callback(Quaternion.Slerp(from, to, t));
                
                yield return updateType == Tween.UpdateType.FixedUpdate ? WaitForFixedUpdate : null;            
            }
            callback(to);
        }
        
        public static IEnumerator Call(Action callback)
        {
            callback?.Invoke();
            yield return null;
        }
    }
}