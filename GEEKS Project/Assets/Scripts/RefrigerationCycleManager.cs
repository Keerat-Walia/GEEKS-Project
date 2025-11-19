using UnityEngine;
using TMPro; // Still needed if you use TMPro for other UI elements (like current values or labels)

public class RefrigerationCycleManager : MonoBehaviour
{
    // --- System State Variables ---
    [Header("System State")]
    [Tooltip("Represents the amount of refrigerant in the system (0.0 to 1.0)")]
    [Range(0f, 1f)]
    public float refrigerantCharge = 0.5f;

    [Tooltip("Fan speed (0.0 to 1.0)")]
    [Range(0f, 1f)]
    public float fanSpeed = 0.5f;

    // --- Gauge UI References (3D Transforms for Needles) ---
    [Header("Gauge UI References (Heat Exchanger 1 - Condenser)")]
    [Tooltip("The Transform of the Condenser Pressure gauge needle.")]
    public Transform he1PressureNeedle;
    [Tooltip("The Transform of the Condenser Temperature gauge needle.")]
    public Transform he1TemperatureNeedle;

    [Header("Gauge UI References (Heat Exchanger 2 - Evaporator)")]
    [Tooltip("The Transform of the Evaporator Pressure gauge needle.")]
    public Transform he2PressureNeedle;
    [Tooltip("The Transform of the Evaporator Temperature gauge needle.")]
    public Transform he2TemperatureNeedle;

    // --- System Constants (Simplified Model) ---
    [Header("System Constants")]
    public float basePressure = 50.0f;
    public float pressureSensitivity = 100.0f;
    public float baseTemperature = 20.0f;
    public float temperatureSensitivity = 40.0f;
    public float minCharge = 0.1f;

    [Header("Fan Influence")]
    [Tooltip("How much the fan speed affects the cooling/pressure drop (e.g., 20)")]
    public float fanCoolingInfluence = 20.0f;

    [Header("Gauge Rotation Mapping")]
    [Tooltip("The minimum (start) angle for the gauge needle rotation (e.g., -135)")]
    public float minGaugeAngle = -135f;
    [Tooltip("The maximum (end) angle for the gauge needle rotation (e.g., 135)")]
    public float maxGaugeAngle = 135f;
    [Tooltip("The maximum expected value for the highest pressure gauge (e.g., 150 kPa)")]
    public float maxCondenserPressure = 150.0f;
    [Tooltip("The maximum expected value for the highest temperature gauge (e.g., 60 °C)")]
    public float maxCondenserTemperature = 60.0f;

    // --- Game Management ---
    [Header("Game Management")]
    [Tooltip("Reference to the UIManager script for game progression.")]
    public RefrigerationUIManager uiManager;

    // Target values for game completion checks
    private const float TARGET_PRESSURE_MAX = 150.0f; // Goal for Charge Level
    private const float TARGET_PRESSURE_MIN = 60.0f;  // Goal for Fan Speed Level

    // --- Private Calculated State ---
    private float currentPressure;
    private float currentTemperature;

    private void Start()
    {
        // Clamp initial values
        refrigerantCharge = Mathf.Clamp(refrigerantCharge, minCharge, 1.0f);
        fanSpeed = Mathf.Clamp(fanSpeed, 0.0f, 1.0f);

        // Initial UI update
        UpdateGauges();
    }

    /// <summary>
    /// Called by a ValveController to change the refrigerant charge.
    /// </summary>
    public void AdjustCharge(float delta)
    {
        refrigerantCharge += delta;
        refrigerantCharge = Mathf.Clamp(refrigerantCharge, minCharge, 1.0f);
        UpdateGauges();
    }

    /// <summary>
    /// Called by a ValveController to change the fan speed.
    /// </summary>
    public void AdjustFanSpeed(float delta)
    {
        fanSpeed += delta;
        fanSpeed = Mathf.Clamp(fanSpeed, 0.0f, 1.0f);
        UpdateGauges();
    }

    private void UpdateGauges()
    {
        // 1. Calculate the base pressure and temperature based on charge.
        float chargeDelta = refrigerantCharge - 0.5f;

        // 2. Calculate the cooling/pressure effect of the fan.
        float fanEffect = (fanSpeed - 0.5f) * fanCoolingInfluence;

        // Base system calculation (Charge effect MINUS Fan effect)
        currentPressure = basePressure + (chargeDelta * pressureSensitivity) - fanEffect;
        currentTemperature = baseTemperature + (chargeDelta * temperatureSensitivity) - fanEffect;

        // 3. Calculate separate P/T values for high (HE1) and low (HE2) sides.
        float he1P = currentPressure * 1.5f;
        float he1T = currentTemperature * 1.5f;
        float he2P = currentPressure * 0.5f;
        float he2T = currentTemperature * 0.5f;


        // 4. Apply rotations to the 3D gauge needles.

        // Condenser Pressure (HE1)
        RotateNeedle(he1PressureNeedle, he1P, maxCondenserPressure);
        // Condenser Temperature (HE1)
        RotateNeedle(he1TemperatureNeedle, he1T, maxCondenserTemperature);

        // Evaporator Pressure (HE2)
        RotateNeedle(he2PressureNeedle, he2P, maxCondenserPressure);
        // Evaporator Temperature (HE2)
        RotateNeedle(he2TemperatureNeedle, he2T, maxCondenserTemperature);

        // 5. Game Progression Check (Uses Condenser Pressure for tracking)
        if (uiManager != null)
        {
            // Check for Objective 1: Maximize P/T by charge manipulation
            uiManager.CheckChargeObjective(he1P, TARGET_PRESSURE_MAX);

            // Check for Objective 2: Minimize P/T by fan speed manipulation
            uiManager.CheckFanObjective(he1P, TARGET_PRESSURE_MIN);
        }
    }

    /// <summary>
    /// Maps a sensor value to a rotation angle and applies it to the needle's Z-axis.
    /// NOTE: Change the Quaternion.Euler axis (X, Y, or Z) to adjust rotation axis.
    /// </summary>
    /// <param name="needle">The Transform of the 3D gauge needle.</param>
    /// <param name="value">The current pressure or temperature value.</param>
    /// <param name="maxValue">The maximum value the gauge should display.</param>
    private void RotateNeedle(Transform needle, float value, float maxValue)
    {
        if (needle == null) return;

        // Clamp the value to the max to prevent spinning wildly
        float clampedValue = Mathf.Clamp(value, 0f, maxValue);

        // Calculate the normalized position (0.0 to 1.0)
        float normalizedValue = clampedValue / maxValue;

        // Map the normalized position to the angular range (minGaugeAngle to maxGaugeAngle)
        float rotationAngleZ = Mathf.Lerp(minGaugeAngle, maxGaugeAngle, normalizedValue);

        // Apply the rotation on the Z-axis (change to X or Y as needed for your 3D models)
        // Example for Z-Axis: Quaternion.Euler(0f, 0f, rotationAngleZ)
        // Example for X-Axis: Quaternion.Euler(rotationAngleZ, 0f, 0f)
        needle.localRotation = Quaternion.Euler(0f, 0f, rotationAngleZ);
    }
}