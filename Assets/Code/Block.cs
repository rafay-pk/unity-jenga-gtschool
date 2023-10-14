using System;
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
        public static Grade LastSelectedBlockGrade { get; private set; } = Grade.Null;
        public static UnityEvent BlockSelected = new();
        private static SphericalCameraController cameraController;
        private static InformationPanel informationPanel;
        private Outline outline;
        private Rigidbody rigidB;
        private void OnEnable()
        {
            cameraController ??= FindObjectOfType<SphericalCameraController>();
            cameraController.radiusThreshold.AddListener(DisableOutlineDelayed);
            informationPanel ??= FindObjectOfType<InformationPanel>();
            outline = GetComponent<Outline>();
            rigidB = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            cameraController.radiusThreshold.RemoveListener(DisableOutlineDelayed);
        }

        private void DisableOutlineDelayed() => Invoke(nameof(DisableOutline), 0.5f);
        private void DisableOutline()
        {
            outline.enabled = false;
        }
        public void EnablePhysics()
        {
            if (!rigidB) return;
            rigidB.isKinematic = false;
            rigidB.useGravity = true;
        }
        public void DisablePhysics()
        {
            if (!rigidB) return;
            rigidB.isKinematic = true;
            rigidB.useGravity = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                BlockSelected.Invoke();
                FindObjectsOfType<Block>().ToList().ForEach(b => b.DisableOutline());
                outline.enabled = true;
                transform.DOShakeScale(0.2f, rigidB ? 30f : 1f, 10);
                cameraController.MoveController(transform.position);
                cameraController.ResetOffset();
                LastSelectedBlockGrade = standardDataPoint.grade;
                var objectPosition = eventData.pointerPressRaycast.gameObject.transform.position;
                var pressPosition = eventData.pointerPressRaycast.worldPosition;
                informationPanel.ShowInfo(1.5f * pressPosition - 0.5f * objectPosition,
                    $"{StandardDataPoint.GradeString(standardDataPoint.grade)}: {standardDataPoint.domain}\n" +
                    $"{standardDataPoint.cluster}\n{standardDataPoint.standardID}: {standardDataPoint.standardDescription}");
            }
        }
    }
}
