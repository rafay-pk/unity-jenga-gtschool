using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class HelpPanel : MonoBehaviour
    {
        [SerializeField] private Button helpButton, closeButton;
        [SerializeField] private RectTransform screenParent, panel;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip slideInSFX, slideOutSFX;

        private void Awake()
        {
            audioSource ??= GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            helpButton.onClick.AddListener(OpenMenu);
            closeButton.onClick.AddListener(CloseMenu);
        }
        private void OnDisable()
        {
            helpButton.onClick.RemoveListener(OpenMenu);
            closeButton.onClick.RemoveListener(CloseMenu);
        }
        private void OpenMenu()
        {
            screenParent.DOAnchorPosY(panel.sizeDelta.y, 2f).SetEase(Ease.InOutSine);
            audioSource.PlayOneShot(slideInSFX);
        }
        private void CloseMenu()
        {
            screenParent.DOAnchorPosY(0f, 1.5f).SetEase(Ease.InOutSine);
            audioSource.PlayOneShot(slideOutSFX);
        }
    }
}
