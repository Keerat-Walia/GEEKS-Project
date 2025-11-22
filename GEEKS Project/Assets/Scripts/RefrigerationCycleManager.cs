using UnityEngine;
using TMPro;

public class RefrigerationCycleManager : MonoBehaviour
{
    // --- System State Variables ---
    [Header("System State")]
    [Range(0f, 1f)]
    public float refrigerantCharge = 0.5f;

    [Range(0f, 1f)]
    public float fanSpeed = 0.5f;

    // --- Gauge UI References (3D Transforms for Needles) ---
    [Header("Gauge UI References (Heat Exchanger 1 - Condenser)")]
    public Transform he1PressureNeedle;
    public Transform he1TemperatureNeedle;

    [Header("Gauge UI References (Heat Exchanger 2 - Evaporator)")]
    public Transform he2PressureNeedle;
    public Transform he2TemperatureNeedle;

    // --- System Constants (Simplified Model) ---
    [Header("System Constants")]
    public float basePressure = 50.0f;
    public float pressureSensitivity = 100.0f;
    public float baseTemperature = 20.0f;
    public float temperatureSensitivity = 40.0f;
    public float minCharge = 0.1f;

    [Header("Fan Influence")]
    public float fanCoolingInfluence = 20.0f;

    [Header("Gauge Rotation Mapping")]
    public float minGaugeAngle = -135f;
    public float maxGaugeAngle = 135f;
    public float maxCondenserPressure = 150.0f;
    public float maxCondenserTemperature = 60.0f;

    // --- Game Management ---
    [Header("Game Management")]
    public RefrigerationUIManager uiManager;

    // Target for game completion checks
    private const float TARGET_PRESSURE_MAX = 135.0f; // Goal for Level 1 (Max P/T)
    private const float TARGET_PRESSURE_MIN = 30.0f;  // Goal for Level 2 (Min P/T)

    // --- Private Calculated State ---
    private float currentPressure;
    private float currentTemperature;

    private void Start()
    {
        // Clamp initial values
        refrigerantCharge = Mathf.Clamp(refrigerantCharge, minCharge, 1.0f);
        fanSpeed = Mathf.Clamp(fanSpeed, 0.0f, 1.0f);

        // Initial UI update runs, but the objective checks are gated.
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

    /// <summary>
    /// Sets the fan speed directly for objective initialization (called by UIManager).
    /// </summary>
    public void SetFanSpeed(float speed)
    {
        fanSpeed = Mathf.Clamp(speed, 0.0f, 1.0f);
        UpdateGauges();
    }


    private void UpdateGauges()
    {
        // 1. Calculate the base pressure and temperature based on charge.
        float chargeDelta = refrigerantCharge - 0.5f;

        // 2. Calculate the cooling/pressure effect of the fan.
        float fanEffect = (fanSpeed - 0.5f) * fanCoolingInfluence;

        // Base system calculation 
        currentPressure = basePressure + (chargeDelta * pressureSensitivity) - fanEffect;
        currentTemperature = baseTemperature + (chargeDelta * temperatureSensitivity) - fanEffect;

        // 3. Calculate separate P/T values for high (HE1) and low (HE2) sides.
        float he1P = currentPressure * 1.5f;
        float he1T = currentTemperature * 1.5f;
        float he2P = currentPressure * 0.5f;
        float he2T = currentTemperature * 0.5f;


        // 4. Apply rotations to the 3D gauge needles.
        RotateNeedle(he1PressureNeedle, he1P, maxCondenserPressure);
        RotateNeedle(he1TemperatureNeedle, he1T, maxCondenserTemperature);
        RotateNeedle(he2PressureNeedle, he2P, maxCondenserPressure);
        RotateNeedle(he2TemperatureNeedle, he2T, maxCondenserTemperature);

        // 5. Game Progression Check (Gated by isGameActive)
        if (uiManager != null && uiManager.isGameActive)
        {
            uiManager.CheckLevelOneObjective(he1P, TARGET_PRESSURE_MAX);
            uiManager.CheckLevelTwoObjective(he1P, TARGET_PRESSURE_MIN);
        }
    }

    /// <summary>
    /// Maps a sensor value to a rotation angle and applies it to the needle's Z-axis.
    /// </summary>
    private void RotateNeedle(Transform needle, float value, float maxValue)
    {
        if (needle == null) return;

        float clampedValue = Mathf.Clamp(value, 0f, maxValue);
        float normalizedValue = clampedValue / maxValue;
        float rotationAngleZ = Mathf.Lerp(minGaugeAngle, maxGaugeAngle, normalizedValue);

        // Apply the rotation on the Z-axis 
        needle.localRotation = Quaternion.Euler(rotationAngleZ, 0f, 0f);
    }
}