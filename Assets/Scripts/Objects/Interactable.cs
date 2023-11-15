using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Biasanya digunakan untuk mengeksekusi tindakan tertentu saat berinteraksi.
    //public event System.Action<Sprite> MeInteractable;
    //public event System.Action NotInteracting;
    //public event System.Action Interacting;

    // Sprite yang digunakan sebagai konteks interaksi
    public Sprite contextImage;

    // Mendefinisikan variabel boolean yang menyimpan status pemain apakah dalam jangkauan interaksi atau tidak
    protected bool playerInRange = false;

    // GameObject yang merepresentasikan pemain dalam jangkauan interaksi
    protected GameObject player;

    // Mengatur ContextClue dari pemain
    ContextClue playerCC;

    // Filter yang digunakan untuk mendeteksi interaksi
    ContactFilter2D filter;

    // Metode Start() yang akan dieksekusi saat inisialisasi objek
    virtual protected void Start()
    {
        // Menginisialisasi filter untuk menggunakan trigger dan layer mask serta menentukan layerMask "Interactable" (256)
        filter.useTriggers = filter.useLayerMask = true;
        filter.layerMask = 256;
    }

    // Metode Update() yang dipanggil setiap frame
    virtual protected void Update()
    {
        // Memeriksa jika pemain dalam jangkauan dan menekan tombol "Submit" untuk berinteraksi
        if (playerInRange && Input.GetButtonDown("Submit"))
        {
            Interacting();
        }
    }

    // Ketika objek masuk dalam collider
    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        // Memeriksa apakah objek lain dengan tag "Player" masuk ke dalam jangkauan interaksi
        if (other.CompareTag("Player"))
        {
            // Mengatur playerInRange menjadi true dan mendapatkan GameObject player dan ContextClue dari player tersebut
            playerInRange = true;
            player = other.gameObject.GetComponentInParent<Player>().gameObject;
            playerCC = player.GetComponentInChildren<ContextClue>();
            MeInteractable();
        }
    }

    // Metode untuk menangani tindakan saat interaksi terjadi
    virtual protected void Interacting()
    {
        // Memeriksa jika playerCC tidak null, lalu memanggil metode Interacting dari playerCC
        if (playerCC != null)
            playerCC.Interacting();
        
        //Interacting?.Invoke();
    }

    // Metode untuk menampilkan interaksi objek
    virtual protected void MeInteractable()
    {
        // Jika playerCC tidak null, menampilkan konteks menggunakan contextImage
        if (playerCC != null)
            playerCC.ShowContext(contextImage);
        
        //MeInteractable?.Invoke(contextImage);
    }

    // Metode untuk menghentikan interaksi
    virtual protected void StopInteracting()
    {
        // Jika playerCC tidak null, memanggil metode StopInteracting dari playerCC
        if (playerCC != null)
            playerCC.StopInteracting();
        
        //NotInteracting?.Invoke();
    }

    // Ketika objek keluar dari collider
    virtual protected void OnTriggerExit2D(Collider2D other)
    {
        // Memeriksa jika objek lain keluar dari jangkauan interaksi
        if (other.CompareTag("Player"))
            playerInRange = false;

        // Memeriksa apakah objek masih bersentuhan dengan filter, jika tidak, maka memanggil StopInteracting()
        bool isTouching = other.IsTouching(filter);
        if (isTouching)
        {
            return;
        }
        StopInteracting();
    }
}
