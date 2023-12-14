using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Inventory : MonoBehaviour
{
    ContextClue context; // Komponen untuk menunjukkan petunjuk konteks
    GameObject itemToShow; // GameObject item yang akan ditampilkan dalam inventori
    TextMeshProUGUI dialogueText; // Teks yang digunakan untuk pesan dalam dialog
    bool receivedItem; // Menandakan apakah item telah diterima

    public GameObject dialogueBox; // GameObject yang berisi kotak dialog
    public List<Item> MyItems; // Daftar item dalam inventori
    public int commonKeys; // Jumlah kunci biasa dalam inventori
    public int uncommonKeys; // Jumlah kunci tidak biasa dalam inventori
    public int bossKeys; // Jumlah kunci bos dalam inventori
    public int money; // Jumlah uang dalam inventori
    public int currentAmmo, maxAmmo = 10; // Jumlah amunisi saat ini dan maksimum
    public event Action MoneyChanged; // Event yang terpicu saat jumlah uang berubah
    public event Action AmmoChanged; // Event yang terpicu saat jumlah amunisi berubah
    public event Action MagicChanged; // Event yang terpicu saat jumlah magic berubah
    public event Action HeartAmountChanged; // Event yang terpicu saat jumlah hati berubah
    public event Action CommonKeyChanged;
    public event Action UncommonKeyChanged;
    public event Action BossKeyChanged;

    private void Start()
    {
        context = GetComponentInChildren<ContextClue>(); // Mengambil komponen ContextClue
        MyItems = InfoManager.Instance.items; // Menginisialisasi daftar item dengan data dari InfoManager
        commonKeys = InfoManager.Instance.CommonKeys; // Mengambil jumlah kunci biasa dari InfoManager
        uncommonKeys = InfoManager.Instance.UnCommonKeys; // Mengambil jumlah kunci tidak biasa dari InfoManager
        bossKeys = InfoManager.Instance.BossKeys; // Mengambil jumlah kunci bos dari InfoManager
        money = InfoManager.Instance.Money; // Mengambil jumlah uang dari InfoManager
        currentAmmo = InfoManager.Instance.AmmoLeft; // Mengambil jumlah amunisi saat ini dari InfoManager
        dialogueText = dialogueBox.GetComponentInChildren<TextMeshProUGUI>(); // Mengambil komponen Teks dari dialogBox
    }

    private void Update()
    {
        if (receivedItem && Input.GetButtonDown("Submit"))
        {
            receivedItem = false; // Menandakan item telah diterima
            GetComponent<Animator>().SetBool("receiveItem", false); // Menghentikan animasi penerimaan item
            GetComponent<PlayerMovement>().enabled = true; // Mengaktifkan pergerakan pemain
            context.StopInteracting(); // Menghentikan interaksi dengan petunjuk konteks
            dialogueBox.SetActive(false); // Menonaktifkan kotak dialog
        }
    }

    public void ReceiveChestItem(GameObject itemToReceiveGO)
    {
        Item itemToReceive = itemToReceiveGO.GetComponent<Item>(); // Mengambil komponen Item dari item yang akan diterima

        dialogueText.text = itemToReceive.itemName + "\n" + itemToReceive.itemDescription; // Menampilkan pesan dialog

        itemToShow = itemToReceiveGO; // Menyimpan item yang akan ditampilkan

        MyItems.Add(itemToReceive); // Menambahkan item ke daftar inventori
        context.GetComponent<SpriteRenderer>().sprite = itemToReceive.contextImage; // Mengatur gambar konteks petunjuk
        Animate(); // Memulai animasi penerimaan item
    }

    public void ReceiveItem(GameObject itemToReceiveGO, int amount = 1)
    {
        Item itemToReceive = itemToReceiveGO.GetComponent<Item>(); // Mengambil komponen Item dari item yang akan diterima

        // Memeriksa tipe item dan mengambil tindakan yang sesuai berdasarkan tipe item
        switch (itemToReceive.type)
        {
            case ItemType.CommonKey:
                commonKeys += amount; // Menambahkan jumlah kunci biasa
                CommonKeyChanged?.Invoke();
                break;
            case ItemType.UncommonKey:
                uncommonKeys += amount; // Menambahkan jumlah kunci tidak biasa
                UncommonKeyChanged?.Invoke();
                break;
            case ItemType.BossKey:
                bossKeys += amount; // Menambahkan jumlah kunci bos
                BossKeyChanged?.Invoke();
                break;
            case ItemType.Heart:
                Player player = GetComponent<Player>(); // Mengambil komponen Player
                player.Heal(amount); // Menyembuhkan pemain
                break;
            case ItemType.Money:
                money += amount; // Menambahkan jumlah uang
                MoneyChanged?.Invoke(); // Memicu event perubahan uang
                break;
            case ItemType.Bomb:
                break;
            case ItemType.Stick:
                break;
            case ItemType.Arrow:
                currentAmmo += amount; // Menambahkan jumlah amunisi
                AmmoChanged?.Invoke(); // Memicu event perubahan amunisi
                break;
            case ItemType.MagicBottle:
                MagicChanged?.Invoke(); // Memicu event perubahan magic
                break;
            case ItemType.HeartContainer:
                GetComponent<Player>().maxHealth += amount; // Menambahkan kapasitas hati pemain
                HeartAmountChanged?.Invoke(); // Memicu event perubahan jumlah hati
                break;
            default:
                break;
        }

        if (itemToReceive.pickupSound != null)
        {
            SoundsManager.instance.GetComponent<AudioSource>().PlayOneShot(itemToReceive.pickupSound); // Memutar suara pengambilan item
        }
    }

    public void RemoveItem(ItemType itemToRemove, int amount = 1)
    {
        // Memeriksa tipe item dan mengambil tindakan yang sesuai berdasarkan tipe item
        switch (itemToRemove)
        {
            case ItemType.CommonKey:
                commonKeys -= amount; // Mengurangi jumlah kunci biasa
                CommonKeyChanged?.Invoke();
                break;
            case ItemType.UncommonKey:
                uncommonKeys -= amount; // Mengurangi jumlah kunci tidak biasa
                UncommonKeyChanged?.Invoke();
                break;
            case ItemType.BossKey:
                bossKeys -= amount; // Mengurangi jumlah kunci bos
                BossKeyChanged?.Invoke();
                break;
            case ItemType.Heart:
                break;
            case ItemType.Money:
                money -= amount; // Mengurangi jumlah uang
                MoneyChanged?.Invoke(); // Memicu event perubahan uang
                break;
            case ItemType.Bomb:
                break;
            case ItemType.Stick:
                break;
            case ItemType.Arrow:
                currentAmmo -= amount; // Mengurangi jumlah amunisi
                AmmoChanged?.Invoke(); // Memicu event perubahan amunisi
                break;
            case ItemType.HeartContainer:
                break;
            default:
                break;
        }
    }

    void Animate()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("receiveItem", true); // Memulai animasi penerimaan item
        GetComponent<PlayerMovement>().enabled = false; // Menonaktifkan pergerakan pemain
    }

    public void ShowItem()
    {
        context.GetComponent<SpriteRenderer>().enabled = true; // Mengaktifkan gambar konteks petunjuk
        ReceiveItem(itemToShow); // Menerima item
        dialogueBox.SetActive(true); // Mengaktifkan kotak dialog
        receivedItem = true; // Menandakan item telah diterima
    }
}
