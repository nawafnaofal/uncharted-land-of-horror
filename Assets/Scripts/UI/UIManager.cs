using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    Player player;  // Referensi ke pemain
    Inventory playersInventory;  // Referensi ke inventaris pemain
    public GameObject heartContainer;  // Kontainer untuk representasi hati
    public GameObject pausePanel;  // Panel jeda permainan
    public GameObject losePanel;  // Panel untuk menangani kekalahan
    public GameObject winPanel;
    public GameObject fadeOutImage;
    public Button retryButton;  // Tombol Retry pada LosePanel
    public Button quitButton;   // Tombol Quit pada LosePanel
    public TextMeshProUGUI moneyText;  // Teks untuk menampilkan uang
    public Slider magicBar;  // Slider untuk menampilkan kemampuan khusus pemain
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI commonKeysText;
    public TextMeshProUGUI uncommonKeysText;
    public TextMeshProUGUI bossKeysText;

    public List<Image> hearts;  // Daftar gambar hati
    public Sprite fullHeart;  // Sprite hati penuh
    public Sprite halfHeart;  // Sprite setengah hati
    public Sprite emptyHeart;  // Sprite hati kosong



    public static UIManager instance;

    void Awake()
    {
        instance = this; 
                        
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();  // Mencari objek pemain di dalam adegan
        player.DamageTaken += UpdateHearts;  // Menyambungkan metode UpdateHearts dengan acara DamageTaken pemain
        player.HealthGiven += UpdateHearts;  // Menyambungkan metode UpdateHearts dengan acara HealthGiven pemain
        playersInventory = player.GetComponent<Inventory>();  // Mendapatkan komponen Inventory dari pemain
        playersInventory.MoneyChanged += UpdateMoney;  // Menyambungkan metode UpdateMoney dengan acara MoneyChanged dari inventaris pemain
        playersInventory.CommonKeyChanged += UpdateCommonKey;
        playersInventory.UncommonKeyChanged += UpdateUncommonKey;
        playersInventory.BossKeyChanged += UpdateBossKey;
        playersInventory.AmmoChanged += UpdateAmmo;  // Menyambungkan metode UpdateAmmo dengan acara AmmoChanged dari inventaris pemain
        playersInventory.MagicChanged += UpdateMagic;  // Menyambungkan metode UpdateMagic dengan acara MagicChanged dari inventaris pemain
        playersInventory.HeartAmountChanged += UpdateHeartContainer;  // Menyambungkan metode UpdateHeartContainer dengan acara HeartAmountChanged dari inventaris pemain
        InitHearts();  // Memanggil metode InitHearts untuk menginisialisasi representasi hati
        UpdateHearts();  // Memanggil metode UpdateHearts untuk memperbarui representasi hati
        UpdateMoney();  // Memanggil metode UpdateMoney untuk memperbarui tampilan uang
        InitMagic();  // Memanggil metode InitMagic untuk menginisialisasi tampilan kemampuan khusus
        UpdateMagic();  // Memanggil metode UpdateMagic untuk memperbarui tampilan kemampuan khusus

        commonKeysText.text = playersInventory.commonKeys.ToString() + "x";
        uncommonKeysText.text = playersInventory.uncommonKeys.ToString() + "x";
        bossKeysText.text = playersInventory.bossKeys.ToString() + "x";
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))  // Memeriksa tombol Pause ditekan
        {
            if (!pausePanel.activeInHierarchy)  // Jika panel jeda permainan tidak aktif
            {
                Pause();  // Panggil metode Pause
            }
            else
            {
                UnPause();  // Panggil metode UnPause
            }
        }
    }


    void InitHearts()
    {
        int maximumHearts = (int)player.maxHealth / 2;  // Menghitung jumlah hati maksimum berdasarkan kesehatan maksimum pemain
        for (int i = 0; i < maximumHearts; i++)
        {
            if (i >= hearts.Count)
            {
                GameObject heartImage = Instantiate(new GameObject("HeartImage"), heartContainer.transform);  // Membuat objek gambar hati baru
                Image image = heartImage.AddComponent<Image>();  // Menambahkan komponen gambar ke objek hati
                image.sprite = fullHeart;  // Mengatur sprite hati sebagai hati penuh
                RectTransform t = heartImage.GetComponent<RectTransform>();
                t.sizeDelta = new Vector2(30, 30);  // Mengatur lebar dan tinggi dari RectTransform
                hearts.Add(image);  // Menambahkan gambar hati ke daftar gambar hati
            }
        }
    }

    void InitMagic()
    {
        magicBar.maxValue = playersInventory.maxAmmo;  // Mengatur nilai maksimum slider kemampuan khusus
        magicBar.value = playersInventory.currentAmmo;  // Mengatur nilai slider kemampuan khusus saat ini
    }

    void InitAmmo()
    {
        Debug.LogError("belum ada method nya");  
    }

    void UpdateHearts()
    {
        float tempHealth = player.currHealth / 2;  // Menghitung jumlah hati berdasarkan kesehatan pemain
        for (int currentHeart = 0; currentHeart < hearts.Count; currentHeart++)
        {
            if (currentHeart <= tempHealth - 1)
            {
                hearts[currentHeart].sprite = fullHeart;  // Mengatur gambar hati menjadi hati penuh
            }
            else if (currentHeart >= tempHealth)
            {
                hearts[currentHeart].sprite = emptyHeart;  // Mengatur gambar hati menjadi hati kosong
            }
            else
            {
                hearts[currentHeart].sprite = halfHeart;  // Mengatur gambar hati menjadi setengah hati
            }
        }
    }

    void UpdateHeartContainer()
    {
        InitHearts();  // Memanggil metode InitHearts untuk menginisialisasi kembali representasi hati
        UpdateHearts();  // Memanggil metode UpdateHearts untuk memperbarui representasi hati
    }

    void UpdateMoney()
    {
        if (moneyText != null)
            moneyText.text = playersInventory.money.ToString("0");  // Memperbarui teks uang
    }

    void UpdateCommonKey()
    {
        if (commonKeysText != null)
            commonKeysText.text =  playersInventory.commonKeys.ToString() + "x";  // Memperbarui teks jumlah kunci biasa
    }
     void UpdateUncommonKey()
    {
        if (uncommonKeysText != null)
            uncommonKeysText.text =  playersInventory.uncommonKeys.ToString() + "x";  // Memperbarui teks jumlah kunci tidak biasa
    }

    void UpdateBossKey()
    {
        if (bossKeysText != null)
            bossKeysText.text =  playersInventory.bossKeys.ToString() + "x";
    }

    void UpdateAmmo()
    {
        if (magicBar != null)
            magicBar.value = playersInventory.currentAmmo;  // Memperbarui nilai slider kemampuan khusus
            ammoText.text = playersInventory.currentAmmo.ToString("0") + "x";
    }

    void UpdateMagic()
    {
        if (magicBar != null)
            magicBar.value = playersInventory.currentAmmo;  // Memperbarui nilai slider kemampuan khusus
    }

    public void Pause()
    {
        Time.timeScale = 0;  // Menghentikan waktu dalam permainan
        pausePanel.SetActive(true);  // Mengaktifkan panel jeda permainan
    }

    public void UnPause()
    {
        Time.timeScale = 1;  // Mengatur waktu dalam permainan kembali ke kecepatan normal
        pausePanel.SetActive(false);  // Menonaktifkan panel jeda permainan
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
        print("Quit Game");  // Mencetak pesan ke konsol
    }

    public void ShowLosePanel()
    {
        //Time.timeScale = 0;  // Menghentikan waktu dalam permainan

        // Menampilkan LosePanel dan mengaktifkan tombol Retry dan Quit
        losePanel.SetActive(true);
        retryButton.interactable = true;
        quitButton.interactable = true;
        
    }

    public void Retry()
    {
        // Menyembunyikan LosePanel
        losePanel.SetActive(false);

        // Menghidupkan kembali pemain dan mengatur kesehatannya penuh kembali
        player.Respawn();
        player.Heal((int)player.maxHealth);
        // Mengaktifkan kembali objek pemain
        player.gameObject.SetActive(true);

        Time.timeScale = 1;  // Mengatur waktu dalam permainan kembali ke kecepatan normal
    }

    public void BossDefeated()
    {
        // Tampilkan teks "TAMAT" (Anda mungkin perlu menambahkan objek teks pada UI)
        StartCoroutine(ShowGameOverText());

    }

    IEnumerator ShowGameOverText()
    {
        // Tampilkan animasi fade-out
        FadeOut();

        // Tunggu beberapa detik sebelum menampilkan panel TAMAT
        yield return new WaitForSeconds(2f);

        // Tampilkan panel TAMAT
        winPanel.SetActive(true);

        // Tunggu beberapa detik sebelum menampilkan animasi fade-in
        yield return new WaitForSeconds(2f);

        // Sembunyikan panel TAMAT
        winPanel.SetActive(false);

        // Tampilkan animasi fade-in
        FadeIn();

        // Tunggu beberapa detik sebelum pindah ke panel credit
        yield return new WaitForSeconds(2f);

        // Pindah ke panel credit
        SceneManager.LoadScene("CreditScene");
    }

    void FadeOut()
    {
        // Menonaktifkan kontrol pergerakan pemain selama fade-out
        player.GetComponent<PlayerMovement>().enabled = false;

        // Memutar trigger "Fade" pada Animator yang berada dalam GameObject yang memiliki tag "Transition"
        GameObject.FindWithTag("Transition").GetComponentInParent<Animator>().SetTrigger("Fade");
    }

    void FadeIn()
    {
        // Mengaktifkan kembali kontrol pergerakan pemain setelah fade-in
        player.GetComponent<PlayerMovement>().enabled = true;

        // Memutar trigger "FadeIn" pada Animator yang berada dalam GameObject yang memiliki tag "Transition"
        GameObject.FindWithTag("Transition").GetComponentInParent<Animator>().SetTrigger("Fade");
    }


}
