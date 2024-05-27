using UnityEngine;

namespace Brkyzdmr.Utils
{
    public static class GizmoUtils
    {
        public static void DrawArrow(Vector3 startPosition, Vector3 direction, Color color, float length = 0.5f, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float yOffset = 0.5f)
        {
            Gizmos.color = color;

            // Adjust the start position with the Y offset
            startPosition += Vector3.up * yOffset;

            // Draw the main line
            Gizmos.DrawRay(startPosition, direction * length);

            // Calculate arrowhead directions
            Vector3 arrowTip = startPosition + direction * length;
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;

            // Draw arrowhead
            Gizmos.DrawRay(arrowTip, right * arrowHeadLength);
            Gizmos.DrawRay(arrowTip, left * arrowHeadLength);
        }
    }
}