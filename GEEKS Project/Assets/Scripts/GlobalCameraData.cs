// GlobalCameraData.cs
using UnityEngine;

public static class GlobalCameraData
{
    // A static variable to hold the rotation (Quaternion) from the previous scene.
    // Static variables persist across scene loads (unless marked [RuntimeInitializeOnLoadMethod]).
    public static Quaternion cameraRotation;

    // A flag to check if we should actually apply the stored rotation.
    // Set to true by loadNextScene, set to false after being used.
    public static bool applyRotation = false;
}