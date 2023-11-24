using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainMenu : MonoBehaviour
{

    public Button loadButton;
    private InfoManager infoManager;

    void Start()
    {
        //infoManager = FindObjectOfType<InfoManager>();

        // Mengecek apakah InfoManager sudah ada sebelum menggunakannya
        if (InfoManager.Instance != null)
        {
            // InfoManager sudah ada, bisa menggunakan instance-nya
            InfoManager.Instance.NewGameStats();
        }
        else
        {
            // InfoManager belum ada, tampilkan pesan error
            Debug.LogError("InfoManager not found in the scene.");
        }

        // Mengecek apakah file save data ada
        string savePath = Application.persistentDataPath + "/player.json";
        if (File.Exists(savePath))
        {
            // File save data ada, aktifkan tombol Load
            loadButton.interactable = true;
        }
        else
        {
            // File save data tidak ada, nonaktifkan tombol Load
            loadButton.interactable = false;
        }
    }


    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnNewGameButtonClicked()
    {
        // Logic untuk memulai permainan baru
        InfoManager.Instance.NewGameStats();
        // Tambahkan logika lain sesuai kebutuhan permainan
        SceneManager.LoadScene("Main"); // Ganti dengan nama scene yang sesuai
    }

    // Contoh di metode atau fungsi yang menangani tombol "Load Game"
    public void OnLoadGameButtonClicked()
    {
        // Lakukan apa yang diperlukan untuk memuat permainan yang sudah ada
        // ...

        // Setelah itu, panggil LoadStats() untuk memuat data dari save data
        InfoManager.Instance.LoadStats();
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Quit Game");
    }
}
