using UnityEngine;

namespace Camera
{
    public interface ICameraMovementService
    {
        void OnMoveStart(Vector3 position);
        void UpdateCameraBounds(Vector2 minBounds, Vector2 maxBounds);
        void OnMoveEnd(Vector3 position);
        void UpdateMovement();
        void Zoom(float zoomDelta);
    }
}