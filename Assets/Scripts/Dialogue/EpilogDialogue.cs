using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EpilogDialogue : MonoBehaviour
{
    [Header("Ink Json")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Timeline")]
    [SerializeField] private PlayableDirector timeline;

    private void Awake()
    {
        StartCoroutine(StartDialogueCoroutine());
    }

    private IEnumerator StartDialogueCoroutine()
    {
        // Aktifkan game object
        gameObject.SetActive(true);

        // Pastikan DialogueManager diinisialisasi sebelum memanggil StartDialogue
        DialogueManager dialogueManager = DialogueManager.GetInstance();
        if (dialogueManager != null)
        {
            // Mulai dialog
            dialogueManager.EnterDialogueMode(inkJSON);

            timeline.Pause();
            // Tunggu sampai dialog selesai
            yield return new WaitUntil(() => !dialogueManager.dialogueIsPlaying);

            // Memastikan timeline ada sebelum mencoba memainkannya
            if (timeline != null)
            {
                // Memainkan timeline
                timeline.Play();
            }

            // Tunggu sampai timeline selesai (opsional)
            yield return new WaitForSeconds((float)timeline.duration);

            // Ganti scene setelah timeline selesai (opsional)
            SceneManager.LoadScene("CreditScene");
        }
        else
        {
            Debug.LogError("DialogueManager is not initialized.");
        }
    }
}
