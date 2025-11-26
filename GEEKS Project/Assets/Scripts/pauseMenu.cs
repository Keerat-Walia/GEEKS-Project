using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    // Static variable to track the pause state globally
    public static bool gameIsPaused = false;

    // Reference to the parent GameObject that holds all the pause menu elements
    public GameObject pauseMenuUI;

    void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // The Escape key always toggles the pause state
            TogglePause();
        }
    }

    // NEW METHOD: Public method that will be called by the Pause Button's On Click() event.
    // This allows the button to act as a toggle, just like the Escape key.
    public void TogglePause()
    {
        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    // Public method to be called by the Resume Button and the TogglePause() method.
    public void Resume()
    {
        // Make sure the reference is assigned before trying to use it
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false); // Hide the pause menu UI
        }
        Time.timeScale = 1f;          // Set time back to normal
        gameIsPaused = false;         // Update pause state
    }

    // Private method called by the TogglePause() method.
    private void Pause()
    {
        // Make sure the reference is assigned before trying to use it
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);  // Show the pause menu UI
        }
        Time.timeScale = 0f;          // Stop the game time
        gameIsPaused = true;          // Update pause state
    }

    public void loadMenu()
    {
        Time.timeScale = 1f; // Always ensure time scale is normal before loading a new scene
        gameIsPaused = false;
        SceneManager.LoadScene("MenuPage");
    }

    public void quitGame()
    {
        Debug.Log("Quitting Game -Keerat");
        Application.Quit();
    }
}