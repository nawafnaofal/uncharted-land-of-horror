using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Quit Game");
    }
}
