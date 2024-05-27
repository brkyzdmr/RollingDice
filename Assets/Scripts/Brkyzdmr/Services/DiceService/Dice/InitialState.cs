using UnityEngine;

namespace Brkyzdmr.Services.DiceService
{
    /// <summary>
    /// This is a struct to hold all data needed to initialize the dice
    /// </summary>
    [System.Serializable]
    public struct InitialState
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 force;
        public Vector3 torque;

        public InitialState(Vector3 position, Quaternion rotation,
            Vector3 force, Vector3 torque)
        {
            this.position = position;
            this.rotation = rotation;
            this.force = force;
            this.torque = torque;
        }
    }
}