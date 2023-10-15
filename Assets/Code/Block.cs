using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Plugins._3DModelViewer.Code.Camera;
using Plugins.QuickOutline.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Code
{
    public class Block : MonoBehaviour, IPointerClickHandler
    {
        public StandardDataPoint standardDataPoint;
        public static Grade LastSelectedBlockGrade { get; private set; } = Grade.Null;
        public static readonly UnityEvent BlockSelected = new();
        private static SphericalCameraController cameraController;
        private static InformationPanel informationPanel;
        private static readonly List<Block> blocks = new();
        [SerializeField] private Outline outline;
        [SerializeField] private Rigidbody rigidB;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip glassSFX, woodSFX, stoneSFX;
        [SerializeField] private List<AudioClip> tapSFX, slideSFX;
        private void OnEnable()
        {
            cameraController ??= FindObjectOfType<SphericalCameraController>();
            cameraController.radiusThreshold.AddListener(DisableOutlineDelayed);
            informationPanel ??= FindObjectOfType<InformationPanel>();
            outline ??= GetComponent<Outline>();
            rigidB ??= GetComponent<Rigidbody>();
            audioSource ??= GetComponent<AudioSource>();
            blocks.Add(this);
        }
        private void OnDisable()
        {
            cameraController.radiusThreshold.RemoveListener(DisableOutlineDelayed);
            blocks.Remove(this);
        }
        private void OnCollisionEnter(Collision other)
        {
            audioSource.PlayOneShot(tapSFX[Random.Range(0, tapSFX.Count)], other.impulse.magnitude * 0.05f);
        }

        private void OnCollisionExit(Collision other)
        {
            if (rigidB.velocity.y > -2f) return;
            print("triggered");
            audioSource.PlayOneShot(slideSFX[Random.Range(0, slideSFX.Count)], Random.Range(0.05f, 0.3f));
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
                audioSource.PlayOneShot(standardDataPoint.mastery switch
                {
                    Mastery.NeedToLearn => glassSFX,
                    Mastery.Learned => woodSFX,
                    Mastery.Mastered => stoneSFX,
                    Mastery.Null => throw new ArgumentOutOfRangeException(),
                    _ => throw new ArgumentOutOfRangeException()
                });
                blocks.ForEach(b=>b.DisableOutline());
                // FindObjectsOfType<Block>().ToList().ForEach(b => b.DisableOutline());
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
