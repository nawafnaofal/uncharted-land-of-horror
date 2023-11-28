using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Image goldIcon;
    public Image itemImage;
    private Button buyButton;
    public Item item;  // Tambahkan akses publik ke item

    private Inventory playerInventory;

    public void Initialize(Item newItem, Inventory newPlayerInventory, Button newBuyButton)
    {
        item = newItem;
        playerInventory = newPlayerInventory;
        buyButton = newBuyButton;

        //if (nameText != null) nameText.text = item.itemName;
        if (priceText != null) priceText.text = item.itemPrice.ToString("0");

        //// Pastikan Anda memberikan referensi Image untuk goldIcon dan itemImage
        //if (goldIcon != null) goldIcon.sprite = Resources.Load<Sprite>("goldIcon");
        //if (itemImage != null) itemImage.sprite = Resources.Load<Sprite>("ItemImage");

        if (buyButton != null)
        {
            // Menambahkan fungsi untuk tombol pembelian
            buyButton.onClick.AddListener(BuyItem);

            // Menonaktifkan tombol jika item sudah dibeli
            if (ItemAlreadyBought())
            {
                buyButton.interactable = false;
            }
        }
    }

    public void SetBuyButton(Button button)
    {
        buyButton = button;
    }

    public void SetPlayerInventory(Inventory inventory)
    {
        playerInventory = inventory;
    }

    public void BuyItem()
    {
        Debug.Log("Coba membeli item...");

        if (playerInventory != null)
        {
            Debug.Log("Inventory player ditemukan!");

            if (playerInventory.money >= item.itemPrice)
            {
                Debug.Log("Uang cukup untuk membeli item!");

                // Menghapus uang dari inventory sesuai dengan harga item
                playerInventory.RemoveItem(ItemType.Money, item.itemPrice);

                // Menggunakan ReceiveItem untuk menambah item yang dibeli ke inventory
                playerInventory.ReceiveItem(item.gameObject, 1);

                // Memproses manfaat item
                ProcessItemBenefits();

                Debug.Log("Item berhasil dibeli!");

                // Menonaktifkan tombol setelah pembelian
                buyButton.interactable = false;
            }
            else
            {
                Debug.Log("Uang tidak cukup untuk membeli item!");
            }
        }
        else
        {
            Debug.Log("Inventory player tidak ditemukan!");
        }
    }



    void ProcessItemBenefits()
    {
        switch (item.type)
        {
            case ItemType.Heart:
                FindObjectOfType<Player>().Heal(1);
                break;
                // Tambahkan case untuk jenis item lain jika diperlukan
        }
    }

    bool ItemAlreadyBought()
    {
        
        return false;
    }
}

