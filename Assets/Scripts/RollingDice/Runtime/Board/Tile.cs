using Brkyzdmr.Utils;
using UnityEngine;

namespace RollingDice.Runtime.Board
{
    public class Tile : MonoBehaviour
    {
        public Transform Direction;
        public Renderer ProgressUp;
        public Renderer ProgressDown;
        public Renderer ColorMap;

        private void OnDrawGizmos()
        {
            if (Direction != null)
            {
                Vector3 forwardDirection = Direction.forward;
                Vector3 startPosition = transform.position;
                GizmoUtils.DrawArrow(startPosition, forwardDirection, Color.red, yOffset:0.15f);
            }
        }
    }
}