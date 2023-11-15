using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour, IDamageable
{
    // Komponen Animator untuk mengatur animasi pot
    Animator anim;
    // GameObject yang mewakili isi dari pot
    public GameObject contents;
    // Komponen Collectables untuk mengelola item yang dapat diambil
    Collectables newItem;

    void Start()
    {
        // Mendapatkan komponen Animator dari objek pot
        anim = GetComponent<Animator>();
        // Mendapatkan komponen Collectables dari objek pot
        newItem = GetComponent<Collectables>();
    }

    public void TakeDamage(float damageTaken, GameObject damageGiver)
    {
        // Memanggil metode Destroy() saat pot menerima kerusakan
        Destroy();
        // Mengaktifkan komponen Animator
        anim.enabled = true;
    }

    public void Destroy()
    {
        // Memainkan animasi penghancuran pot
        anim.SetBool("Destroyed", true);
        // Memanggil metode Disable() dengan penundaan 2 detik
        Invoke("Disable", 2f);

        // Memeriksa apakah pot memiliki isi
        if (contents != null)
            // Memanggil metode SpawnItem() dengan konten pot sebagai argumen
            SpawnItem(contents);
        else
        {
            // Mengambil item acak dari komponen Collectables
            GameObject droppedItem = newItem.GetRandomItem();
            if (droppedItem != null)
                // Memanggil metode SpawnItem() dengan item yang diambil sebagai argumen
                SpawnItem(droppedItem);
        }

    }

    void SpawnItem(GameObject contents)
    {
        // Menginstansiasi objek yang mewakili isi pot pada posisi pot
        Instantiate(contents, transform.position, contents.transform.rotation);
    }

    void Disable()
    {
        // Mengatur kembali animasi penghancuran menjadi tidak aktif
        anim.SetBool("Destroyed", false);
        // Menonaktifkan objek pot
        gameObject.SetActive(false);
    }
}
