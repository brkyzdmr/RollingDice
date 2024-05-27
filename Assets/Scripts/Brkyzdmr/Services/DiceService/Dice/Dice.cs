using UnityEngine;
using UnityEngine.Serialization;

namespace Brkyzdmr.Services.DiceService
{
    public class Dice : MonoBehaviour
    {
        [Header("References")]
        public DiceDataScriptableObject diceDataScriptableObject;
        public GameObject[] faceDetectors;

        [Header("Debug")]
        public int defaultFaceResult = -1;
        public int alteredFaceResult = -1;
        
        public void Reset()
        {
            defaultFaceResult = -1;
            alteredFaceResult = -1;
        }

        /// <summary>
        /// Rotate the dice from the defaultFaceResult to alteredFaceResult
        /// </summary>
        /// <param name="alteredFaceResult"></param>
        public void RotateDice(int alteredFaceResult)
        {
            if (alteredFaceResult <= diceDataScriptableObject.faceRotations.Count - 1)
            {
                this.alteredFaceResult = alteredFaceResult;
                Vector3 rotate = diceDataScriptableObject.faceRotations[defaultFaceResult].relativeRotations[alteredFaceResult];
                transform.Rotate(rotate);
            }
            else
            {
                this.alteredFaceResult = defaultFaceResult;
            }
        }

        /// <summary>
        /// Find the result of the roll, the topmost face of the dice
        /// </summary>
        public int FindFaceResult()
        {
            //Since we have all child objects for each face,
            //We just need to find the highest Y value
            int maxIndex = 0;
            for (int i = 1; i < faceDetectors.Length; i++)
            {
                if (faceDetectors[maxIndex].transform.position.y <
                    faceDetectors[i].transform.position.y)
                {
                    maxIndex = i;
                }
            }
            defaultFaceResult = maxIndex;
            return maxIndex;
        }
    }
}