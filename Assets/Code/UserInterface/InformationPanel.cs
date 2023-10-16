using Code.Camera;
using Code.Tools;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.UserInterface
{
    public class InformationPanel : Singleton<InformationPanel>
    {
        [Header("Component References")]
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private Transform cam;
        [Header("Data")] 
        [SerializeField] private Quaternion showRotation = Quaternion.Euler(-90f, 0f,0f);
        [SerializeField] private float showDuration = 1f;
        [SerializeField] private float hideDuration = 0.5f;
        private Transform trans;
        private Tween rotationTween;
        private bool stopRotating;

        #region Unity Functions
        private void Awake()
        {
            cam ??= UnityEngine.Camera.main!.transform;
            trans = transform;
        }
        private void OnEnable()
        {
            SphericalCameraController.Instance.UserRotating.AddListener(CancelRotation);
            SphericalCameraController.Instance.RadiusThreshold.AddListener(HideInfoPanel);
            StackTester.Instance.StackTestStarted.AddListener(HideInfoPanel);
        }
        private void OnDisable()
        {
            SphericalCameraController.Instance.UserRotating.RemoveListener(CancelRotation);
            SphericalCameraController.Instance.RadiusThreshold.RemoveListener(HideInfoPanel);
            StackTester.Instance.StackTestStarted.RemoveListener(HideInfoPanel);
        }

        private void Update()
        {
            if (stopRotating) return;
            transform.rotation = cam.rotation;
        }
        #endregion
        
        #region API
        public void ShowInfo(Vector3 position, string info)
        {
            stopRotating = true;
            trans.position = position;
            trans.rotation = showRotation;
            infoText.enabled = true;
            rotationTween = trans.DORotate(cam.rotation.eulerAngles, showDuration)
                .SetEase(Ease.OutExpo).OnComplete(() => stopRotating = false);
            infoText.text = info;
        }
        #endregion
        
        #region Private Functions
        private void CancelRotation()
        {
            rotationTween?.Kill();
            stopRotating = false;
        }
        private void HideInfoPanel()
        {
            if (stopRotating) return;
            stopRotating = true;
            trans.DORotate(-cam.rotation.eulerAngles, hideDuration).SetEase(Ease.InExpo)
                .OnComplete(() => infoText.enabled = false);
        }
        #endregion
    }
}
