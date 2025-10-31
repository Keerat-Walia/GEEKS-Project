using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneSwitcher : MonoBehaviour
{
    public string targetSceneName; // Name of the scene to load when Escape is pressed

    void Update()
    {
        // Check if the Escape key is pressed down
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Load the specified scene
            SceneManager.LoadScene(targetSceneName);
        }
    }
}