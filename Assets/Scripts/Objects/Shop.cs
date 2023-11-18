using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : Interactable
{
    public GameObject shopPanel;         // Referensi ke GameObject panel shop
    public Button backButton;            // Referensi ke Button untuk menutup panel shop

    protected override void Start()
    {
        base.Start();
        // Menyembunyikan panel shop pada awal permainan
        shopPanel.SetActive(false);

        // Menambahkan fungsi untuk tombol Back
        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseShopPanel);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (playerInRange && Input.GetButtonDown("Submit"))
        {
            // Menampilkan atau menyembunyikan panel shop saat tombol "Submit" ditekan
            ToggleShopPanel();
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
            // Menyembunyikan panel shop saat pemain menjauhi objek interaktif
            shopPanel.SetActive(false);
        }
    }

    // Method untuk menampilkan atau menyembunyikan panel shop
    private void ToggleShopPanel()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
    }

    // Method untuk menutup panel shop
    private void CloseShopPanel()
    {
        shopPanel.SetActive(false);
    }
}
