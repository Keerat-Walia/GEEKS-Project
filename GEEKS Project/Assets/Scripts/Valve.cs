using UnityEngine;
using UnityEngine.Events;

public class Valve : MonoBehaviour
{
    // New variable to hold the Transform of the visual mesh (the handle)
    [Header("Component References")]
    public Transform valveHandleTransform;

    [Header("Rotation Settings")]
    public float rotationSpeed = 90f;        // Degrees per second
    public float maxRotation = 360f;         // Total rotation before considered fully turned

    // ... (rest of your existing variables) ...
    [Header("Events")]
    public UnityEvent onValveFullyTurned;    // Triggered when fully rotated

    private float currentRotation = 0f;
    private bool isCompleted = false;

    public bool CanRotate = false;           // Controlled by Raycast

    void Update()
    {
        // Guard clause: ensure the handle reference is set
        if (valveHandleTransform == null)
        {
            Debug.LogError("valveHandleTransform is not assigned in the Inspector!");
            return;
        }

        if (isCompleted || !CanRotate)
            return;

        float rotationDelta = 0f;

        // ... (input checks remain the same) ...
        if (Input.GetMouseButton(1))
        {
            rotationDelta = rotationSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButton(0))
        {
            rotationDelta = -rotationSpeed * Time.deltaTime;
        }

        if (rotationDelta != 0f)
        {
            // --- THE FIX IS HERE ---
            // Rotate the assigned handle Transform instead of 'this.transform'
            // NOTE: You may need to change Vector3.forward to Vector3.up or Vector3.right
            // based on the child model's local axis.
            valveHandleTransform.Rotate(Vector3.forward, rotationDelta, Space.Self);

            currentRotation += Mathf.Abs(rotationDelta);

            if (currentRotation >= maxRotation)
            {
                currentRotation = maxRotation;
                isCompleted = true;
                CanRotate = false;
                Debug.Log("Valve fully turned!");
                onValveFullyTurned.Invoke();
            }
        }
    }
}