using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollider : MonoBehaviour
{
    [Header("Ink Json")]
    [SerializeField] private TextAsset inkJSON;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Memulai dialog saat pemain bersentuhan
            DialogueManager.GetInstance()?.EnterDialogueMode(inkJSON);
            gameObject.SetActive(false);

        }
    }
}
