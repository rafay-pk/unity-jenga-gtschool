using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class AttributionPanel : MonoBehaviour
    {
        [SerializeField] private Button attributionButton, closeButton;
        [SerializeField] private RectTransform attributionPanel, panelParent;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip slideInSFX, slideOutSFX;
        private void OnEnable()
        {
            attributionButton.onClick.AddListener(ShowAttributionPanel);
            closeButton.onClick.AddListener(HideAttributionPanel);
        }
        private void OnDisable()
        {
            attributionButton.onClick.RemoveListener(ShowAttributionPanel);
            closeButton.onClick.RemoveListener(HideAttributionPanel);
        }
        private void ShowAttributionPanel()
        {
            panelParent.DOAnchorPosX(-attributionPanel.sizeDelta.x, 2f).SetEase(Ease.InOutSine);
            audioSource.PlayOneShot(slideInSFX);
        }
        private void HideAttributionPanel()
        {
            panelParent.DOAnchorPosX(0f, 1.5f).SetEase(Ease.InOutSine);
            audioSource.PlayOneShot(slideOutSFX);
        }
    }
}
