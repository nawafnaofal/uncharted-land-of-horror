using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// Enum yang mendefinisikan berbagai tipe item
public enum ItemType
{
    CommonKey,
    UncommonKey,
    BossKey,
    Heart,
    Money,
    Bomb,
    Stick,
    Arrow,
    MagicBottle,
    HeartContainer
}

// Kelas yang mengatur perilaku item
public class Item : MonoBehaviour
{
    // Gambar item yang ditampilkan dalam konteks item
    public Sprite contextImage;
    // Suara yang diputar saat item diambil
    public AudioClip pickupSound;
    // Kekuatan lompatan saat item diambil
    public float jumpStrength = 1;
    // Jumlah lompatan yang dapat dilakukan setelah mengambil item
    public int jumps = 1;

    // Prefab yang digunakan untuk menampilkan item saat diambil
    [Tooltip("Always 'CollectableContext' Prefab")]
    public GameObject itemDisplay;
    // Tipe item
    public ItemType type;
    // Nama item
    public string itemName;
    // Deskripsi item
    [Multiline]
    public string itemDescription;
    // Nilai item
    public int value = 1;

    // Metode yang dijalankan saat objek bersentuhan dengan item
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(DisplayItem(other));
    }

    // Metode untuk menampilkan item saat diambil
    IEnumerator DisplayItem(Collider2D other)
    {
        // Menonaktifkan render dan collider item yang sedang diambil
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        // Membuat objek tampilan item yang mengikuti pemain
        GameObject display = Instantiate(itemDisplay, other.transform.position + Vector3.up * 1.5f, transform.rotation, other.transform);
        display.GetComponent<SpriteRenderer>().sprite = contextImage;
        Collect(other.GetComponent<Inventory>());

        // Membuat objek tampilan item melompat sedikit
        display.transform.DOLocalJump(Vector3.zero, jumpStrength, jumps, 1);

        yield return new WaitForSeconds(1f);
        Destroy(display);
        Destroy(gameObject);
    }

    // Metode untuk mengumpulkan item dan menambahkannya ke inventori pemain
    public virtual void Collect(Inventory playerInventory)
    {
        playerInventory.ReceiveItem(gameObject, value);
    }
}
