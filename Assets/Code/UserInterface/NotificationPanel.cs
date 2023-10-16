using Code.Tools;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UserInterface
{
    public class NotificationPanel : Singleton<NotificationPanel>
    {
        [Header("Component References")]
        [SerializeField] private RectTransform notificationPanel;
        [SerializeField] private TMP_Text notificationText;
        [SerializeField] private Image panelImage;
        [Header("Data")] 
        [SerializeField] private float waitTime = 1f;
        [SerializeField] private Vector2 resetPosition = new(500f, 50f);
        [SerializeField] private float motion1Position = -50f, motion1Duration = 0.5f;
        [SerializeField] private float motion2Position = 200f, motion2Duration = 1f;
        private Sequence notificationSequence;

        private void Awake()
        {
            panelImage ??= notificationPanel.GetComponent<Image>();
            notificationSequence = DOTween.Sequence();
            notificationSequence.Append(notificationPanel.DOAnchorPosX(motion1Position, motion1Duration));
            notificationSequence.AppendInterval(waitTime);
            notificationSequence.Append(notificationPanel.DOAnchorPosY(motion2Position, motion2Duration));
            notificationSequence.Join(panelImage.DOFade(0f, motion2Duration));
            notificationSequence.Join(notificationText.DOFade(0f, motion2Duration));
            notificationSequence.Pause();
            notificationSequence.SetAutoKill(false);
        }
        public void ShowNotification(string message)
        {
            notificationText.text = message;
            panelImage.color = Color.white;
            notificationText.color = Color.white;
            notificationPanel.anchoredPosition = resetPosition;
            notificationSequence.Restart();
        }
    }
}
