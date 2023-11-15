using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextClue : MonoBehaviour
{
    //Interactable[] interactables; //   Array untuk menyimpan objek yang dapat diinteraksi
    SpriteRenderer myRenderer; //   Komponen untuk merender sprite dari objek ini

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>(); //  Mengambil komponen SpriteRenderer dari objek ini

        //interactables = FindObjectsOfType<Interactable>(); //   Mencari semua objek Interactable dalam scene
        //foreach (Interactable interactable in interactables)
        //{
        //    interactable.MeInteractable += Interactable;
        //    interactable.NotInteracting += NotInteracting;
        //    interactable.Interacting += Interacting;
        //}
    }

    public void ShowContext(Sprite contextImage)
    {
        myRenderer.sprite = contextImage; //   Mengatur sprite yang akan ditampilkan
        myRenderer.enabled = true; //   Menampilkan sprite pada renderer
    }

    public void Interacting()
    {
        GetComponentInParent<Player>().ChangeState(PlayerState.Interact); //   Mengubah state pemain menjadi "Interact"
        myRenderer.enabled = !myRenderer.enabled; //   Mengubah visibilitas renderer
    }

    public void StopInteracting()
    {
        GetComponentInParent<Player>().ChangeState(PlayerState.Walk); //   Mengubah state pemain menjadi "Walk"
        myRenderer.enabled = false; //   Menyembunyikan renderer
    }
}
