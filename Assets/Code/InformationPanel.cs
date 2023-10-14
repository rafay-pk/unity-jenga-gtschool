using DG.Tweening;
using Plugins._3DModelViewer.Code.Camera;
using TMPro;
using UnityEngine;

namespace Code
{
    public class InformationPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text infoText;
        private Transform cam;
        private bool stopRotating;
        private Tween rotationTween;
        private void Start()
        {
            cam = Camera.main.transform;
            var controller = cam.GetComponentInChildren<SphericalCameraController>();
            controller.userRotating.AddListener(CancelRotation);
            controller.radiusThreshold.AddListener(HideInfoPanel);
        }
        private void Update()
        {
            if (stopRotating) return;
            transform.rotation = cam.rotation;
        }
        public void ShowInfo(Vector3 position, string info)
        {
            stopRotating = true;
            transform.position = position;
            transform.rotation = Quaternion.Euler(-90f, 0f,0f);
            infoText.enabled = true;
            rotationTween = transform.DORotate(cam.rotation.eulerAngles, 1f)
                .SetEase(Ease.OutExpo).OnComplete(() => stopRotating = false);
            infoText.text = info;
        }
        private void CancelRotation()
        {
            rotationTween?.Kill();
            stopRotating = false;
        }
        private void HideInfoPanel()
        {
            if (stopRotating) return;
            stopRotating = true;
            transform.DORotate(-cam.rotation.eulerAngles, 0.5f).SetEase(Ease.InExpo)
                .OnComplete(() => infoText.enabled = false);
        }
    }
}
