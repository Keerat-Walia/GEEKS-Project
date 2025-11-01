using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ContinueGame : MonoBehaviour
{
    public string targetSceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

            SceneManager.LoadScene(targetSceneName);
       
        
    }
    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

}
