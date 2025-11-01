using UnityEngine;

public class Raycast1 : MonoBehaviour
{
    public float interactRange = 3f;  // How far you can reach
    private Valve currentValve;

    void Update()
    {
        // Draw ray in Scene view for debugging
        Debug.DrawRay(transform.position, transform.forward * interactRange, Color.red);

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            Valve valve = hit.collider.GetComponentInParent<Valve>();
            if (valve != null)
            {
                // Set valve to be rotatable while looking at it
                if (currentValve != valve)
                {
                    // Reset previous valve if any
                    if (currentValve != null)
                        currentValve.CanRotate = false;

                    currentValve = valve;
                }

                currentValve.CanRotate = true;
            }
        }
        else
        {
            // Not looking at any valve, disable rotation
            if (currentValve != null)
            {
                currentValve.CanRotate = false;
                currentValve = null;
            }
        }
    }
}
