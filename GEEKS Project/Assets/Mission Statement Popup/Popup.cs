using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    public GameObject PopupPanel;      // Reference to the popup panel
    public Button Accept;        // Reference to the "Accept" button
    public MissionText;  // Reference to popup text
    public Image Background;            // Left image
    public Image Frame;           // Right image
    public float popupDelay = 5f;      // Delay time before showing popup

    void Start()
    {
        PopupPanel.SetActive(false);  // Hide popup at start
        Invoke(nameof(ShowPopup), popupDelay); // Show after delay

            Accept.onClick.AddListener(ClosePopup);
    }

    void ShowPopup()
    {
        PopupPanel.SetActive(true);
    }

    void ClosePopup()
    {
        Destroy(PopupPanel);
    }
}
