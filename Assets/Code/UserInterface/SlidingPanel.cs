using UnityEngine;
using UnityEngine.UI;

namespace Code.UserInterface
{
	public abstract class SlidingPanel : MonoBehaviour
	{
		[Header("Asset References")] 
		[SerializeField] private AudioClip slideInSFX;
		[SerializeField] private AudioClip slideOutSFX;
		[Header("Component References")]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private Button openButton, closeButton;
		[SerializeField] protected RectTransform parent, panel;
		[Header("Data")] 
		[SerializeField] protected float slideInDuration = 2f;
		[SerializeField] protected float slideOutDuration = 1.5f;
		
		#region Unity Functions
		protected virtual void Awake()
		{
			audioSource ??= GetComponent<AudioSource>();
		}
		private void OnEnable()
		{
			openButton.onClick.AddListener(OpenPanel);
			closeButton.onClick.AddListener(ClosePanel);
		}
		private void OnDisable()
		{
			openButton.onClick.RemoveListener(OpenPanel);
			closeButton.onClick.RemoveListener(ClosePanel);
		}
		#endregion
		
		#region Private Functions
		private void OpenPanel()
		{
			audioSource.PlayOneShot(slideInSFX);
			SetPanelState(true);
		}
		private void ClosePanel()
		{
			audioSource.PlayOneShot(slideOutSFX);
			SetPanelState(false);
		}
		protected abstract void SetPanelState(bool state);
		#endregion
	}
}