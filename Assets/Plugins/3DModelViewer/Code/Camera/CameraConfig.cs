using UnityEngine;

namespace Plugins._3DModelViewer.Code.Camera
{
    [CreateAssetMenu(fileName = "CameraConfig_", menuName = "Spherical Coordinate System/Camera Config")]
    public class CameraConfig : ScriptableObject
    {
        public float defaultRadius = 3f;
        
        public float rotationSpeed = 0.5f;
        public Vector2 rotationAltitudeLimits = new Vector2(-0.2f, 0.95f);

        public float zoomSpeed = 1f;
        public Vector2 zoomLimits = new Vector2(5f, 50f);
        
        public float panSpeed = 1f;
        public float panLimit = 1f;
    }
}