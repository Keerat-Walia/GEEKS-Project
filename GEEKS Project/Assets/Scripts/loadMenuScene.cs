using UnityEngine;
using UnityEngine.SceneManagement;

public class loadMenuScene : MonoBehaviour
{
    // The name of the scene to load
    public string targetSceneName = "MenuPage";

    void Update()
    {
        // Check if the Enter key (KeyCode.Return or KeyCode.KeypadEnter) is pressed
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            LoadTargetScene();
        }
    }

    // Public method to be called by a UI Button component
    public void LoadTargetScene()
    {
        // Load the scene by its name
        // Make sure the scene is added to the Build Settings
        SceneManager.LoadScene(targetSceneName);
    }
}