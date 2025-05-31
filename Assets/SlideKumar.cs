using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Slider sensitivitySlider;
    public PlayerController playerController;

    void Start()
    {
        if (sensitivitySlider != null && playerController != null)
        {
            // Initialize slider value from controller
            sensitivitySlider.value = playerController.mouseSensitivity;

            // Hook up listener
            sensitivitySlider.onValueChanged.AddListener(playerController.SetMouseSensitivity);
        }
    }

    void OnDestroy()
    {
        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.RemoveAllListeners();
        }
    }
}