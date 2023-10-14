using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class HelpScreen : MonoBehaviour
    {
        [SerializeField] private Button helpButton, closeButton;
        [SerializeField] private RectTransform screenParent, panel;
        private void OnEnable()
        {
            helpButton.onClick.AddListener(OpenMenu);
            closeButton.onClick.AddListener(CloseMenu);
        }
        private void OpenMenu()
        {
            screenParent.DOAnchorPosY(panel.sizeDelta.y, 2f).SetEase(Ease.InOutSine);
        }
        private void CloseMenu()
        {
            screenParent.DOAnchorPosY(0f, 1.5f).SetEase(Ease.InOutSine);
        }
        private void OnDisable()
        {
            helpButton.onClick.RemoveListener(OpenMenu);
            closeButton.onClick.RemoveListener(CloseMenu);
        }
    }
}
