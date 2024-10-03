using UnityEngine;

namespace Camera
{
    public static class CameraExtensions
    {
        public static Vector3 ConvertToScreenCoordinates(this UnityEngine.Camera camera, Vector3 position)
        {
            if (position.x >= 0 && position.x <= Screen.width &&
                position.y >= 0 && position.y <= Screen.height && 
                Mathf.Approximately(position.z, 0))
            {
                return position;
            }
            
            Vector3 screenPoint = camera.WorldToScreenPoint(position);
            screenPoint.z = 0;

            return screenPoint;
        }
    }
}