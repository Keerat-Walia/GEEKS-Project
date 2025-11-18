using UnityEngine;
using TMPro;

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

    // --- Gauge UI References ---
    [Header("Gauge UI References (Heat Exchanger 1 - Condenser)")]
    public TextMeshProUGUI he1PressureText;
    public TextMeshProUGUI he1TemperatureText;

    [Header("Gauge UI References (Heat Exchanger 2 - Evaporator)")] // ⭐ ADDED HE2 REFERENCES
    public TextMeshProUGUI he2PressureText;
    public TextMeshProUGUI he2TemperatureText;

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

    private float currentPressure;
    private float currentTemperature;

    private void Start()
    {
        refrigerantCharge = Mathf.Clamp(refrigerantCharge, minCharge, 1.0f);
        fanSpeed = Mathf.Clamp(fanSpeed, 0.0f, 1.0f);
        UpdateGauges();
    }

    /// <summary>
    /// Called by the ValveController to change the refrigerant charge.
    /// </summary>
    public void AdjustCharge(float delta)
    {
        refrigerantCharge += delta;
        refrigerantCharge = Mathf.Clamp(refrigerantCharge, minCharge, 1.0f);
        UpdateGauges();
    }

    /// <summary>
    /// Called by the ValveController to change the fan speed.
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

        // 3. Update the UI Text Gauges.

        // Heat Exchanger 1 (Condenser - generally higher P/T)
        if (he1PressureText != null) he1PressureText.text = $"P: {currentPressure * 1.5f:F1} kPa";
        if (he1TemperatureText != null) he1TemperatureText.text = $"T: {currentTemperature * 1.5f:F1} °C";

        // Heat Exchanger 2 (Evaporator - generally lower P/T)
        // ⭐ UPDATED LOGIC TO CHECK FOR NULLS AND USE HE2 REFERENCES
        if (he2PressureText != null) he2PressureText.text = $"P: {currentPressure * 0.5f:F1} kPa";
        if (he2TemperatureText != null) he2TemperatureText.text = $"T: {currentTemperature * 0.5f:F1} °C";
    }
}