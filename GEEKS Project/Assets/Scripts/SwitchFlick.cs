using UnityEngine;
using System.Collections; // Required for using Coroutines

public class SwitchFlick : MonoBehaviour
{
    public Transform pivot;           // The transform to rotate (likely the switch handle's parent)
    public float flickAngle = 180f;    // 180 degrees for the flip illusion
    public float duration = 0.2f;      // Time in seconds the flip should take
    public GameObject objectToActivate; // optional task

    private bool isOn = false;
    private bool isFlipping = false; // Guard to prevent re-triggering while flipping

    void Start()
    {
        if (pivot == null)
        {
            // Fallback to the object this script is attached to if no pivot is assigned
            pivot = this.transform;
        }
    }

    void OnMouseDown()
    {
        // Don't allow a new click while the switch is already in motion
        if (isFlipping) return;

        // 1. Toggle the state
        isOn = !isOn;

        // 2. Start the smooth rotation coroutine
        StartCoroutine(RotateSwitch(flickAngle));

        // 3. Trigger task
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(isOn);
        }

        Debug.Log("Switch is now " + (isOn ? "ON" : "OFF"));
    }

    // Coroutine to handle smooth, interpolated rotation
    private IEnumerator RotateSwitch(float angle)
    {
        isFlipping = true;

        // The rotation to be applied relative to the current position
        Quaternion rotationDelta = Quaternion.AngleAxis(angle, Vector3.forward);

        Quaternion startRot = pivot.localRotation;
        // The target rotation is the current rotation multiplied by the delta
        Quaternion targetRot = startRot * rotationDelta;

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Use Lerp (Linear Interpolation) to smoothly move from start to target rotation
            pivot.localRotation = Quaternion.Slerp(startRot, targetRot, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Snap to the final rotation to prevent floating point errors
        pivot.localRotation = targetRot;

        isFlipping = false;
    }
}