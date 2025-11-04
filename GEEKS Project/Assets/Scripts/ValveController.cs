using UnityEngine;
using System.Collections;

public class ValveController : MonoBehaviour
{
    // --- Configuration for the valve ---
    public float rotationAngle = 90f;
    public Vector3 rotationAxis = Vector3.right; // X-Axis for the valve spin
    public float rotationDuration = 0.5f;

    // ⭐ NEW: Reference to the central manager
    [Header("System Integration")]
    public RefrigerationCycleManager manager;

    // ⭐ NEW: Charge adjustment parameters
    [Tooltip("The amount of charge to add/remove per full rotationAngle.")]
    public float chargeAdjustmentPerClick = 0.05f;

    [Tooltip("Set to 1 for adding charge (V3), or -1 for removing charge (V2).")]
    public float chargeDirection = 1f;

    [Header("Valve State")]
    public float TotalRotationAngle = 0f;

    private bool isRotating = false;

    // --- REMOVED: gaugeNeedle and GaugeScale are no longer needed here ---
    // private const float GaugeScale = 0.25f;

    void Start()
    {
        if (manager == null)
        {
            Debug.LogError("ValveController needs a reference to the RefrigerationCycleManager!");
            enabled = false;
        }
    }

    void OnMouseDown()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateValveSmoothly());
        }
    }

    IEnumerator RotateValveSmoothly()
    {
        isRotating = true;

        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);

        // Calculate the target total rotation for the valve
        float newTotalRotation = TotalRotationAngle + rotationAngle;

        float timeElapsed = 0f;

        // Calculate the total charge change for this rotation click
        float totalChargeChange = chargeAdjustmentPerClick * chargeDirection;
        float chargeChangePerFrame;

        // Calculate the charge change amount based on the duration (for smooth update)
        // If duration is too short, we calculate a simple delta update
        if (rotationDuration > 0.001f)
        {
            chargeChangePerFrame = totalChargeChange / (rotationDuration / Time.deltaTime);
        }
        else
        {
            chargeChangePerFrame = totalChargeChange; // Update all at once if duration is near zero
        }

        // --- Interpolation Loop ---
        while (timeElapsed < rotationDuration)
        {
            float t = timeElapsed / rotationDuration;

            // 1. Rotate the VALVE
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // 2. ⭐ NEW: Adjust the Refrigerant Charge on the Manager
            // We use a small, frequent update during the rotation duration.
            manager.AdjustCharge(chargeChangePerFrame);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Finalize rotation
        transform.localRotation = targetRotation;
        TotalRotationAngle = newTotalRotation;

        // ⭐ NEW: Ensure any remaining fractional change is applied
        // This is important to ensure the total charge added/removed is exactly `totalChargeChange`.
        float actualChargeAdded = (TotalRotationAngle - (newTotalRotation - rotationAngle)) * (chargeAdjustmentPerClick / rotationAngle) * chargeDirection;
        float difference = totalChargeChange - actualChargeAdded;

        // Only adjust if the difference is significant
        if (Mathf.Abs(difference) > 0.0001f)
        {
            manager.AdjustCharge(difference);
        }

        isRotating = false;
    }
}