using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Nama adegan yang akan dimuat setelah transisi
    public string sceneToLoad;
    // Posisi awal pemain setelah masuk ke adegan baru
    public Vector2 playerStartPosition;

    // GameObject yang mewakili efek fade out saat transisi
    public GameObject fadeOutImage;
    // Waktu tunda sebelum transisi selesai
    public float fadeWait = .33f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Memeriksa apakah objek yang masuk ke area trigger adalah pemain
        if (other.CompareTag("Player"))
        {
            // Menyimpan posisi awal pemain ke InfoManager
            InfoManager.Instance.NewPlayerPosition = playerStartPosition;

            // Memperbarui statistik pemain
            InfoManager.Instance.UpdateStats();

            // Memulai transisi dengan fade
            StartCoroutine(FadeAndLoad(other));
        }
    }

    IEnumerator FadeAndLoad(Collider2D player)
    {
        // Menonaktifkan kontrol pergerakan pemain
        player.GetComponent<PlayerMovement>().enabled = false;

        // Memutar trigger "Fade" pada Animator yang berada dalam GameObject yang memiliki tag "Transition"
        GameObject.FindWithTag("Transition").GetComponentInParent<Animator>().SetTrigger("Fade");

        // Menunggu selama fadeWait
        yield return new WaitForSeconds(fadeWait);

        // Memuat adegan baru secara asinkron
        AsyncOperation loadNewScene = SceneManager.LoadSceneAsync(sceneToLoad);

        // Menunggu hingga proses pengisian adegan baru selesai
        while (!loadNewScene.isDone)
            yield return null;
    }
}
