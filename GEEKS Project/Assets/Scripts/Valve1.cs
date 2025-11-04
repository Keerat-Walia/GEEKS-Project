using UnityEngine;

using System.Collections;


public class Valve1 : MonoBehaviour

{

    // Configuration for the valve

    public float rotationAngle = 90f;

    public Vector3 rotationAxis = Vector3.right; // X-Axis for the valve spin

    public float rotationDuration = 0.5f;


    // ⭐ NEW: Reference to the Gauge Needle Transform

    public Transform gaugeNeedle;


    // ⭐ NEW: Scaling Factor (1/4 or 0.25)

    private const float GaugeScale = 0.25f;


    [Header("Valve State")]

    public float TotalRotationAngle = 0f;


    private bool isRotating = false;


    void OnMouseDown()

    {

        if (!isRotating)

        {

            StartCoroutine(RotateValveSmoothly());

        }

    }


    IEnumerator RotateValveSmoothly()

    {

        isRotating = true;


        Quaternion startRotation = transform.localRotation;

        Quaternion targetRotation = startRotation * Quaternion.AngleAxis(rotationAngle, rotationAxis);


        // Calculate the target total rotation for the valve

        float newTotalRotation = TotalRotationAngle + rotationAngle;


        float timeElapsed = 0f;


        while (timeElapsed < rotationDuration)

        {

            float t = timeElapsed / rotationDuration;


            // 1. Rotate the VALVE

            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);


            // 2. Rotate the GAUGE NEEDLE proportionally

            if (gaugeNeedle != null)

            {

                // Calculate the current rotation interpolation for this frame

                float currentValveRotation = Mathf.Lerp(TotalRotationAngle, newTotalRotation, t);


                // Apply the 1/4 scale factor for the gauge rotation

                float gaugeRotation = currentValveRotation * GaugeScale;


                // Assuming the gauge rotates around its Z-axis (Vector3.forward) for a typical dial

                gaugeNeedle.localRotation = Quaternion.Euler(0f, 0f, -gaugeRotation);

                // Note: The negative sign is often needed to make the gauge needle move clockwise/correctly.

            }


            timeElapsed += Time.deltaTime;

            yield return null;

        }


        // Finalize rotations

        transform.localRotation = targetRotation;

        TotalRotationAngle = newTotalRotation;


        // Finalize gauge rotation to ensure precision

        if (gaugeNeedle != null)

        {

            float finalGaugeRotation = TotalRotationAngle * GaugeScale;

            gaugeNeedle.localRotation = Quaternion.Euler(0, finalGaugeRotation, 0f);

        }


        isRotating = false;

    }

}