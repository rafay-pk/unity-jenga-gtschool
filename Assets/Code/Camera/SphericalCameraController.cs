using Code.Tools;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Camera
{
    public class SphericalCameraController : Singleton<SphericalCameraController>
    {
        [Header("Asset References")]
        [SerializeField] private CameraConfig config;

        [Header("Events")]
        public UnityEvent UserRotating, RadiusThreshold;
        
        private Transform trans, parent;
        private Tween moveTween, zoomTween;
        private bool pauseRotation;
        private float radius, azimuth, altitude;
        private Vector3 offset;
        
        #region Unity Functions
        private void Awake()
        {
            trans = transform;
            parent = trans.parent;
            pauseRotation = true;
            ResetCamera();
        }
        private void Update()
        {
            HandleRotation();
            HandleZoom();
            HandlePan();
            UpdateCameraView();
        }
        #endregion
        
        #region API
        public void MoveController(Vector3 value)
        {
            parent.DOMove(value, 1f).SetEase(Ease.OutExpo);
            DOTween.To(() => offset, x => offset = x, Vector3.zero, 0.1f);
            zoomTween = DOTween.To(() => radius, x => radius = x, config.defaultZoomedInValue, 1f)
                .SetEase(Ease.OutExpo);
        }
        public void ResetCamera()
        {
            azimuth = -Mathf.PI/2;
            altitude = 0.5f;
            offset = Vector3.zero;
            radius = config.defaultZoom;
            UpdateCameraView();
        }
        #endregion

        #region Private Functions
        private void HandleRotation()
        {
            if (Input.GetMouseButtonDown(0))
                pauseRotation = false;
            if (Input.GetMouseButtonUp(0))
                pauseRotation = true;
            
            if (pauseRotation) return;
            var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (mouseDelta == Vector2.zero) return;
            mouseDelta *= config.rotationSpeed;
            
            azimuth = (azimuth - mouseDelta.x).SphericalClamp();
            altitude = (altitude - mouseDelta.y).SphericalClamp().Clamp(config.rotationAltitudeLimits);
            UserRotating.Invoke();
        }
        private void HandleZoom()
        {
            var scroll = Input.mouseScrollDelta.y;
            if (scroll == 0f) return;
            zoomTween?.Kill();
            
            radius = (radius - scroll * config.zoomSpeed).Clamp(config.zoomLimits);
            if (radius > 10f) RadiusThreshold.Invoke();
        }
        private void HandlePan()
        {
            if (!Input.GetKey(KeyCode.Mouse2)) return;
            
            var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (mouseDelta == Vector2.zero) return;

            offset = Vector3.ClampMagnitude(
                offset - (mouseDelta.x * trans.right + mouseDelta.y * trans.up) * (config.panSpeed * radius), config.panLimit);
        }
        private void UpdateCameraView()
        {
            trans.localPosition = offset + radius * new Vector3(
                Mathf.Cos(altitude) * Mathf.Cos(azimuth),
                Mathf.Sin(altitude),
                Mathf.Cos(altitude) * Mathf.Sin(azimuth)
            );;
            trans.LookAt(parent.position + offset);
        }
        #endregion
    }

    public static class MathematicalFunctions
    {
        public static float Clamp(this float value, Vector2 limits)
        {
            return value < limits.x ? limits.x : value > limits.y ? limits.y : value;
        }
        public static float SphericalClamp(this float angle)
        {
            return (angle + Mathf.PI) % (2 * Mathf.PI) - Mathf.PI;
        }
    }
}
