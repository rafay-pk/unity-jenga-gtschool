using Plugins._3DModelViewer.Code.Camera;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins._3DModelViewer.Code.Sample
{
    public class UIValues : MonoBehaviour
    {
        [Header("Scene References")] 
        [SerializeField] private SphericalCameraController cam;
        [SerializeField] private TMP_Text AzimuthTMPText;
        [SerializeField] private TMP_Text AltitudeTMPText;
        [SerializeField] private TMP_Text RadiusTMPText;
        [SerializeField] private TMP_Text OffsetTMPText;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button zoomPanButton;

        private void Awake()
        {
            Application.targetFrameRate = 120;
        }
        
        private void Start()
        {
            resetButton.onClick.AddListener(cam.ResetCamera);
            zoomPanButton.onClick.AddListener(() =>
            {
                cam.ZoomPanToggle();
                zoomPanButton.GetComponentInChildren<TMP_Text>().text = cam.GetZoomPanToggle() ? "To Pan" : "To Zoom";
            });
        }

        private void Update()
        {
            AzimuthTMPText.text = "Azimuth: " + cam.GetAzimuth();
            AltitudeTMPText.text = "Altitude: " + cam.GetAltitude();
            RadiusTMPText.text = "Radius: " + cam.GetRadius();
            OffsetTMPText.text = "Offset: " + cam.GetOffset();
        }
    }
}
