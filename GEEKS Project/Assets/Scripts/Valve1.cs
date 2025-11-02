using UnityEngine;
using UnityEngine.Events;

public class Valve1 : MonoBehaviour
{
    // ... (Your existing variables) ...
    private bool isCompleted = false;
    private bool isBeingClicked = false; // This is the new, local gate

    void Update()
    {
        // Only run rotation if this specific valve is being clicked
        if (isCompleted || !isBeingClicked)
            return;

        float rotationDelta = 0f;

        // ... (Your rotation input logic using Input.GetMouseButton(0/1)) ...

        if (rotationDelta != 0f)
        {
            // ... (Your rotation and completion logic) ...
        }
    }

    void OnMouseDown()
    {
        // This only runs when the mouse clicks THIS object's collider
        if (!isCompleted)
        {
            isBeingClicked = true;
        }
    }

    void OnMouseUp()
    {
        // This only runs when the mouse is released after being down on THIS object
        isBeingClicked = false;
    }
}