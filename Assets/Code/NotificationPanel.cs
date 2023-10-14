using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class NotificationPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform notificationPanel;
        [SerializeField] private TMP_Text notificationText;
        [SerializeField] private Image panelImage;
        private Sequence notificationSequence;

        private void Awake()
        {
            panelImage ??= notificationPanel.GetComponent<Image>();
            notificationSequence = DOTween.Sequence();
            notificationSequence.Append(notificationPanel.DOAnchorPosX(-50f, 0.5f));
            notificationSequence.AppendInterval(1f);
            notificationSequence.Append(notificationPanel.DOAnchorPosY(200f, 1f));
            notificationSequence.Join(panelImage.DOFade(0f, 1f));
            notificationSequence.Join(notificationText.DOFade(0f, 1f));
            notificationSequence.Pause();
            notificationSequence.SetAutoKill(false);
        }

        public void ShowNotification(string message)
        {
            notificationText.text = message;
            panelImage.color = Color.white;
            notificationText.color = Color.white;
            notificationPanel.anchoredPosition = new Vector2(500f, 50f);
            notificationSequence.Restart();
        }
    }
}
