using System.Collections;
using System.Linq;
using DG.Tweening;
using Plugins._3DModelViewer.Code.Camera;
using Plugins.QuickOutline.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Code
{
    public class Block : MonoBehaviour, IPointerClickHandler
    {
        public StandardDataPoint standardDataPoint;
        private static SphericalCameraController cameraController;
        private static InformationPanel _informationPanel;
        private Outline outline;
        private void OnEnable()
        {
            cameraController ??= FindObjectOfType<SphericalCameraController>();
            cameraController.radiusThreshold.AddListener(DisableOutlineDelayed);
            _informationPanel ??= FindObjectOfType<InformationPanel>();
            outline = GetComponent<Outline>();
        }
        private void DisableOutlineDelayed() => Invoke(nameof(DisableOutline), 0.5f);
        private void DisableOutline() => outline.enabled = false;
        private static void DisableAllOutlines() => FindObjectsOfType<Block>().ToList().ForEach(b => b.DisableOutline());
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                DisableAllOutlines();
                outline.enabled = true;
                transform.DOShakeScale(0.2f, randomnessMode: ShakeRandomnessMode.Harmonic);
                cameraController.MoveController(transform.position);
                cameraController.ResetOffset();
                var objectPosition = eventData.pointerPressRaycast.gameObject.transform.position;
                var pressPosition = eventData.pointerPressRaycast.worldPosition;
                _informationPanel.ShowInfo(1.5f * pressPosition - 0.5f * objectPosition,
                    $"{StandardDataPoint.GradeString(standardDataPoint.grade)}: {standardDataPoint.domain}\n" +
                    $"{standardDataPoint.cluster}\n{standardDataPoint.standardID}: {standardDataPoint.standardDescription}");
            }
        }
    }
}
