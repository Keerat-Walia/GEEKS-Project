using UnityEngine;

/// <summary>
/// Controls the rotation of the attached GameObject (usually the camera) 
/// using Arrow Keys and Click-and-Drag mouse input.
/// </summary>
public class cameraRotator : MonoBehaviour
{
    // --- Public Configuration Variables ---

    [Header("Rotation Speeds")]
    [Tooltip("The speed of rotation when using arrow keys (degrees per second).")]
    public float rotationSpeedKeyboard = 50f;
    [Tooltip("The speed multiplier for mouse movement.")]
    public float rotationSpeedMouse = 3f;

    [Header("Limits")]
    [Tooltip("The maximum angle the camera can pitch (look up/down) before clamping.")]
    public float pitchLimit = 80f; // Prevents camera from flipping (e.g., 80 degrees up/down

    // --- Private State Variables ---

    // Stores the current Euler angles (Pitch, Yaw, Roll)
    private Vector3 _currentEulerAngles;

    void Start()
    {
        // Initialize the stored rotation to the object's current rotation.
        _currentEulerAngles = transform.localEulerAngles;

        // Ensure the rotation is normalized (to handle cases where X > 180 degrees)
        if (_currentEulerAngles.x > 180)
        {
            _currentEulerAngles.x -= 360;
        }

        // Optional: Lock the cursor in the center of the screen during gameplay
        // Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        // 1. Handle Keyboard Input (Arrow Keys)
        HandleKeyboardInput();

        // 2. Handle Mouse Input (Click and Drag)
        HandleMouseInput();

        // 3. Apply the final calculated rotation to the Transform
        transform.localRotation = Quaternion.Euler(_currentEulerAngles);
    }

    /// <summary>
    /// Calculates rotation based on the "Horizontal" and "Vertical" input axes (defaulted to Arrow Keys).
    /// </summary>
    void HandleKeyboardInput()
    {
        // Get input from Arrow Keys (or WASD, if set up in Input Manager)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Only calculate rotation if there is input
        if (Mathf.Abs(horizontalInput) > 0.01f || Mathf.Abs(verticalInput) > 0.01f)
        {
            // Yaw (Y-axis rotation for Left/Right keys)
            // We use Time.deltaTime to make rotation speed frame-rate independent.
            _currentEulerAngles.y += horizontalInput * rotationSpeedKeyboard * Time.deltaTime;

            // Pitch (X-axis rotation for Up/Down keys)
            // Subtracting because positive vertical input (Up arrow) corresponds to looking down (negative X rotation)
            _currentEulerAngles.x -= verticalInput * rotationSpeedKeyboard * Time.deltaTime;

            // Clamp the pitch (X-axis rotation) to prevent flipping
            _currentEulerAngles.x = Mathf.Clamp(_currentEulerAngles.x, -pitchLimit, pitchLimit);
        }
    }

    /// <summary>
    /// Calculates rotation based on mouse movement while the left mouse button is held down.
    /// </summary>
    void HandleMouseInput()
    {
        // Check for left mouse button click and hold (Input.GetMouseButton(0))
        if (Input.GetMouseButton(0))
        {
            // Get mouse movement delta (raw pixel movement)
            float mouseX = Input.GetAxis("Mouse X"); // Yaw contribution
            float mouseY = Input.GetAxis("Mouse Y"); // Pitch contribution

            // Yaw (Y-axis rotation)
            _currentEulerAngles.y += mouseX * rotationSpeedMouse;

            // Pitch (X-axis rotation) 
            // Subtracting mouseY flips the rotation to be intuitive (moving mouse up means looking up)
            _currentEulerAngles.x -= mouseY * rotationSpeedMouse;

            // Clamp the pitch (X-axis rotation) to prevent flipping
            _currentEulerAngles.x = Mathf.Clamp(_currentEulerAngles.x, -pitchLimit, pitchLimit);
        }
    }
}
