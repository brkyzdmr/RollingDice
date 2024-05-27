using UnityEngine;

namespace Brkyzdmr.Helpers
{
    public static class CameraHelper
    {
        public enum Axis
        {
            X, Y, Z
        }
        
        /// <summary>
        /// Calculates a move direction based on an object's position relative to screen edges.
        /// </summary>
        /// <param name="objectTransform">The Transform of the object to move.</param>
        /// <param name="screenPosition">The screen position of the object.</param>
        /// <param name="edgeTolerance">How close to the edge (as a fraction of screen size) triggers movement.</param>
        /// <param name="horizontalAxis">The axis to use for horizontal movement.</param>
        /// <param name="verticalAxis">The axis to use for vertical movement.</param>
        /// <returns>The calculated move direction.</returns>
        public static Vector3 CalculateEdgeMoveDirection(
            Transform objectTransform, 
            Vector2 screenPosition, 
            float edgeTolerance,
            Axis horizontalAxis = Axis.X,
            Axis verticalAxis = Axis.Z)
        {
            Vector3 moveDirection = Vector3.zero;

            if (screenPosition.x < edgeTolerance * Screen.width)
                moveDirection -= GetAxisVector(objectTransform, horizontalAxis);
            else if (screenPosition.x > (1f - edgeTolerance) * Screen.width)
                moveDirection += GetAxisVector(objectTransform, horizontalAxis);

            if (screenPosition.y < edgeTolerance * Screen.height)
                moveDirection -= GetAxisVector(objectTransform, verticalAxis);
            else if (screenPosition.y > (1f - edgeTolerance) * Screen.height)
                moveDirection += GetAxisVector(objectTransform, verticalAxis);

            return moveDirection;
        }

        /// <summary>
        /// Gets a normalized vector representing a specified axis from a Transform.
        /// </summary>
        public static Vector3 GetAxisVector(Transform transform, Axis axis)
        {
            switch (axis)
            {
                case Axis.X: return transform.right;
                case Axis.Y: return transform.up;
                case Axis.Z: return transform.forward;
                default: throw new System.ArgumentException("Invalid axis");
            }
        }

        public static Vector3 CalculateCameraMovement(Vector3 targetPosition, ref float speed, 
            ref Vector3 horizontalVelocity, float maxSpeed, float acceleration, float damping)
        {
            if (targetPosition.sqrMagnitude > 0.1f)
            {
                // Acceleration
                speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
                return targetPosition * speed * Time.deltaTime; 
            }
            else
            {
                // Deceleration
                horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
                return horizontalVelocity * Time.deltaTime;
            }
        }

        public static float CalculateZoomLevel(float currentHeight, float inputValue, float minHeight, 
            float maxHeight, float stepSize)
        {
            float newHeight = currentHeight + inputValue * stepSize;
            return Mathf.Clamp(newHeight, minHeight, maxHeight);
        }
    }
}