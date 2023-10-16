using System.Collections.Generic;
using Code.Tools;
using Code.UserInterface;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code
{
    public class StackTester : Singleton<StackTester>
    {
        [Header("Asset References")]
        [SerializeField] private AudioClip swooshSFX;
        [Header("Component References")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private RectTransform testMyStackPanel, testMyStackButtonRect;
        [SerializeField] private Button testMyStackButton, resetButton, earthQuakeButton;
        [SerializeField] private Transform ground;
        [Header("Events")]
        public UnityEvent StackTestStarted;
        //[Header("Data")]
        private readonly Dictionary<Block, TransformData> resetPoint = new();
        private Vector3? groundOriginalLocation;
        private int shakeCounter;

        #region Unity Functions
        private void Awake()
        {
            audioSource ??= GetComponent<AudioSource>();
            testMyStackButtonRect ??= testMyStackButton.GetComponent<RectTransform>();
        }
        private void OnEnable()
        {
            testMyStackButton.onClick.AddListener(TestMyStack);
            earthQuakeButton.onClick.AddListener(ShakePlatform);
            resetButton.onClick.AddListener(ResetStackFromUser);
            Block.BlockSelected.AddListener(ResetStack);
        }
        private void OnDisable()
        {
            testMyStackButton.onClick.RemoveListener(TestMyStack);
            earthQuakeButton.onClick.RemoveListener(ShakePlatform);
            resetButton.onClick.RemoveListener(ResetStackFromUser);
            Block.BlockSelected.RemoveListener(ResetStack);
        }
        #endregion

        #region Private Functions
        private void TestMyStack()
        {
            var selectedBlockGrade = Block.LastSelectedBlockGrade;
            if (selectedBlockGrade == Grade.Null)
            {
                NotificationPanel.Instance.ShowNotification("No Block has been selected");
                return;
            }
            StackTestStarted.Invoke();
            testMyStackPanel.DOAnchorPosY(-testMyStackButtonRect.sizeDelta.y, 1.2f).SetEase(Ease.InOutSine);
            resetPoint.Clear();
            foreach (var block in StackCreator.Instance.GetBlocks(selectedBlockGrade))
            {
                resetPoint[block] = new TransformData(block.transform);
                if (block.standardDataPoint.mastery == Mastery.NeedToLearn)
                {
                    block.gameObject.SetActive(false);
                    continue;
                }
                block.SetPhysicsState(true);
            }
        }
        private void ResetStackFromUser()
        {
            audioSource.PlayOneShot(swooshSFX, 0.5f);
            ResetStack();
        }
        private void ResetStack()
        {
            const float tweenDuration = 1f;
            testMyStackPanel.DOAnchorPosY(0f, tweenDuration).SetEase(Ease.InOutSine);
            foreach (var block in resetPoint.Keys)
            {
                resetPoint[block].OverrideData(block.transform, tweenDuration);
                block.gameObject.SetActive(true);
                block.SetPhysicsState(false);
            }
        }
        private void ShakePlatform()
        {
            const float tweenDuration = 1f;
            groundOriginalLocation ??= ground.transform.position;
            ground.DOShakePosition(tweenDuration, 0.1f);
            if (++shakeCounter > 3)
            {
                ground.transform.position = groundOriginalLocation.Value;
                shakeCounter = 0;
            }
        }
        #endregion
    }
    public readonly struct TransformData
    {
        private readonly Vector3 position;
        private readonly Quaternion rotation;
        public TransformData(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
        }
        public void OverrideData(Transform transform, float tweenDuration)
        {
            transform.DOMove(position, tweenDuration);
            transform.DORotateQuaternion(rotation, tweenDuration);
        }
    }
}
