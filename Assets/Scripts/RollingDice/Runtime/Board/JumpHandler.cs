using System.Collections;
using System;
using UnityEngine;
using Brkyzdmr.Services.CoroutineService;

namespace RollingDice.Runtime.Board
{
    public class JumpHandler
    {
        private readonly ICoroutineService _coroutineService;

        public JumpHandler(ICoroutineService coroutineService)
        {
            _coroutineService = coroutineService;
        }

        public void Jump(GameObject gameObject, Vector3 from, Vector3 to, float height, float duration, 
            AnimationCurve riseCurve, AnimationCurve fallCurve, Action onComplete = null)
        {
            _coroutineService.StartCoroutine(JumpCoroutine(gameObject, from, to, height, duration, 
                riseCurve, fallCurve, onComplete));
        }

        private IEnumerator JumpCoroutine(GameObject gameObject, Vector3 from, Vector3 to, float height, float duration, 
            AnimationCurve riseCurve, AnimationCurve fallCurve, Action onComplete)
        {
            float elapsedTime = 0f;
            float halfwayDuration = duration / 2f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                float curveValue = (elapsedTime < halfwayDuration) 
                    ? riseCurve.Evaluate(t * 2) 
                    : fallCurve.Evaluate((t - 0.5f) * 2);

                Vector3 currentPos = Vector3.Lerp(from, to, t);
                currentPos.y += curveValue * height;

                gameObject.transform.position = currentPos;
                yield return null;
            }

            gameObject.transform.position = to;
            onComplete?.Invoke();
        }
    }
}