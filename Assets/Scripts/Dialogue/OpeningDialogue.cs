using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningDialogue : MonoBehaviour
{
    [Header("Ink Json")]
    [SerializeField] private TextAsset inkJSON;

    private void Start()
    {
        // Memulai dialog saat game object diaktifkan
        StartDialogue();
    }

    private void StartDialogue()
    {
        // Memulai dialog
        DialogueManager.GetInstance()?.EnterDialogueMode(inkJSON);
        StartCoroutine(WaitForDialogueToEnd());
    }

    private IEnumerator WaitForDialogueToEnd()
    {
        // Tunggu sampai dialog selesai
        yield return new WaitUntil(() => !DialogueManager.GetInstance().dialogueIsPlaying);

        // Jalankan perpindahan scene setelah dialog selesai
        SceneManager.LoadScene("Island");
    }
}
