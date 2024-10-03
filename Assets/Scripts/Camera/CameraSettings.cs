using UnityEngine;

namespace Camera
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings")]
    public class CameraSettings : ScriptableObject
    {
        [SerializeField, Range(4f, 20f)] private float _horizontalMoveSpeed;
        [SerializeField, Range(4f, 20f)] private float _verticalMoveSpeed;
        [SerializeField, Range(25f, 250f)] private float _zoomSensitivity;
        [SerializeField, Range(10f, 20f)] private float _maxCameraDistance;
        [SerializeField, Range(1f, 10f)] private float _minCameraDistance;
        [SerializeField, Range(0.01f,0.5f)] private float _cameraMoveSensitivity;
        
        [Header("Bounds Settings")]
        [SerializeField] private Vector2 _offsetMin;
        [SerializeField] private Vector2 _offsetMax;

        public float HorizontalMoveSpeed => _horizontalMoveSpeed;
        public float VerticalMoveSpeed => _verticalMoveSpeed;
        public float ZoomSensitivity => _zoomSensitivity;
        public float MaxCameraDistance => _maxCameraDistance;
        public float MinCameraDistance => _minCameraDistance;
        public float CameraMoveSensitivity => _cameraMoveSensitivity;
        public Vector2 OffsetMin => _offsetMin;
        public Vector2 OffsetMax => _offsetMax;
    }
}