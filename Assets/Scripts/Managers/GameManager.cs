using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerWin()
    {
        // Panggil fungsi atau tindakan yang perlu dilakukan saat pemain menang
        Debug.Log("Player Win!");
        // Misalnya, tampilkan layar kemenangan atau tampilkan pesan kemenangan

        // Untuk contoh, kita reload scene setelah menang
        ReloadScene();
    }

    public void PlayerLose()
    {
        // Panggil fungsi atau tindakan yang perlu dilakukan saat pemain kalah
        Debug.Log("Player Lose!");
        
        ShowGameOverScreen();
    }

    public void ReloadScene()
    {
        // Fungsi untuk me-reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        // Fungsi untuk kembali ke menu utama
        SceneManager.LoadScene("MainMenu"); // Ganti "MainMenu" dengan nama scene menu utama Anda
    }

    public void Retry()
    {
        // Fungsi untuk mengulang dari posisi terakhir
        ReloadScene();
    }

    void ShowGameOverScreen()
    {
        // dengan tombol Quit dan Retry yang terhubung ke QuitToMainMenu dan Retry
        UIManager.instance.ShowLosePanel(); // Panggil metode yang menampilkan panel kekalahan pada UIManager
    }
}
