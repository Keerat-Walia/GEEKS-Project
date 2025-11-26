using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Attachable script to load a specific scene when a function is called.
/// Designed primarily for use with UI buttons.
/// </summary>
public class LoadMenu : MonoBehaviour
{
    // Public method to be called by the Button's OnClick() event.
    // Ensure the scene "MenuPage" is added to your Build Settings (File > Build Settings).
    public void GoToMenuPage()
    {
        // Load the scene with the name "MenuPage".
        Debug.Log("Attempting to load scene: MenuPage");

        // SceneManager is used for managing scenes, and it requires the 
        // UnityEngine.SceneManagement namespace.
        SceneManager.LoadScene("MenuPage");
    }

    // You can also create a generic function to load any scene if you prefer
    // to pass the scene name from the Inspector, which is more flexible.
    public void LoadSpecificScene(string sceneName)
    {
        Debug.Log($"Attempting to load scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}