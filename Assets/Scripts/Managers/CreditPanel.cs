using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditPanel : MonoBehaviour
{
    public GameObject backButton;  // Tambahkan objek tombol "Back" di Inspector

    void Start()
    {
        // Menyembunyikan tombol "Back" pada awal
        backButton.SetActive(false);

        // Mulai coroutine untuk menampilkan tombol "Back" setelah animasi selesai
        StartCoroutine(ShowBackButton());
    }

    IEnumerator ShowBackButton()
    {
        // Tunggu hingga animasi selesai (gantilah dengan waktu yang sesuai)
        yield return new WaitForSeconds(5f);

        // Tampilkan tombol "Back"
        backButton.SetActive(true);
    }

    // Metode yang akan dipanggil oleh tombol "Back" di UI
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

