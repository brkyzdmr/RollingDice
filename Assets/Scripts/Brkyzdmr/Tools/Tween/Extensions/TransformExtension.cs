using System.Collections;
using Brkyzdmr.Tools.EzTween;
using Unity.VisualScripting;
using UnityEngine;

namespace Brkyzdmr.Tools.EzTween
{
    public static class TransformExtension
    {
        private static TweenConfig<T> CreateTransformTweenConfig<T>(this Transform transform, T targetValue)
        {
            return new TweenConfig<T>
            {
                Target = transform,
                To = targetValue
            };
        }

        #region DoMove

        public static TweenConfig<Vector3> DoMove(this Transform transform, Vector3 to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            var config = transform.CreateTransformTweenConfig(to);
            config.duration = duration;
            config.updateType = updateType;
            config.onUpdate = pos => transform.position = pos;

            return config;
        }

        public static TweenConfig<Vector3> DoMoveX(this Transform transform, float to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            return transform.DoMove(new Vector3(to, 0, 0), duration, updateType);
        }

        public static TweenConfig<Vector3> DoMoveY(this Transform transform, float to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            return transform.DoMove(new Vector3(0, to, 0), duration, updateType);
        }

        public static TweenConfig<Vector3> DoMoveZ(this Transform transform, float to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            return transform.DoMove(new Vector3(0, 0, to), duration, updateType);
        }

        #endregion

        #region DoRotate

        public static TweenConfig<Quaternion> DoRotate(this Transform transform, Quaternion to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            var config = transform.CreateTransformTweenConfig(to);
            config.duration = duration;
            config.updateType = updateType;
            config.onUpdate = rot => transform.rotation = rot;

            return config;
        }

        public static TweenConfig<Vector3> DoRotate(this Transform transform, Vector3 to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            var config = transform.CreateTransformTweenConfig(to);
            config.duration = duration;
            config.updateType = updateType;
            config.onUpdate = rot => transform.rotation = Quaternion.Euler(rot);

            return config;
        }

        public static TweenConfig<Vector3> DoRotateX(this Transform transform, float to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            return transform.DoRotate(new Vector3(to, 0, 0), duration, updateType);
        }

        public static TweenConfig<Vector3> DoRotateY(this Transform transform, float to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            return transform.DoRotate(new Vector3(0, to, 0), duration, updateType);
        }

        public static TweenConfig<Vector3> DoRotateZ(this Transform transform, float to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            return transform.DoRotate(new Vector3(0, 0, to), duration, updateType);
        }

        #endregion

        #region DoScale

        public static TweenConfig<Vector3> DoScale(this Transform transform, Vector3 to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            var config = transform.CreateTransformTweenConfig(to);
            config.From = transform.localScale;
            config.duration = duration;
            config.updateType = updateType;
            config.onUpdate = scale => transform.localScale = scale;
            return config;
        }

        public static TweenConfig<Vector3> DoScaleX(this Transform transform, float to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            return transform.DoScale(new Vector3(to, 0, 0), duration, updateType);
        }

        public static TweenConfig<Vector3> DoScaleY(this Transform transform, float to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            return transform.DoScale(new Vector3(0, to, 0), duration, updateType);
        }

        public static TweenConfig<Vector3> DoScaleZ(this Transform transform, float to, float duration,
            Tween.UpdateType updateType = Tween.UpdateType.Update)
        {
            return transform.DoScale(new Vector3(0, 0, to), duration, updateType);
        }

        #endregion

        #region DoJump


        public static Sequence DoJump(this Transform transform, Vector3 endValue, float jumpPower, int numJumps, float jumpDuration, Ease jumpEase = Ease.Linear)
        {
            if (numJumps <= 0)
                throw new System.ArgumentException("numJumps must be greater than zero.");

            var monoBehaviour = transform.GetComponent<MonoBehaviour>();
            if (monoBehaviour == null)
                throw new MissingComponentException("Target must have a MonoBehaviour component to create a sequence.");
        
            var jumpSequence = new Sequence(monoBehaviour);
            Vector3 startPos = transform.position;

            for (int i = 0; i < numJumps; i++)
            {
                float currentJumpDuration = jumpDuration / numJumps; // Calculate duration per jump
                float jumpHeight = jumpPower + endValue.y - startPos.y;

                // Upward Movement
                jumpSequence.Append(
                    transform.DoMoveY(jumpHeight, currentJumpDuration / 2f)
                        .SetEase(jumpEase)
                        .SetRelative(true) // Start from the current position
                );

                // Downward Movement to Target
                jumpSequence.Append(
                    transform.DoMove(endValue, currentJumpDuration / 2f)
                        .SetEase(jumpEase)
                        .SetRelative(true) // Continue from the previous position
                );

                if (i < numJumps - 1) // Reset if not the last jump
                {
                    jumpSequence.Append(Interpolates.Call(() => transform.position = startPos));
                }
            }
            return jumpSequence;
        }
        #endregion
    }
}