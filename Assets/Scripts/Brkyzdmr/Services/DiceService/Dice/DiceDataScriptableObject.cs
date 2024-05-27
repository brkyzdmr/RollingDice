using System.Collections.Generic;
using UnityEngine;

namespace Brkyzdmr.Services.DiceService
{
    [CreateAssetMenu(fileName = "DiceData", menuName = "RollingDice/Dice Data")]
    public class DiceDataScriptableObject : ScriptableObject
    {
        public List<DiceFaceRotation> faceRotations;

        [System.Serializable]
        public struct DiceFaceRotation
        {
            public DiceValue value;
            public List<Vector3> relativeRotations;
        }
    }
}