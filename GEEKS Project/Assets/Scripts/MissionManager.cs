using UnityEngine;

public class MissionManager : MonoBehaviour
{
    // --- Public References (Assign these in the Inspector) ---
    [Header("Pop-up Canvases")]
    [SerializeField] private GameObject _popup1; // "Welcome, Agent..."
    [SerializeField] private GameObject _popup2; // "Before you proceed, you must blend in..."
    [SerializeField] private GameObject _popup3; // "Excellent work, Agent..."

    [Header("3D Interactables")]
    [SerializeField] private GameObject _goggles3D;
    [SerializeField] private GameObject _labCoat3D;
    [SerializeField] private GameObject _door3D;

    // --- State Variables ---
    // 0: Start, 1: Popup 2 active (Equip Gear), 2: Popup 3 active (Go to Door)
    private int _missionStep = 0;

    // Flags to track required clicks
    private bool _gogglesEquipped = false;
    private bool _coatEquipped = false;


    void Start()
    {
        // Ensure only the first pop-up is active at the start
        _popup1.SetActive(true);
        _popup2.SetActive(false);
        _popup3.SetActive(false);

        // Disable interaction on the gear and door initially
        SetGearInteractionEnabled(false);
        if (_door3D.GetComponent<Collider>() != null)
            _door3D.GetComponent<Collider>().enabled = false;
    }

    // A helper function to manage colliders easily
    private void SetGearInteractionEnabled(bool isEnabled)
    {
        // Ensure both gear items have a Collider component
        if (_goggles3D != null && _goggles3D.GetComponent<Collider>() != null)
            _goggles3D.GetComponent<Collider>().enabled = isEnabled;

        if (_labCoat3D != null && _labCoat3D.GetComponent<Collider>() != null)
            _labCoat3D.GetComponent<Collider>().enabled = isEnabled;
    }

    // 1. Called when the 'Continue' button on POP-UP 1 is pressed
    public void OnContinueClicked()
    {
        if (_missionStep == 0)
        {
            // Transition from Pop-up 1 to Pop-up 2
            _popup1.SetActive(false);
            _popup2.SetActive(true);
            _missionStep = 1;

            // Enable the 3D safety gear for interaction
            SetGearInteractionEnabled(true);
        }
    }

    // 2. Called when the Goggles are clicked
    public void OnGogglesClicked()
    {
        if (_missionStep == 1 && !_gogglesEquipped)
        {
            _gogglesEquipped = true;
            Debug.Log("Goggles equipped.");

            // Disable the visual and collider immediately
            _goggles3D.SetActive(false);

            CheckForProgress();
        }
    }

    // 2. Called when the Lab Coat is clicked
    public void OnLabCoatClicked()
    {
        if (_missionStep == 1 && !_coatEquipped)
        {
            _coatEquipped = true;
            Debug.Log("Lab Coat equipped.");

            // Disable the visual and collider immediately
            _labCoat3D.SetActive(false);

            CheckForProgress();
        }
    }

    // Check if all requirements for the current step are met
    private void CheckForProgress()
    {
        if (_gogglesEquipped && _coatEquipped)
        {
            Debug.Log("Both safety items equipped. Proceeding to Pop-up 3.");

            // Transition from Pop-up 2 to Pop-up 3
            _popup2.SetActive(false);
            _popup3.SetActive(true);
            _missionStep = 2;

            // Enable the door interaction
            if (_door3D.GetComponent<Collider>() != null)
                _door3D.GetComponent<Collider>().enabled = true;
        }
    }

    // 3. Called when the 3D door is clicked
    public void OnDoorClicked()
    {
        if (_missionStep == 2)
        {
            // Mission Step 3: Scene transition or next stage
            Debug.Log("Mission infiltration begins! Loading new scene or starting game logic.");
            _popup3.SetActive(false);
            // Example: UnityEngine.SceneManagement.SceneManager.LoadScene("Lab_Interior");
        }
    }
}