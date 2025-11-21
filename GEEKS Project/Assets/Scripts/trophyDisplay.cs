using UnityEngine;

public class trophyDisplay : MonoBehaviour
{
    // --- Public Variables (Editable in Inspector) ---
    [Header("Trophy Configuration")]
    [Tooltip("The 3D object (trophy) that will appear.")]
    public GameObject trophyObject;

    [Tooltip("The vertical distance above the final position where the trophy starts.")]
    public float initialHeightOffset = 10f;

    [Tooltip("The final local position offset relative to this GameObject's position.")]
    public Vector3 targetPositionOffset = new Vector3(0, 1.5f, 0);

    [Tooltip("Speed of descent (units per second).")]
    public float descentSpeed = 5f;

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

        // 2. Descent and Rotation Logic
        if (isDescending)
        {
            HandleDescent();
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

        // Move the trophy towards the target position smoothly
        trophyTransform.position = Vector3.MoveTowards(
            trophyTransform.position,
            worldTarget,
            descentSpeed * Time.deltaTime
        );

        // Check if the trophy has reached the final position
        if (trophyTransform.position == worldTarget)
        {
            isDescending = false;
            Debug.Log("Trophy descent complete. Spinning indefinitely.");
        }
    }

    private void HandleRotation()
    {
        // Spin the trophy around its local Y-axis at a continuous rate
        trophyTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}