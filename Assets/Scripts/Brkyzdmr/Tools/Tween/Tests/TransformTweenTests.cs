namespace Brkyzdmr.Tools.EzTween.Test
{
    using NUnit.Framework;
    using UnityEngine;

    public class TransformExtensionTests
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
        public void DoMove_CorrectlyMovesTransform()
        {
            var targetPosition = new Vector3(2, 3, 4);
            var duration = 1.0f;
            _transform.DoMove(targetPosition, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetPosition, _transform.position));
        }
        
        [Test]
        public void DoMove_CorrectlyMovesXTransform()
        {
            var targetPosition = 4f;
            var duration = 1.0f;
            _transform.DoMoveX(targetPosition, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetPosition, _transform.position));
        }
        
        [Test]
        public void DoMove_CorrectlyMovesYTransform()
        {
            var targetPosition = 4f;
            var duration = 1.0f;
            _transform.DoMoveY(targetPosition, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetPosition, _transform.position));
        }
        
        [Test]
        public void DoMove_CorrectlyMovesZTransform()
        {
            var targetPosition = 4f;
            var duration = 1.0f;
            _transform.DoMoveZ(targetPosition, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetPosition, _transform.position));
        }

        [Test]
        public void DoRotate_CorrectlyRotatesTransform()
        {
            var targetRotation = Quaternion.Euler(45, 90, 0);
            var duration = 1.0f;
            _transform.DoRotate(targetRotation, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetRotation, _transform.rotation));
        }
        
        [Test]
        public void DoRotate_CorrectlyRotatesXTransform()
        {
            var targetRotation = 45f;
            var duration = 1.0f;
            _transform.DoRotateX(targetRotation, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetRotation, _transform.rotation));
        }
        
        [Test]
        public void DoRotate_CorrectlyRotatesYTransform()
        {
            var targetRotation = 45f;
            var duration = 1.0f;
            _transform.DoRotateY(targetRotation, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetRotation, _transform.rotation));
        }
        
        [Test]
        public void DoRotate_CorrectlyRotatesZTransform()
        {
            var targetRotation = 45f;
            var duration = 1.0f;
            _transform.DoRotateZ(targetRotation, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetRotation, _transform.rotation));
        }

        [Test]
        public void DoScale_CorrectlyScalesTransform()
        {
            var targetScale = new Vector3(2, 2, 2);
            var duration = 1.0f;
            _transform.DoScale(targetScale, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetScale, _transform.localScale));
        }
        
        [Test]
        public void DoScale_CorrectlyScalesXTransform()
        {
            var targetScale = 2f;
            var duration = 1.0f;
            _transform.DoScaleX(targetScale, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetScale, _transform.localScale));
        }
        
        [Test]
        public void DoScale_CorrectlyScalesYTransform()
        {
            var targetScale = 2f;
            var duration = 1.0f;
            _transform.DoScaleY(targetScale, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetScale, _transform.localScale));
        }
        
        [Test]
        public void DoScale_CorrectlyScalesZTransform()
        {
            var targetScale = 2f;
            var duration = 1.0f;
            _transform.DoScaleZ(targetScale, duration).TweenTo()
                .OnComplete(() => Assert.AreEqual(targetScale, _transform.localScale));
        }
        
        [Test]
        public void SetRelative_CorrectlySetsRelativeMovement()
        {
            _transform.position = new Vector3(1, 2, 3);
            var relativeMovement = new Vector3(1, 0, 0);
            var duration = 1.0f;
            var initialPosition = new Vector3(2, 2, 2);
            _transform.position = initialPosition;

            _transform.DoMove(Vector3.zero, duration).SetRelative(true).TweenTo()
                .OnComplete(() => Assert.AreEqual(initialPosition + relativeMovement, _transform.position));
        }

        [Test]
        public void SetLoops_CorrectlySetsLoopCount()
        {
            _transform.position = new Vector3(1, 2, 3);
            var targetPosition = new Vector3(2, 3, 4);
            var duration = 1.0f;
            var loopCount = 2;

            var initialPosition = _transform.position;
            _transform.DoMove(targetPosition, duration).SetLoops(loopCount).TweenTo()
                .OnComplete(() => Assert.AreEqual(initialPosition, _transform.position));
        }
        
        [Test]
        public void DoMoveDoRotate_CorrectlyRunComplexTween()
        {
            _transform.position = new Vector3(1, 2, 3);

            var initialPosition = _transform.position;
            
            _transform.DoMove(new Vector3(0, 10, 0), 3f)
                .SetEase(Ease.EaseInOut)
                .SetRelative(true)
                .SetDelay(5f)
                .SetLoops(2, Tween.LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _transform.DoRotate(new Vector3(30, 10, 25), 4f)
                        .SetEase(Ease.EaseInOut)
                        .SetRelative(true)
                        .OnUpdate((rot) => Debug.Log("Rot: " + rot))
                        .OnComplete(() => Assert.AreEqual(initialPosition, _transform.position)).TweenTo();
                }).TweenTo();
        }
    }
}