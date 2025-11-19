// This script calls the manager function when the 3D object is clicked
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    // Assign the manager in the Inspector
    [SerializeField] private MissionManager manager;

    // 0=Goggles, 1=LabCoat, 2=Door (Use an enum for cleaner code if you prefer!)
    [SerializeField] private int actionType;

    // Requires a Collider component on the object
    void OnMouseDown()
    {
        if (manager == null) return;

        switch (actionType)
        {
            case 0: // Goggles
                manager.OnGogglesClicked();
                break;
            case 1: // Lab Coat
                manager.OnLabCoatClicked();
                break;
            case 2: // Door
                manager.OnDoorClicked();
                break;
        }
    }
}