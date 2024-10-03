using System;
using Connection;
using Events;
using Player;
using Player.ActionHandlers;
using UnityEngine;
using Utils.Singleton;

namespace Camera
{
    public class CameraHolder : DontDestroyMonoBehaviourSingleton<CameraHolder>
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        public UnityEngine.Camera MainCamera => mainCamera;
        
        [SerializeField] private CameraSettings _cameraSettings;

        private ICameraMovementService _cameraMovementService;
        private ClickHandler _clickHandler;
        private BoundsStorage _boundsStorage;

        public override void Awake()
        {
            base.Awake();

            InitializeCameraMovementService();
            InitializeClickHandler();
            InitializeBoundsStorage();
            SubscribeToInputEvents();
        }

        public BoundsStorage GetBoundsStorage()
        {
            return _boundsStorage;
        }
        
        private void InitializeCameraMovementService()
        {
            _cameraMovementService = new CameraMovementService(
                transform,
                mainCamera,
                _cameraSettings.HorizontalMoveSpeed,
                _cameraSettings.VerticalMoveSpeed,
                _cameraSettings.ZoomSensitivity,
                _cameraSettings.MinCameraDistance,
                _cameraSettings.MaxCameraDistance,
                _cameraSettings.CameraMoveSensitivity
            );
        }

        private void InitializeClickHandler()
        {
            _clickHandler = ClickHandler.Instance;
        }

        private void InitializeBoundsStorage()
        {
            _boundsStorage = new BoundsStorage();
            _boundsStorage.OnBoundsUpdated += UpdateCameraBounds;
        }
        
        private void UnsubscribeFromBoundsStorage()
        {
            if (_boundsStorage != null)
            {
                _boundsStorage.OnBoundsUpdated -= UpdateCameraBounds;
            }
        }
        private void Update()
        {
            _cameraMovementService.UpdateMovement();
        }
        
        private void UpdateCameraBounds(Vector2 minBounds, Vector2 maxBounds)
        {
            var minBoundsWithOffset = minBounds + _cameraSettings.OffsetMin;
            var maxBoundsWithOffset = maxBounds + _cameraSettings.OffsetMax;
        
            _cameraMovementService.UpdateCameraBounds(minBoundsWithOffset, maxBoundsWithOffset);
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromInputEvents();
            UnsubscribeFromBoundsStorage();
        }

        private void SubscribeToInputEvents()
        {
            _clickHandler.SubscribeToDragEventHandlers(OnMoveStart,OnMoveEnd);
            _clickHandler.SubscribeToScrollEvent(OnScroll);
        }

        private void UnsubscribeFromInputEvents()
        {
            _clickHandler.UnsubscribeToDragEventHandlers(OnMoveStart,OnMoveEnd);
            _clickHandler.UnsubscribeToScrollEvent(OnScroll);
        }

        private void OnMoveStart(Vector3 position)
        {
            if (PlayerController.PlayerState == PlayerState.Connecting) return;
            
            var screenPosition = MainCamera.ConvertToScreenCoordinates(position);
            _cameraMovementService.OnMoveStart(screenPosition);
        }

        private void OnMoveEnd(Vector3 position)
        {
            var screenPosition = MainCamera.ConvertToScreenCoordinates(position);
            _cameraMovementService.OnMoveEnd(screenPosition);
        }

        private void OnScroll(Vector3 scrollDirection)
        {
            if (PlayerController.PlayerState == PlayerState.Connecting) return;
            _cameraMovementService.Zoom(scrollDirection.y);
        }
    }
}