using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
