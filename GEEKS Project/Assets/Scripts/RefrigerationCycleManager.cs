using UnityEngine;
using TMPro; // Required for using TextMeshPro gauges

public class RefrigerationCycleManager : MonoBehaviour
{
    // --- System State Variables ---
    [Header("System State")]
    [Tooltip("Represents the amount of refrigerant in the system (0.0 to 1.0)")]
    [Range(0f, 1f)]
    public float refrigerantCharge = 0.5f;

    // --- Gauge References ---
    [Header("Gauge UI References (Heat Exchanger 1)")]
    public TextMeshProUGUI he1PressureText;
    public TextMeshProUGUI he1TemperatureText;

    [Header("Gauge UI References (Heat Exchanger 2)")]
    public TextMeshProUGUI he2PressureText;
    public TextMeshProUGUI he2TemperatureText;

    // --- System Constants (Simplified Model) ---
    [Header("System Constants")]
    public float basePressure = 50.0f; // Pressure when charge is 0.5
    public float pressureSensitivity = 100.0f; // Max pressure delta
    public float baseTemperature = 20.0f; // Temperature when charge is 0.5 (Room Temp)
    public float temperatureSensitivity = 40.0f; // Max temperature delta (e.g., 20C to 60C)
    public float minCharge = 0.1f; // Minimum charge to prevent infinite pressure/temp drop

    // --- Pressure & Temperature Calculation ---
    private float currentPressure;
    private float currentTemperature;

    private void Start()
    {
        // Clamp the initial charge to be safe
        refrigerantCharge = Mathf.Clamp(refrigerantCharge, minCharge, 1.0f);
        UpdateGauges();
    }

    /// <summary>
    /// Called by the ValveController to change the refrigerant charge.
    /// </summary>
    /// <param name="delta">The amount to add or remove.</param>
    public void AdjustCharge(float delta)
    {
        refrigerantCharge += delta;
        refrigerantCharge = Mathf.Clamp(refrigerantCharge, minCharge, 1.0f);
        UpdateGauges();
    }

    private void UpdateGauges()
    {
        // 1. Calculate the new pressure and temperature based on charge.
        // Simplified linear model: Higher charge -> higher pressure/temperature.
        float chargeDelta = refrigerantCharge - 0.5f; // Deviation from 'normal' charge

        // Pressure (e.g., in PSI or kPa)
        currentPressure = basePressure + (chargeDelta * pressureSensitivity);

        // Temperature (e.g., in Celsius)
        // Note: In a real cycle, pressure and temperature are related (P-T Saturation curve).
        currentTemperature = baseTemperature + (chargeDelta * temperatureSensitivity);

        // 2. Update the UI Text Gauges.
        // Heat Exchanger 1 (e.g., Condenser - generally higher P/T)
        he1PressureText.text = $"P: {currentPressure:F1} kPa";
        he1TemperatureText.text = $"T: {currentTemperature * 1.5f:F1} °C"; // Condenser is hotter

        // Heat Exchanger 2 (e.g., Evaporator - generally lower P/T)
        he2PressureText.text = $"P: {currentPressure * 0.5f:F1} kPa"; // Evaporator is lower pressure
        he2TemperatureText.text = $"T: {currentTemperature * 0.5f:F1} °C"; // Evaporator is colder

        // Optional: Rotate 3D gauge needles if you use them.
    }
}