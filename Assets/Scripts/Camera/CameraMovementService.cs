using UnityEngine;

namespace Camera
{
    public class CameraMovementService : ICameraMovementService
    {
        private readonly Transform _cameraTransform;
        private readonly UnityEngine.Camera _mainCamera;
        private readonly float _horizontalMoveSpeed;
        private readonly float _verticalMoveSpeed;
        private readonly float _zoomSensitivity;
        private readonly float _minCameraDistance;
        private readonly float _maxCameraDistance;
        private readonly float _cameraMoveSensitivity;

        private Vector3 _lastMousePosition;
        private bool _isMoving;
        
        private Vector2 _minBounds;
        private Vector2 _maxBounds;

        public CameraMovementService(Transform cameraTransform, UnityEngine.Camera mainCamera,
            float horizontalMoveSpeed, float verticalMoveSpeed, float zoomSensitivity,
            float minCameraDistance, float maxCameraDistance, float cameraMoveSensitivity)
        {
            _cameraTransform = cameraTransform;
            _mainCamera = mainCamera;
            _horizontalMoveSpeed = horizontalMoveSpeed;
            _verticalMoveSpeed = verticalMoveSpeed;
            _zoomSensitivity = zoomSensitivity;
            _minCameraDistance = minCameraDistance;
            _maxCameraDistance = maxCameraDistance;
            _cameraMoveSensitivity = cameraMoveSensitivity;
        }

        public void OnMoveStart(Vector3 position)
        {
            _lastMousePosition = position;
            _isMoving = true;
        }

        public void UpdateCameraBounds(Vector2 minBounds, Vector2 maxBounds)
        {
            _minBounds = minBounds;
            _maxBounds = maxBounds;
        }
        public void OnMoveEnd(Vector3 position)
        {
            _isMoving = false;
        }

        public void UpdateMovement()
        {
            if (!_isMoving) return;

            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 deltaMousePosition = currentMousePosition - _lastMousePosition;

            if (deltaMousePosition.magnitude > 0.01f)
            {
                Vector3 moveDirection = new Vector3(deltaMousePosition.x, deltaMousePosition.y, 0).normalized * -1;
                float moveAmplifier = _cameraMoveSensitivity * _mainCamera.orthographicSize;

                Vector3 newPosition = _cameraTransform.position + new Vector3(
                    moveDirection.x * _horizontalMoveSpeed * moveAmplifier,
                    moveDirection.y * _verticalMoveSpeed * moveAmplifier,
                    0
                ) * Time.deltaTime;
                
                newPosition.x = Mathf.Clamp(newPosition.x, _minBounds.x, _maxBounds.x);
                newPosition.y = Mathf.Clamp(newPosition.y, _minBounds.y, _maxBounds.y);

                _cameraTransform.position = newPosition;

                _lastMousePosition = currentMousePosition;
            }
        }

        public void Zoom(float zoomDelta)
        {
            var orthographicSize = _mainCamera.orthographicSize;
            orthographicSize -= zoomDelta * _zoomSensitivity * Time.deltaTime;
            _mainCamera.orthographicSize = orthographicSize;
            _mainCamera.orthographicSize =
                Mathf.Clamp(orthographicSize, _minCameraDistance, _maxCameraDistance);
        }
    }
}