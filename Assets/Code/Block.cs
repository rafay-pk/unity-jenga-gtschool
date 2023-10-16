using System;
using System.Collections.Generic;
using Code.Camera;
using Code.UserInterface;
using DG.Tweening;
using Plugins.QuickOutline.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Code
{
    public class Block : MonoBehaviour, IPointerClickHandler
    {
        [Header("Asset References")]
        [SerializeField] private AudioClip glassSFX, woodSFX, stoneSFX;
        [SerializeField] private List<AudioClip> tapSFX, slideSFX;
        [Header("Component References")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Outline outline;
        [SerializeField] private Rigidbody rigidB;
        [Header("Data")]
        public StandardDataPoint standardDataPoint;
        public static Grade LastSelectedBlockGrade { get; private set; } = Grade.Null;
        private static readonly List<Block> blocks = new();
        // Events
        public static UnityEvent BlockSelected { get; }= new();
        
        #region Unity Functions
        private void Awake()
        {
            outline ??= GetComponent<Outline>();
            rigidB ??= GetComponent<Rigidbody>();
            audioSource ??= GetComponent<AudioSource>();
        }
        private void OnEnable()
        {
            SphericalCameraController.Instance.RadiusThreshold.AddListener(DisableOutlineDelayed);
            blocks.Add(this);
        }
        private void OnDisable()
        {
            SphericalCameraController.Instance.RadiusThreshold.RemoveListener(DisableOutlineDelayed);
            blocks.Remove(this);
        }
        private void OnCollisionEnter(Collision other)
        {
            audioSource.PlayOneShot(tapSFX[Random.Range(0, tapSFX.Count)], other.impulse.magnitude * 0.05f);
        }
        private void OnCollisionExit(Collision other)
        {
            if (rigidB.velocity.y > -2f) return;
            audioSource.PlayOneShot(slideSFX[Random.Range(0, slideSFX.Count)], Random.Range(0.1f, 0.2f));
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right) return;
            
            BlockSelected.Invoke();
            LastSelectedBlockGrade = standardDataPoint.grade;
            blocks.ForEach(b=>b.DisableOutline());
            outline.enabled = true;
            SphericalCameraController.Instance.MoveController(transform.position);
            transform.DOShakeScale(0.2f, rigidB ? 30f : 1f);
            audioSource.PlayOneShot(standardDataPoint.mastery switch
            {
                Mastery.NeedToLearn => glassSFX,
                Mastery.Learned => woodSFX,
                Mastery.Mastered => stoneSFX,
                Mastery.Null => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            });
            
            var objectPosition = eventData.pointerPressRaycast.gameObject.transform.position;
            var pressPosition = eventData.pointerPressRaycast.worldPosition;
            InformationPanel.Instance.ShowInfo(1.5f * pressPosition - 0.5f * objectPosition,
                $"{StandardDataPoint.GradeString(standardDataPoint.grade)}: {standardDataPoint.domain}\n" +
                $"{standardDataPoint.cluster}\n{standardDataPoint.standardID}: {standardDataPoint.standardDescription}");
        }
        #endregion

        #region API
        public void SetPhysicsState(bool interactionEnabled)
        {
            if (!rigidB) return;
            rigidB.isKinematic = !interactionEnabled;
            rigidB.useGravity = interactionEnabled;
        }
        #endregion
        
        #region Private Functions
        private void DisableOutlineDelayed() => Invoke(nameof(DisableOutline), 0.5f);
        private void DisableOutline() => outline.enabled = false;
        #endregion
    }
}
