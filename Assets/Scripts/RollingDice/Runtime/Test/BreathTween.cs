using System;
using Brkyzdmr.Tools.EzTween;
using UnityEngine;

namespace RollingDice.Runtime.Test
{
    public class BreathTween : MonoBehaviour
    {
        [SerializeField] private float scale = 1.15f;
        [SerializeField] private float duration = 0.8f;
        [SerializeField] private Ease ease = Ease.EaseInOut;

        private void Start()
        {
            transform.DoScale(Vector3.one * scale, duration).SetEase(ease)
                .SetLoops(-1, Tween.LoopType.Yoyo).TweenTo();
        }
    }
}