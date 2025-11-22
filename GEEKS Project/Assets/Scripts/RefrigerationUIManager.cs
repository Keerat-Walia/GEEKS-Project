using UnityEngine;

public class RefrigerationUIManager : MonoBehaviour
{
    // ⭐ Global Flag: Read by other scripts to know when the game is over.
    public static bool refrigerationGameComplete = false;

    [Header("Pop-up UI Panels")]
    public GameObject introPanel;
    public GameObject chargeExplanationPanel; // Used for Level 1 (Maximize P/T)
    public GameObject fanExplanationPanel;    // Used for Level 2 (Minimize P/T)
    public GameObject congratulationsPanel;

    private bool isChargeUpObjectiveComplete = false;
    private bool isChargeDownObjectiveComplete = false;

    // ⭐ FIX: Tracks if the introduction is dismissed and the game has started.
    public bool isGameActive = false;

    [Header("Manager Reference")]
    public RefrigerationCycleManager cycleManager;

    // Tolerance to hit the target pressure
    private const float TARGET_TOLERANCE = 1.0f;

    // --- Initialization ---
    void Start()
    {
        // Reset the global state and local flags
        refrigerationGameComplete = false;
        isGameActive = false;
        isChargeUpObjectiveComplete = false;
        isChargeDownObjectiveComplete = false;

        // Ensure panels are correctly set on start
        ShowPanel(introPanel);
        HidePanel(chargeExplanationPanel);
        HidePanel(fanExplanationPanel);
        HidePanel(congratulationsPanel);

        // Ensure the cycle manager starts at the nominal 0.5/0.5 state
        if (cycleManager != null)
        {
            cycleManager.refrigerantCharge = 0.5f;
            cycleManager.SetFanSpeed(0.5f);
        }

        if (cycleManager == null)
        {
            Debug.LogError("UIManager is missing reference to the RefrigerationCycleManager! Drag the manager into the Inspector.");
        }
    }

    // --- Helper Methods to Show/Hide Panels ---
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
        HidePanel(introPanel);

        // ⭐ FIX: Activate the game state
        isGameActive = true;

        // LEVEL 1 SETUP: Start at Nominal 0.5/0.5 (Set in Start method)
        // Player is instructed to increase both Charge and Fan Speed to 1.0.

        ShowPanel(chargeExplanationPanel); // Start the Maximize P/T task
    }

    // --- Pop-up 2: Level 1 Check (Maximize P/T) ---
    public void CheckLevelOneObjective(float currentPressure, float targetPressure)
    {
        if (!isGameActive || isChargeUpObjectiveComplete) return;

        // Check if pressure is >= the target MAX pressure (135.0 kPa)
        if (currentPressure >= targetPressure - TARGET_TOLERANCE)
        {
            isChargeUpObjectiveComplete = true;
            Debug.Log("Level 1 Complete! Starting Minimize Task.");

            HidePanel(chargeExplanationPanel);

            // ⭐ LEVEL 2 SETUP: Set system to MAX state (Charge=1.0, Fan=1.0)
            if (cycleManager != null)
            {
                cycleManager.SetFanSpeed(1.0f);
                cycleManager.refrigerantCharge = 1.0f;
            }

            ShowPanel(fanExplanationPanel); // Start the Minimize P/T task
        }
    }

    // --- Pop-up 3: Level 2 Check (Minimize P/T) ---
    public void CheckLevelTwoObjective(float currentPressure, float targetPressure)
    {
        if (!isChargeUpObjectiveComplete || isChargeDownObjectiveComplete) return;

        // Check if pressure is <= the target MIN pressure (30.0 kPa)
        if (currentPressure <= targetPressure + TARGET_TOLERANCE)
        {
            isChargeDownObjectiveComplete = true;
            Debug.Log("Level 2 Complete! Showing Congratulations.");

            HidePanel(fanExplanationPanel);
            ShowPanel(congratulationsPanel);

            // ⭐ Final Action: Set the Global Variable to TRUE
            refrigerationGameComplete = true;
        }
    }
}