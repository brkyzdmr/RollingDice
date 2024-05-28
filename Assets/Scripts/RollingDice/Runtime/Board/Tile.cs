using System;
using Brkyzdmr.Services.ConfigService;
using Brkyzdmr.Utils;
using UnityEngine;

namespace RollingDice.Runtime.Board
{
    public class Tile : MonoBehaviour
    {
        public ItemData ItemData;
        public Transform Direction;
        public Renderer ProgressUp;
        public Renderer ProgressDown;
        public Renderer ColorMap;

        private void Awake()
        {
            ProgressDown.enabled = false;
            ProgressUp.enabled = false;
        }

        public void SetItemVisual()
        {
            ProgressDown.enabled = false;
            ProgressUp.enabled = true;
            ProgressUp.material = new Material(ProgressUp.material);
            ProgressUp.material.color = ItemData.config.GetColor();
        }
        
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