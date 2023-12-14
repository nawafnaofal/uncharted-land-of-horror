using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{

    public Button loadButton;
    public GameObject optionPanel;
    private InfoManager infoManager;

    void Start()
    {
   
        if (InfoManager.Instance != null)
        {

            InfoManager.Instance.NewGameStats();
        }
        else
        {
    
            Debug.LogError("InfoManager not found in the scene.");
        }

        string savePath = Application.persistentDataPath + "/player.json";
        if (File.Exists(savePath))
        {

            loadButton.interactable = true;
        }
        else
        {
  
            loadButton.interactable = false;
        }
    }


    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnNewGameButtonClicked()
    {
   
        InfoManager.Instance.NewGameStats();
 
        SceneManager.LoadScene("Prolog"); 
    }


    public void OnLoadGameButtonClicked()
    {
        
        InfoManager.Instance.LoadStats();
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Quit Game");
    }

    public void ShowOptions()
    {
        // Aktifkan panel opsi
        if (optionPanel != null)
        {
            optionPanel.SetActive(true);
        }
    }

    public void HideOptions()
    {
        // Nonaktifkan panel opsi
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
        }
    }
}
