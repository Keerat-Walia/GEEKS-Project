using UnityEngine;
using System.Collections; // Required for Coroutines

public class Valve1 : MonoBehaviour
{
    public float rotationAngle = 90f; // Total angle to rotate
    public Vector3 rotationAxis = Vector3.forward;
    public float rotationDuration = 0.5f; // Time in seconds for the rotation
    private bool isRotating = false; // Flag to prevent multiple rotations at once

    void OnMouseDown()
    {
        // Only start a new rotation if one isn't already in progress
        if (!isRotating)
        {
            StartCoroutine(RotateValveSmoothly());
        }
    }

    IEnumerator RotateValveSmoothly()
    {
        isRotating = true;

        // Calculate the target rotation
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);

        float timeElapsed = 0f;

        while (timeElapsed < rotationDuration)
        {
            // Lerp from start to target rotation over time
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the rotation ends exactly at the target
        transform.rotation = targetRotation;

        isRotating = false;
    }
}