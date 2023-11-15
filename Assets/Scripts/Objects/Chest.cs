using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    // Objek yang berisi isi peti.
    public GameObject contents;

    // Sprite yang digunakan ketika peti telah terbuka.
    public Sprite openSprite;

    // Menyimpan status peti apakah sudah terbuka atau belum.
    public bool isOpen;

    // ID unik untuk peti, digunakan untuk menyimpan status buka/tutup peti.
    string uniqueID;

    // Dipanggil saat objek ini diinisialisasi.
    override protected void Start()
    {
        // Memanggil fungsi Start() dari kelas dasar (Interactable).
        base.Start();

        // Membuat ID unik berdasarkan nama objek, posisi, dan scene saat ini.
        uniqueID = UnityEngine.SceneManagement.SceneManager.GetActiveScene() + name + transform.position;

        // Mengecek apakah peti telah terbuka sebelumnya berdasarkan ID unik.
        if (InfoManager.Instance.chests.TryGetValue(uniqueID, out bool temp))
        {
            isOpen = temp;

            // Jika peti terbuka, mengganti sprite dan menonaktifkan collider.
            if (isOpen)
            {
                GetComponent<SpriteRenderer>().sprite = openSprite;
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else
        {
            // Jika peti belum terbuka sebelumnya, menambahkannya ke InfoManager.
            InfoManager.Instance.chests.Add(uniqueID, isOpen);
        }
    }

    // Dipanggil setiap frame.
    protected override void Update()
    {
        // Mengecek apakah pemain dalam jangkauan dan menekan tombol "Submit" (biasanya "E" atau "Space").
        if (playerInRange && Input.GetButtonDown("Submit"))
        {
            if (!isOpen)
            {
                Interacting(); // Memanggil fungsi Interacting() jika peti belum terbuka.
            }
        }
    }

    // Fungsi yang dipanggil ketika interaksi terjadi (misalnya, pemain menekan tombol "Submit").
    protected override void Interacting()
    {
        // Memanggil fungsi Interacting() dari kelas dasar (Interactable).
        base.Interacting();

        // Menonaktifkan collider peti.
        GetComponent<BoxCollider2D>().enabled = false;

        // Mengaktifkan animator dan menandai peti sebagai terbuka.
        GetComponent<Animator>().enabled = true;
        isOpen = true;

        // Mengupdate status terbuka/tutup peti pada InfoManager.
        InfoManager.Instance.chests[uniqueID] = isOpen;
    }

    // Dipanggil ketika ada objek lain yang masuk ke dalam collider.
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        // Mengecek apakah peti belum terbuka sebelumnya dan ada objek yang masuk ke collider.
        if (!isOpen)
            base.OnTriggerEnter2D(other);
    }

    // Dipanggil ketika objek lain keluar dari collider.
    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }

    // Fungsi ini digunakan untuk memberikan item dari peti ke pemain.
    public void GiveItem()
    {
        // Jika isi peti belum ditentukan, mengambil item secara acak.
        if (contents == null)
        {
            contents = gameObject.GetComponent<Collectables>().GetRandomItem();
        }

        // Menambahkan item ke inventori pemain.
        player.GetComponent<Inventory>().ReceiveChestItem(contents);

        // Menonaktifkan animator peti.
        GetComponent<Animator>().enabled = false;
    }
}
