using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputMouseLook : MonoBehaviour
{
    public float mouseSensitivity = 0.5f;
    public Transform playerBody; // Assign the player or parent object here

    private InputAction lookAction;
    float xRotation = 0f;

    // Assuming you have an Input Action asset named 'PlayerControls'
    private PlayerControls playerControls;

    void Awake()
    {
        playerControls = new PlayerControls();
        lookAction = playerControls.Player.Look; // Replace 'Player' and 'Look' with your map/action names
    }

    void OnEnable()
    {
        playerControls.Enable();
        // Lock and hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        // Read the mouse delta value from the Input Action
        Vector2 lookDelta = lookAction.ReadValue<Vector2>() * mouseSensitivity;

        // Vertical Rotation (Pitch - up/down)
        xRotation -= lookDelta.y * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotation to the camera object itself
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal Rotation (Yaw - left/right)
        // Apply rotation to the player/parent body
        playerBody.Rotate(Vector3.up * lookDelta.x * Time.deltaTime);
    }
}