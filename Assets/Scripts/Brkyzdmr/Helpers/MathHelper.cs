using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Brkyzdmr.Helpers
{
    public static class MathHelper
    {
        /// <summary>
        /// Generates a random rotation in the form of a Quaternion.
        /// </summary>
        /// <returns>A random Quaternion rotation.</returns>
        public static Quaternion GetRandomRotation()
        {
            float x = Random.Range(0, 360);
            float y = Random.Range(0, 360);
            float z = Random.Range(0, 360);
            return Quaternion.Euler(x, y, z);
        }
        
        /// <summary>
        /// Generates a random vector within specified minimum and maximum magnitudes along each axis.
        /// </summary>
        /// <param name="min">The minimum magnitude of the vector along each axis.</param>
        /// <param name="max">The maximum magnitude of the vector along each axis.</param>
        /// <returns>A random vector within the specified range.</returns>
        public static Vector3 GenerateRandomVector3(float min, float max)
        {
            float x = Random.Range(min, max);
            float y = Random.Range(min, max);
            float z = Random.Range(min, max);
            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// Smoothly moves an object towards a target direction and distance with a specified speed and dampening.
        /// </summary>
        /// <param name="objectToMove">The transform of the object to be moved.</param>
        /// <param name="direction">The direction in which the object should move.</param>
        /// <param name="specificDistance">The specific distance the object should move towards in the y-axis.</param>
        /// <param name="moveSpeed">The speed at which the object should move.</param>
        /// <param name="moveDampening">The dampening factor to smooth out the movement.</param>
        public static void SmoothMoveTowardsTarget(Transform objectToMove, Vector3 direction, float specificDistance, 
            float moveSpeed, float moveDampening)
        {
            Vector3 zoomTarget = new Vector3(objectToMove.position.x, specificDistance,
                objectToMove.position.z);
            zoomTarget -= moveSpeed * (specificDistance - objectToMove.position.y) * direction;

            objectToMove.position = Vector3.Lerp(objectToMove.position, zoomTarget,
                Time.deltaTime * moveDampening);
        }
        
        public static Vector3 ClampVectorXZ(Vector3 vector, Vector2 xzLimits)
        {
            return new Vector3(
                Mathf.Clamp(vector.x, xzLimits.x, xzLimits.y),
                vector.y, // No clamping on the Y axis
                Mathf.Clamp(vector.z, xzLimits.x, xzLimits.y)
            );
        }
        
        public static T GenerateRandomOffset<T>(float magnitude)
        {
            if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)(Random.insideUnitSphere * magnitude);
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                var randomEuler = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * magnitude;
                return (T)(object)Quaternion.Euler(randomEuler);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)Random.Range(-magnitude, magnitude);
            }
            else
            {
                throw new NotSupportedException($"Shake for type {typeof(T)} is not supported.");
            }
        }

        public static T CombineValues<T>(T original, T offset)
        {
            if (typeof(T) == typeof(Vector3))
            {
                return (T)(object)((Vector3)(object)original + (Vector3)(object)offset);
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                return (T)(object)((Quaternion)(object)original * (Quaternion)(object)offset);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)((float)(object)original + (float)(object)offset);
            }
            else
            {
                throw new NotSupportedException($"Shake for type {typeof(T)} is not supported.");
            }
        }
        
        public static T CalculateNonRandomOffset<T>(float elapsedTime, float duration, float intensity, AnimationCurve shakeCurve)
        {
            var shakeFactor = shakeCurve.Evaluate(elapsedTime / duration);

            if (typeof(T) == typeof(Vector3))
            {
                // Sinusoidal non-random movement for Vector3 (position)
                var offset = new Vector3(Mathf.Sin(elapsedTime * 5f), Mathf.Cos(elapsedTime * 3f), 0f) * intensity * shakeFactor;
                return (T)(object)offset;
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                // Sinusoidal non-random rotation for Quaternion
                var rotationAngle = Mathf.Sin(elapsedTime * 5f) * intensity * shakeFactor;
                var offset = Quaternion.AngleAxis(rotationAngle, Random.onUnitSphere); // Random axis but controlled angle
                return (T)(object)offset;
            }
            else if (typeof(T) == typeof(float))
            {
                // Sinusoidal non-random scaling for float
                var offset = (Mathf.Sin(elapsedTime * 5f) + 1f) * 0.5f * intensity * shakeFactor; // Ensure positive scaling
                return (T)(object)offset;
            }
            else
            {
                throw new NotSupportedException($"Non-random shake for type {typeof(T)} is not yet implemented.");
            }
        }

    }
}