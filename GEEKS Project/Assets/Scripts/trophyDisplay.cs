using UnityEngine;

public class trophyDisplay : MonoBehaviour
{
    // --- Public Variables (Editable in Inspector) ---
    [Header("Trophy Configuration")]
    [Tooltip("The 3D object (trophy) that will appear.")]
    public GameObject trophyObject;

    [Tooltip("The vertical distance above the final position where the trophy starts.")]
    public float initialHeightOffset = 10f;

    // VARIABLE FOR SPECIFYING THE FINAL POINT (Y-axis included in the offset)
    [Tooltip("The final local position offset relative to this GameObject's position.")]
    public Vector3 targetPositionOffset = new Vector3(0, 1.5f, 0);

    // VARIABLE FOR SPECIFYING THE DESCENT SPEED
    [Tooltip("Speed of descent (units per second).")]
    public float descentSpeed = 5f;

    // VARIABLE FOR SPECIFYING THE ROTATION SPEED
    [Tooltip("Rotation speed (degrees per second).")]
    public float rotationSpeed = 120f;

    // --- Private State Variables ---
    private bool isActivated = false;
    private bool isDescending = false;
    private Transform trophyTransform;

    void Start()
    {
        // Basic setup and error checking
        if (trophyObject == null)
        {
            Debug.LogError("NextModuleActivator requires the 'Trophy Object' to be assigned in the Inspector.");
            enabled = false;
            return;
        }

        trophyTransform = trophyObject.transform;

        // Calculate and set the starting position high above the target
        Vector3 initialWorldPos = transform.position + targetPositionOffset + Vector3.up * initialHeightOffset;
        trophyTransform.position = initialWorldPos;

        // Ensure the trophy is hidden at the start
        trophyObject.SetActive(false);
    }

    void Update()
    {
        // 1. Activation Check: Trigger the sequence once when the condition is met
        if (!isActivated && RefrigerationUIManager.refrigerationGameComplete)
        {
            ActivateTrophySequence();
        }

        // 2. Descent Logic (Only runs until it hits the target)
        if (isDescending)
        {
            HandleDescent();
        }

        // 3. Rotation Logic (Runs continuously after activation)
        if (isActivated)
        {
            HandleRotation();
        }
    }

    private void ActivateTrophySequence()
    {
        isActivated = true;
        isDescending = true;

        // Make the trophy visible
        trophyObject.SetActive(true);

        Debug.Log("Refrigeration Game Completed. Trophy sequence started!");
    }

    private void HandleDescent()
    {
        // Calculate the absolute world position for the target
        Vector3 worldTarget = transform.position + targetPositionOffset;

        // Move the trophy towards the target position smoothly at the specified descentSpeed
        trophyTransform.position = Vector3.MoveTowards(
            trophyTransform.position,
            worldTarget,
            descentSpeed * Time.deltaTime
        );

        // Check if the trophy has reached the final position
        // The check uses a tiny epsilon to handle floating point comparisons safely
        if (Vector3.Distance(trophyTransform.position, worldTarget) < 0.001f)
        {
            // Lock the final position exactly to prevent jitter
            trophyTransform.position = worldTarget;
            isDescending = false;
            Debug.Log("Trophy descent complete. Spinning continues.");
        }
    }

    private void HandleRotation()
    {
        // Spin the trophy around its local Y-axis at the specified rotationSpeed
        trophyTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
    }
}