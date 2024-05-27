using Brkyzdmr.Generics.Singletons;
using Brkyzdmr.Helpers;
using Brkyzdmr.Services;
using Brkyzdmr.Services.InputService;
using UnityEngine;

namespace RollingDice.Runtime.Managers
{
    public class InputManager : PersistentSingleton<SaveManager>
    {
        [Header("Horizontal Translation")]
        [SerializeField] private float maxHorizontalSpeed = 5f;
        [SerializeField] private float horizontalAcceleration = 10f;
        [SerializeField] private float horizontalDamping = 15f;
        [SerializeField] private Vector2 horizontalPositionLimits = new Vector2(-10f, 10f); 

        [Header("Vertical Translation")]
        [SerializeField] private float zoomStepSize = 2f;
        [SerializeField] private float zoomDampening = 7.5f;
        [SerializeField] private float minZoomDistance = 5f;
        [SerializeField] private float maxZoomDistance = 20f;
        [SerializeField] private float zoomSpeed = 2f;

        private IInputService _inputService;
        private CameraController _cameraController;
        private Vector3 _startDrag;
        private Vector3 _targetPosition;

        protected override void Awake()
        {
            _inputService = Services.GetService<IInputService>();
            _cameraController = new CameraController(Camera.main, horizontalPositionLimits);
        }

        private void Update()
        {
            HandleCameraMovement();
            HandleCameraZoom();
        }

        private void HandleCameraMovement()
        {
            if (!_inputService.GetMouseButton(0)) return;
            
            DragCamera();
            _cameraController.MoveCamera(ref _targetPosition, maxHorizontalSpeed, horizontalAcceleration, horizontalDamping);
        }

        private void HandleCameraZoom()
        {
            float scrollDelta = _inputService.GetMouseScrollDelta();
            if (Mathf.Abs(scrollDelta) > 0.1f)
            {
                _cameraController.ZoomCamera(scrollDelta, zoomStepSize, zoomSpeed, zoomDampening, minZoomDistance, maxZoomDistance);
            }
        }

        private void DragCamera()
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = _cameraController.camera.ScreenPointToRay(_inputService.GetMousePosition());

            if (plane.Raycast(ray, out float distance))
            {
                if (_inputService.GetMouseButtonDown(0))
                {
                    _startDrag = ray.GetPoint(distance);
                }
                else
                {
                    _targetPosition += _startDrag - ray.GetPoint(distance);
                }
            }
        }
    }
}