using UnityEngine;
using System.Collections; // Needed for IEnumerator

public class ValveController : MonoBehaviour
{
    // --- Configuration for the valve ---
    public float rotationAngle = 90f;
    public Vector3 rotationAxis = Vector3.right;
    public float rotationDuration = 0.5f;

    // --- System Integration ---
    [Header("System Integration")]
    // Must be a reference to the main manager script
    public RefrigerationCycleManager manager;

    // ⭐ NEW: Enum to define what this valve controls
    public enum ControlType { RefrigerantCharge, FanSpeed }
    public ControlType controlType = ControlType.RefrigerantCharge;

    // --- Control Parameters ---
    [Tooltip("The amount of charge/speed to add/remove per full rotationAngle.")]
    public float adjustmentPerClick = 0.05f;

    [Tooltip("Set to 1 for increase, or -1 for decrease.")]
    public float direction = 1f;

    [Header("Valve State")]
    public float TotalRotationAngle = 0f;

    private bool isRotating = false; // Must be declared outside the coroutine

    // --- Required Unity Methods ---

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

    // --- Coroutine Logic (The part you provided, now fixed and inside the class) ---

    IEnumerator RotateValveSmoothly()
    {
        isRotating = true;

        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);

        // Calculate the target total rotation for the valve
        float newTotalRotation = TotalRotationAngle + rotationAngle;

        float timeElapsed = 0f; // Declared within the method scope

        // Calculate the total change for this rotation click
        float totalChange = adjustmentPerClick * direction;

        // --- Tracking the amount of charge applied during the loop ---
        float appliedChange = 0f; // Initialize applied charge tracker

        // Calculate the charge change per frame based on rotation duration
        float changePerFrame;
        // Use a small constant to prevent division by zero, or simply use Time.deltaTime in the loop.
        // For accurate, frame-rate independent change, let's use the time-based loop:

        // --- Interpolation Loop ---
        while (timeElapsed < rotationDuration)
        {
            float t = timeElapsed / rotationDuration;

            // Calculate change for this specific frame based on the duration ratio
            float deltaChangeThisFrame = (Time.deltaTime / rotationDuration) * totalChange;


            // 1. Rotate the VALVE
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // 2. Adjust the correct property on the Manager
            if (controlType == ControlType.RefrigerantCharge)
            {
                manager.AdjustCharge(deltaChangeThisFrame);
            }
            else
            {
                manager.AdjustFanSpeed(deltaChangeThisFrame);
            }

            appliedChange += deltaChangeThisFrame; // Track the applied change

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Finalize rotation
        transform.localRotation = targetRotation;
        TotalRotationAngle = newTotalRotation;

        // ⭐ FIX: Apply any remaining change due to floating point inaccuracies/frame rounding
        float remainingChange = totalChange - appliedChange;

        if (Mathf.Abs(remainingChange) > 0.0001f)
        {
            if (controlType == ControlType.RefrigerantCharge)
            {
                manager.AdjustCharge(remainingChange);
            }
            else
            {
                manager.AdjustFanSpeed(remainingChange);
            }
        }

        isRotating = false;
    }
}