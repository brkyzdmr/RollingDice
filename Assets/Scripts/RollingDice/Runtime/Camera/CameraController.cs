using Brkyzdmr.Helpers;
using UnityEngine;

namespace RollingDice.Runtime.Managers
{
    public class CameraController
    {
        public Camera camera { get; }
        
        private readonly Transform _cameraTransform;
        private readonly Transform _cameraParentTransform;

        private readonly Vector2 _horizontalPositionLimits;
        private Vector3 _horizontalVelocity;
        private float _speed;
        private Vector3 _lastPosition;

        public CameraController(Camera camera, Vector2 horizontalPositionLimits)
        {
            this.camera = camera;
            _cameraTransform = camera.transform;
            _cameraParentTransform = _cameraTransform.parent;
            _horizontalPositionLimits = horizontalPositionLimits;
            _cameraTransform.LookAt(_cameraParentTransform);
            _lastPosition = _cameraParentTransform.position;
        }
        
        private void UpdateVelocity()
        {
            _horizontalVelocity = (_cameraParentTransform.position - _lastPosition) / Time.deltaTime;
            _horizontalVelocity.y = 0f;
            _lastPosition = _cameraParentTransform.position;
        }

        public void MoveCamera(ref Vector3 targetPosition, float maxSpeed, float acceleration, float damping)
        {
            Vector3 movement = CameraHelper.CalculateCameraMovement(
                targetPosition, ref _speed, ref _horizontalVelocity,
                maxSpeed, acceleration, damping);
            
            targetPosition = Vector3.zero;
            _cameraParentTransform.position += movement;
            _cameraParentTransform.position = MathHelper.ClampVectorXZ(_cameraParentTransform.position, 
                _horizontalPositionLimits);
            
            UpdateVelocity();
        }

        public void ZoomCamera(float scrollDelta, float stepSize, float zoomSpeed, float zoomDampening, 
            float minZoomDistance, float maxZoomDistance)
        {
            float zoomHeight = CameraHelper.CalculateZoomLevel(
                _cameraTransform.localPosition.y,
                scrollDelta,
                minZoomDistance,
                maxZoomDistance, stepSize
            );

            MathHelper.SmoothMoveTowardsTarget(
                _cameraTransform,
                _cameraTransform.forward,
                zoomHeight,
                zoomSpeed,
                zoomDampening
            );
        }
    }
}