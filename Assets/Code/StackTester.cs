using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Code
{
    public class StackTester : MonoBehaviour
    {
        [SerializeField] private RectTransform testMyStackPanel, testMyStackButtonRect;
        [SerializeField] private Button testMyStackButton, resetButton, earthQuakeButton;
        [SerializeField] private StackCreator stackCreator;
        [SerializeField] private Transform ground;
        [SerializeField] private NotificationPanel notificationPanel;
        public UnityEvent StackTestStarted;
        private readonly Dictionary<Block, TransformData> resetPoint = new();
        private void Awake()
        {
            testMyStackButtonRect ??= testMyStackButton.GetComponent<RectTransform>();
            stackCreator ??= FindObjectOfType<StackCreator>();
            notificationPanel ??= FindObjectOfType<NotificationPanel>();
        }
        private void OnEnable()
        {
            testMyStackButton.onClick.AddListener(TestMyStack);
            earthQuakeButton.onClick.AddListener(ShakePlatform);
            resetButton.onClick.AddListener(ResetStack);
            Block.BlockSelected.AddListener(ResetStack);
        }
        private void OnDisable()
        {
            testMyStackButton.onClick.RemoveListener(TestMyStack);
            earthQuakeButton.onClick.RemoveListener(ShakePlatform);
            resetButton.onClick.RemoveListener(ResetStack);
            Block.BlockSelected.RemoveListener(ResetStack);
        }
        private void TestMyStack()
        {
            var selectedBlockGrade = Block.LastSelectedBlockGrade;
            if (selectedBlockGrade == Grade.Null)
            {
                notificationPanel.ShowNotification("No Block has been selected");
                return;
            }
            StackTestStarted.Invoke();
            testMyStackPanel.DOAnchorPosY(-testMyStackButtonRect.sizeDelta.y, 1.2f).SetEase(Ease.InOutSine);
            resetPoint.Clear();
            foreach (var block in stackCreator.GetBlocks(selectedBlockGrade))
            {
                resetPoint[block] = new TransformData(block.transform);
                if (block.standardDataPoint.mastery == Mastery.NeedToLearn)
                {
                    block.gameObject.SetActive(false);
                    continue;
                }
                block.EnablePhysics();
            }
        }
        private void ResetStack()
        {
            testMyStackPanel.DOAnchorPosY(0f, 0.8f).SetEase(Ease.InOutSine);
            foreach (var block in resetPoint.Keys)
            {
                resetPoint[block].OverrideData(block.transform);
                block.gameObject.SetActive(true);
                block.DisablePhysics();
            }
        }
        private void ShakePlatform()
        {
            ground.DOShakePosition(1f, 0.1f);
        }
    }
    public readonly struct TransformData
    {
        private readonly Vector3 position;
        private readonly Quaternion rotation;
        // private readonly Vector3 scale;
        public TransformData(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            // scale = transform.localScale;
        }
        public void OverrideData(Transform transform)
        {
            transform.DOMove(position, 0.5f);
            transform.DORotateQuaternion(rotation, 0.5f);
            // transform.DOPunchScale(scale, 0.5f,5);
        }
    }
}
