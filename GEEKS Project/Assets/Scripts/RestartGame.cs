using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class RestartGame : MonoBehaviour
{
    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("The button is working");

        SceneManager.LoadScene(0);
    }

}
