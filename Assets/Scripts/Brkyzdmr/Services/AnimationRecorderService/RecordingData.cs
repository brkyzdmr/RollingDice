using System.Collections.Generic;
using UnityEngine;

namespace Brkyzdmr.Services.AnimationRecorderService
{
    [System.Serializable]
    public struct RecordingData
    {
        public Rigidbody rb;
        public Vector3 initialPosition;
        public Quaternion initialRotation;
        public List<RecordedFrame> recordedAnimation;

        public RecordingData(Rigidbody rb, Vector3 initialPosition,
            Quaternion initialRotation)
        {
            this.rb = rb;
            this.initialPosition = initialPosition;
            this.initialRotation = initialRotation;
            this.recordedAnimation = new List<RecordedFrame>();
        }
    }
}