using NUnit.Framework;
using UnityEngine;

namespace Brkyzdmr.Tools.EzTween.Test
{
    public class SequenceTests
    {
        private Transform _transform;
        
        [SetUp]
        public void SetUp()
        {
            var testObj = new GameObject("TestObject");
            testObj.AddComponent<TestMono>();
            _transform = testObj.transform;
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(_transform.gameObject); 
        }
        
        [Test]
        public void Sequence_CorrectlyMoveRotate()
        {
            var position = new Vector3(2, 3, 4);
            _transform.position = position;
            
            var moveTween = _transform.DoMove(new Vector3(0, 10, 0), 3f).SetRelative(true);
            var rotateTween = _transform.DoRotate(new Vector3(0, 360, 0), 3f).OnComplete(() =>
            {
                Assert.AreEqual(position + new Vector3(0, 10, 0), _transform.position);
            });

            var sequence = new Sequence(_transform.GetComponent<TestMono>());
            sequence.Append(moveTween)
                .Append(rotateTween)
                .Play();
        }
    }
}