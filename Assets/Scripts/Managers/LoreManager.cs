using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : Interactable
{
    [Header("Panel Lore")]
    public GameObject loreGameObject;    // Referensi ke GameObject Lore

    protected override void Update()
    {
        base.Update();
        if (playerInRange && InputManager.GetInstance().GetInteractPressed())
        {
            // Jika tombol "Submit" ditekan, aktifkan atau nonaktifkan game object Lore
            if (loreGameObject != null)
            {
                loreGameObject.SetActive(!loreGameObject.activeSelf);
            }
        }
    }

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(other);
        }
    }

    override protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            base.OnTriggerExit2D(other);
            // Jika pemain menjauhi objek interaktif, nonaktifkan game object Lore
            if (loreGameObject != null)
            {
                loreGameObject.SetActive(false);
            }
        }
    }
}
