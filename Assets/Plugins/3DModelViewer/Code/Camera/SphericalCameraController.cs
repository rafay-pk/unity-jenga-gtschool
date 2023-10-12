using UnityEngine;

namespace Plugins._3DModelViewer.Code.Camera
{
    public class SphericalCameraController : MonoBehaviour
    {
        #region Setup
        [Header("Asset References")]
        [SerializeField] private CameraConfig config;

        private Transform trans;
        private Vector3 offset;
        private float radius, azimuth, altitude;
        private float previousFingersDistance;
        private bool pauseRotation;
        private bool zoomOrPan = true;
        #endregion
        
        #region Unity Functions
        private void Awake()
        {
            trans = transform;
            ResetCamera();
        }
        private void Update()
        {
            #if UNITY_EDITOR
            DesktopControls();
            #elif UNITY_IOS || UNITY_ANDROID
            switch (Input.touchCount)
            {
                case 0: return;
                case 1: HandleRotationMobile(); break;
                case 2: if (zoomOrPan) HandleZoomMobile(); else HandlePanMobile(); break;
            }
            #else
            DesktopControls();
            #endif
            UpdateCameraView();
        }
        private void DesktopControls()
        {
            if (!HandlePanDesktop())
                HandleRotationDesktop();
            HandleZoomDesktop();
        }
        #endregion
        
        #region API
        public float GetAzimuth() => azimuth;
        public float GetAltitude() => altitude;
        public float GetRadius() => radius;
        public Vector3 GetOffset() => offset;
        public void SetOffset(Vector3 value) => offset = value; 
        public void ZoomPanToggle() => zoomOrPan = !zoomOrPan;
        public bool GetZoomPanToggle() => zoomOrPan;
        public void ZoomSlider(float normalized_value) => SetZoom(normalized_value * config.zoomLimits.y);
        public void PauseRotation() => pauseRotation = true;
        public void ResumeRotation() => pauseRotation = false;
        public void ResetCamera()
        {
            azimuth = 0f;
            altitude = 0f;
            offset = Vector3.zero;
            radius = config.defaultRadius;
            UpdateCameraView();
        }
        #endregion

        #region Private Functions
        private void HandleRotationDesktop()
        {
            if (pauseRotation) return;
            var _mouse_delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (_mouse_delta == Vector2.zero) return;
            SetRotation(_mouse_delta * (config.rotationSpeed * 0.5f));
        }
        private void HandleZoomDesktop()
        {
            var _scroll = Input.mouseScrollDelta.y;
            if (_scroll == 0f) return;
            SetZoom(radius - _scroll * config.zoomSpeed);
        }
        private bool HandlePanDesktop()
        {
            var _scroll_btn = Input.GetKey(KeyCode.Mouse2);
            var _mouse_held = Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1);

            if (!(_scroll_btn || _mouse_held)) return false;
            
            var _mouse_delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (_mouse_delta == Vector2.zero) return false;

            SetPan(_mouse_delta * 60f);
            return true;
        }
        private void HandleRotationMobile()
        {
            if (pauseRotation) return;
            var _finger1 = Input.GetTouch(0);
            if (_finger1.phase != TouchPhase.Moved) return;
            SetRotation(_finger1.deltaPosition * (Time.deltaTime * config.rotationSpeed));
        }
        private void HandleZoomMobile()
        {
            var _finger1 = Input.GetTouch(0);
            var _finger2 = Input.GetTouch(1);

            if (_finger1.phase != TouchPhase.Moved || _finger2.phase != TouchPhase.Moved) return;
            PauseRotation();

            var _distance = Vector2.Distance(_finger1.position, _finger2.position);
            var _direction = _distance > previousFingersDistance ? 1 : _distance < previousFingersDistance ? -1 : 0;
            var _value = radius - _direction * config.zoomSpeed;

            SetZoom(_value);
            ResumeRotation();
            
            previousFingersDistance = _distance;
        }
        private void HandlePanMobile()
        {
            var _finger1 = Input.GetTouch(0);
            var _finger2 = Input.GetTouch(1);

            if (_finger1.phase != TouchPhase.Moved || _finger2.phase != TouchPhase.Moved) return;
            PauseRotation();

            var _delta = _finger1.deltaPosition + _finger2.deltaPosition;

            SetPan(_delta);
            ResumeRotation();
        }
        private void UpdateCameraView()
        {
            var _spherical_position = new Vector3(
                radius * Mathf.Cos(altitude) * Mathf.Cos(azimuth),
                radius * Mathf.Sin(altitude),
                radius * Mathf.Cos(altitude) * Mathf.Sin(azimuth)
            );
            trans.localPosition = offset + _spherical_position;
            trans.LookAt(offset);
        }
        private void SetRotation(Vector2 delta)
        {
            azimuth = ClampAngle(azimuth - delta.x);
            altitude = Clamp(ClampAngle(altitude - delta.y), config.rotationAltitudeLimits);
        }
        private void SetZoom(float value) => radius = Clamp(value, config.zoomLimits);
        private void SetPan(Vector2 delta) => offset = Vector3.ClampMagnitude(offset - (delta.x * trans.right + 
            delta.y * trans.up) * (config.panSpeed * 0.0015f), config.panLimit);
        private static float ClampAngle(float angle) => (angle + Mathf.PI) % (2 * Mathf.PI) - Mathf.PI;
        private static float Clamp(float value, Vector2 limits) =>
            value < limits.x ? limits.x : value > limits.y ? limits.y : value;
        #endregion
    }
}
