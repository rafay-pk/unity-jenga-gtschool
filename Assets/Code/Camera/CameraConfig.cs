using UnityEngine;

namespace Code.Camera
{
    [CreateAssetMenu(fileName = "CameraConfig_", menuName = "Spherical Coordinate System/Camera Config")]
    public class CameraConfig : ScriptableObject
    {
        public float rotationSpeed = 0.05f;
        public Vector2 rotationAltitudeLimits = new (-0.2f, 0.95f);

        public float defaultZoom = 20f;
        public float zoomSpeed = 1f;
        public float defaultZoomedInValue = 5f;
        public Vector2 zoomLimits = new (5f, 25f);
        
        public float panSpeed = 0.45f;
        public float panLimit = 5f;
    }
}