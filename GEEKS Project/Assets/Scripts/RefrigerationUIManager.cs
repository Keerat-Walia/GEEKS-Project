using UnityEngine;

public class RefrigerationUIManager : MonoBehaviour
{
    [Header("Pop-up UI Panels")]
    public GameObject introPanel;
    public GameObject chargeExplanationPanel;
    public GameObject fanExplanationPanel;
    public GameObject congratulationsPanel;

    private bool isChargeObjectiveComplete = false;
    private bool isFanObjectiveComplete = false;

    // --- Initialization ---
    void Start()
    {
        // 1. Show only the Intro Panel on start.
        ShowPanel(introPanel);
        HidePanel(chargeExplanationPanel);
        HidePanel(fanExplanationPanel);
        HidePanel(congratulationsPanel);
    }

    private void ShowPanel(GameObject panel)
    {
        if (panel != null) panel.SetActive(true);
    }

    private void HidePanel(GameObject panel)
    {
        if (panel != null) panel.SetActive(false);
    }

    // --- Pop-up 1: Introduction (Disappears on Continue) ---
    public void OnIntroContinueClicked()
    {
        // Hide Intro, show the first task explanation.
        HidePanel(introPanel);
        ShowPanel(chargeExplanationPanel);
    }

    // --- Pop-up 2: Charge Explanation (Disappears when P/T is Maximized) ---
    public void CheckChargeObjective(float currentPressure, float targetPressure)
    {
        if (isChargeObjectiveComplete) return;

        // Use a small tolerance for floating point comparison (e.g., 0.1 kPa)
        if (currentPressure >= targetPressure - 0.1f)
        {
            isChargeObjectiveComplete = true;
            Debug.Log("Charge Objective Complete! Starting Fan Task.");
            HidePanel(chargeExplanationPanel);
            ShowPanel(fanExplanationPanel); // Proceed to the next task explanation
        }
    }

    // --- Pop-up 3: Fan Explanation (Disappears when P/T is Minimised) ---
    public void CheckFanObjective(float currentPressure, float targetPressure)
    {
        if (!isChargeObjectiveComplete || isFanObjectiveComplete) return;

        // Use a small tolerance for floating point comparison (e.g., 0.1 kPa)
        if (currentPressure <= targetPressure + 0.1f)
        {
            isFanObjectiveComplete = true;
            Debug.Log("Fan Objective Complete! Showing Congratulations.");
            HidePanel(fanExplanationPanel);
            ShowPanel(congratulationsPanel); // Show the final screen
        }
    }
}