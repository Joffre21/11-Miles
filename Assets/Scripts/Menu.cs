using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string gameSceneName = "SampleScene";
    public static bool startGameOnLoad = false;

    public void StartGame()
    {
        startGameOnLoad = true;
        SceneManager.LoadScene(gameSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
