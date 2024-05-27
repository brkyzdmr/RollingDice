using System;
using Brkyzdmr.Tools.EzTween;
using UnityEngine;

namespace RollingDice.Runtime.Test
{
    public class TestTween : MonoBehaviour
    {
        private void Start()
        {
            // var moveTween = transform.DoMove(new Vector3(0, 10, 0), 3f).SetRelative(true);
            // var rotateTween = transform.DoRotate(new Vector3(0, 360, 0), 3f);
            //
            //
            // var sequence = new Sequence(this);
            // sequence.Append(moveTween)
            //         .Append(rotateTween)
            //         .Play();
            
            // transform.DoJump(new Vector3(10, 0, 0), 5, 1, 3, Ease.Linear).Play();

            Vector3 startPos = transform.position;
            Vector3 jumpApex = startPos + Vector3.up * 5; // Calculate the apex of the jump.
            Vector3 endPos = startPos + new Vector3(10, 0, 0); // Absolute end position.

            var sequence = new Sequence(this); // Assuming the script is on a MonoBehaviour.

            // Upwards jump
            sequence.Append(transform.DoMove(jumpApex, 1)
                .SetRelative(true)
                .SetEase(Ease.EaseInOut));

            // Downwards to destination
            sequence.Append(transform.DoMove(endPos, 1)
                .SetRelative(true)
                .SetEase(Ease.EaseInOut));

            sequence.Play();
            

            // var sequence = new Sequence(this);
            // sequence.Append(heightTween)
            //     .Insert(1, moveTween)
            //     .Play();
        }
    }
}