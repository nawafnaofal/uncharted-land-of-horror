using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public List<ShopItem> shopItems; // Isi dengan item-item dari panel toko
    public Inventory playerInventory; // Tambahkan referensi ke Inventory player
    public List<UpgradeButton> upgradeButtons;

    void Start()
    {
        Debug.Log("Player object found: " + (playerInventory != null ? "Yes" : "No"));
        // Inisialisasi tombol-tombol dan berikan referensi tombolnya ke ShopItem
        foreach (ShopItem shopItem in shopItems)
        {
            Button buyButton = shopItem.GetComponent<Button>();
            shopItem.Initialize(shopItem.item, playerInventory, buyButton);
        }

        foreach (UpgradeButton upgradeButton in upgradeButtons)
        {
            Button button = upgradeButton.GetComponent<Button>();
            upgradeButton.Initialize(int.Parse(upgradeButton.priceText.text), playerInventory);
        }
    }
}
