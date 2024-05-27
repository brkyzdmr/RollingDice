using System;
using System.Collections;
using Brkyzdmr.Helpers;
using Brkyzdmr.Services.CoroutineService;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Brkyzdmr.Extensions
{
    public static class TransformExtensions
    {
        public static void ShakePosition(this Transform transform, float duration = 0.5f, float intensity = 0.2f,
            AnimationCurve shakeCurve = null, bool randomize = true)
        {
            var coroutineService = Services.Services.GetService<ICoroutineService>();
            if (coroutineService != null)
            {
                coroutineService.StartCoroutine(ShakeCoroutine(transform, duration, intensity, shakeCurve,
                    GetValue: () => transform.localPosition,
                    SetValue: (value) => transform.localPosition = value,
                    randomize: randomize));
            }
        }

        public static void ShakeRotation(this Transform transform, float duration = 0.5f, float intensity = 10f,
            AnimationCurve shakeCurve = null, bool randomize = true)
        {
            var coroutineService = Services.Services.GetService<ICoroutineService>();
            if (coroutineService != null)
            {
                coroutineService.StartCoroutine(ShakeCoroutine(transform, duration, intensity, shakeCurve,
                    GetValue: () => transform.localRotation,
                    SetValue: (value) => transform.localRotation = value,
                    randomize: randomize));
            }
        }

        public static void ShakeScale(this Transform transform, float duration = 0.5f, float intensity = 0.1f,
            AnimationCurve shakeCurve = null, bool randomize = true)
        {
            var coroutineService = Services.Services.GetService<ICoroutineService>();
            if (coroutineService != null)
            {
                coroutineService.StartCoroutine(ShakeCoroutine(transform, duration, intensity, shakeCurve,
                    GetValue: () => transform.localScale,
                    SetValue: (value) => transform.localScale = value,
                    randomize: randomize));
            }
        }

        private static IEnumerator ShakeCoroutine<T>(this Transform transform, float duration, float intensity, AnimationCurve shakeCurve,
            Func<T> GetValue, Action<T> SetValue, bool randomize = true)
        {
            if (shakeCurve == null)
            {
                shakeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            }

            var originalValue = GetValue();
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var shakeFactor = shakeCurve.Evaluate(elapsedTime / duration);
                var offset = randomize ? MathHelper.GenerateRandomOffset<T>(intensity * shakeFactor) 
                    : MathHelper.CalculateNonRandomOffset<T>(elapsedTime, duration, intensity, shakeCurve); 

                SetValue(MathHelper.CombineValues(originalValue, offset));
                yield return null;
            }

            SetValue(originalValue);
        }
    }
}