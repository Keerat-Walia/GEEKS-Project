// loadNextScene.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadNextScene : MonoBehaviour
{
    // The name of the scene to load
    public string targetSceneName = "MenuPage";

    void Update()
    {
        // ... (Update is still empty)
    }

    // Public method to be called by a UI Button component
    public void LoadTargetScene()
    {
        // 1. Find the Main Camera in the current scene.
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            // 2. SAVE the current camera's rotation to the static data class.
            GlobalCameraData.cameraRotation = mainCamera.transform.rotation;

            // 3. Set the flag to indicate this rotation should be applied in the next scene.
            GlobalCameraData.applyRotation = true;

            Debug.Log("Saving camera rotation: " + GlobalCameraData.cameraRotation.eulerAngles);
        }
        else
        {
            Debug.LogError("No Main Camera found in the current scene!");
        }

        // 4. Load the scene by its name
        SceneManager.LoadScene(targetSceneName);
    }
}