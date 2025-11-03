using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup : MonoBehaviour
{
    public GameObject PopupPanel;      // Reference to the popup panel
    public Button Accept;        // Reference to the "Accept" button
    public TextMeshProUGUI Text;  // Reference to popup text
    public Image Background;         // Left image
    public Image Frame;           // Right image
    public float popupDelay = 3f;      // Delay time before showing popup

    public GameObject PPEPanel;
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
        PopupPanel.SetActive(false);

        if (PPEPanel != null)
            PPEPanel.SetActive(true);
    }
}
