using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Popup2 : MonoBehaviour
{
    public GameObject PopupPanel;       // Reference to the second popup panel
    public Button Equip;                // Reference to the "Equip" button
    public TextMeshProUGUI Text;        // Reference to popup text
    public Image Background;            // Left image
    public Image Frame;                 // Right image

    void Start()
    {
        PopupPanel.SetActive(false);   // Start hidden
        Equip.onClick.AddListener(ClosePopup);
    }

    void OnEnable()
    {
        // Optional: update text or images dynamically here if needed
    }

    void ClosePopup()
    {
        PopupPanel.SetActive(false);   // Hide second popup
    }
}

