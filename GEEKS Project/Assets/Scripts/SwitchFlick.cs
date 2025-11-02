using UnityEngine;

public class SwitchFlick : MonoBehaviour
{
    public Transform pivot;           // the pivot to rotate
    public float flickAngle = 90f;    // degrees to rotate around Z
    public GameObject objectToActivate; // optional task

    private bool isOn = false;
    private Quaternion defaultRotation;

    void Start()
    {
        if (pivot == null)
        {
            Debug.LogError("Pivot not assigned!");
            return;
        }
        defaultRotation = pivot.localRotation;
    }

    void OnMouseDown()
    {
        if (pivot == null) return;

        isOn = !isOn;

        // Rotate around Z
        pivot.localRotation = isOn
            ? Quaternion.Euler(defaultRotation.eulerAngles.x,
                               defaultRotation.eulerAngles.y,
                               defaultRotation.eulerAngles.z + flickAngle)
            : defaultRotation;

        // Trigger task
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(isOn);
        }

        Debug.Log("Switch is now " + (isOn ? "ON" : "OFF"));
    }
}
