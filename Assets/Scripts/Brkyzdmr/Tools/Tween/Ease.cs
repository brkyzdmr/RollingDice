using UnityEngine;

namespace Brkyzdmr.Tools.EzTween
{
    public enum Ease
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        ExpoIn,
        ExpoOut,
        ExpoInOut,
        BackIn,
        BackOut,
        BounceIn,
        BounceOut,
    }
    
    public static class EaseCurve
    {
        public static AnimationCurve Get(Ease ease)
        {
            return ease switch
            {
                Ease.Linear => AnimationCurve.Linear(0, 0, 1, 1),
                Ease.EaseIn => AnimationCurve.EaseInOut(0, 0, 1, 1),
                Ease.EaseOut => new AnimationCurve(new Keyframe(0, 0, 0, 2), new Keyframe(1, 1, 0, 0)),
                Ease.EaseInOut => AnimationCurve.EaseInOut(0, 0, 1, 1),
                Ease.ExpoIn => new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 2, 0)),
                Ease.ExpoOut => new AnimationCurve(new Keyframe(0, 0, 2, 0), new Keyframe(1, 1, 0, 0)),
                Ease.ExpoInOut => new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(0.5f, 0, 0, 0), new Keyframe(1, 1, 2, 0)),
                Ease.BackIn => new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(0.6f, 0, 0, 0), new Keyframe(1, 1, -3, 1)),
                Ease.BackOut => new AnimationCurve(new Keyframe(0, 0, -3, 1), new Keyframe(0.4f, 1, 0, 0), new Keyframe(1, 1, 0, 0)),
                Ease.BounceIn => new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(0.2f, 0, 0, 0), new Keyframe(0.4f, 1, 5, -4), new Keyframe(0.6f, 1, 4, -3), new Keyframe(0.8f, 1, 3, -2), new Keyframe(1, 1, 2, 0)),
                Ease.BounceOut => new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(0.4f, 1, 5, -4), new Keyframe(0.7f, 1, 4, -3), new Keyframe(0.9f, 1, 3, -2), new Keyframe(1, 1, 2, 0)),
                _ => AnimationCurve.Linear(0, 0, 1, 1) // Default to linear
            };
        }
    }
}