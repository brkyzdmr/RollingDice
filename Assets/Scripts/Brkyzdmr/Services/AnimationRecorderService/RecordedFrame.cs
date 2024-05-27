using UnityEngine;

namespace Brkyzdmr.Services.AnimationRecorderService
{
    [System.Serializable]
    public struct RecordedFrame
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool isContactWithArena;
        public bool isContactWithDice;
        public bool isNotMoving;

        public RecordedFrame(Vector3 position, Quaternion rotation,
            bool isContactWithArena, bool isContactWithDice,
            bool isNotMoving)
        {
            this.position = position;
            this.rotation = rotation;
            this.isContactWithArena = isContactWithArena;
            this.isContactWithDice = isContactWithDice;
            this.isNotMoving = isNotMoving;
        }
    }
}