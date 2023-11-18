// UpgradeButton.cs

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    public Button upgradeButton;

    public int maxPurchases = 3; // Jumlah maksimal pembelian
    private int currentPurchases = 0;

    public int damageUpgradeValue = 1; // Jumlah peningkatan damage
    public float speedUpgradeValue = 1f; // Jumlah peningkatan movement speed


    private Inventory playerInventory;

    public void Initialize(int price, Inventory inventory)
    {
        upgradeButton.onClick.AddListener(TryPurchaseUpgrade);
        priceText.text = price.ToString("0");
        playerInventory = inventory;
    }

    private void TryPurchaseUpgrade()
    {
        if (playerInventory != null && currentPurchases < maxPurchases && playerInventory.money >= int.Parse(priceText.text))
        {
            // Lakukan upgrade sesuai tipe upgrade (damage atau movement speed)
            if (gameObject.name.Contains("Damage"))
            {
                UpgradeDamage();
            }
            else if (gameObject.name.Contains("Speed"))
            {
                UpgradeSpeed();
            }

            // Kurangi uang player dan tambah jumlah pembelian
            playerInventory.RemoveItem(ItemType.Money, int.Parse(priceText.text));
            currentPurchases++;

            // Nonaktifkan tombol jika mencapai jumlah pembelian maksimal
            if (currentPurchases >= maxPurchases)
            {
                upgradeButton.interactable = false;
            }
        }
    }



    private void UpgradeDamage()
    {

        FindObjectOfType<Player>().IncreaseDamage(damageUpgradeValue);
        Debug.Log("Damage upgraded!");
    }

    private void UpgradeSpeed()
    {
     
        FindObjectOfType<Player>().IncreaseSpeed(speedUpgradeValue);
        Debug.Log("Speed upgraded!");
    }


}
