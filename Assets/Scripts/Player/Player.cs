using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

// Enum yang mendefinisikan berbagai tipe pemain
public enum PlayerState
{
    Walk,
    Attack,
    Interact,
    Stagger
}

// Kelas yang mengatur perilaku pemain dan mengimplementasikan antarmuka IDamageable
public class Player : MonoBehaviour, IDamageable
{
    protected Rigidbody2D rb;    // Komponen Rigidbody2D untuk mengendalikan fisika pemain.
    SpriteRenderer sr;           // Komponen SpriteRenderer untuk mengendalikan penampilan pemain.
    public PlayerState currentState;   // Status pemain saat ini (misalnya: "Walk", "Attack", "Interact", "Stagger").
    public float maxHealth = 6f;    // Jumlah maksimum kesehatan pemain dalam permainan (default: 6).
    public float currHealth;         // Jumlah kesehatan saat ini pemain (akan berubah selama permainan).
    public float strength = 1f;     // Faktor kerusakan yang diberikan oleh pemain saat menyerang atau berinteraksi.
    public event Action DamageTaken; // Event yang dipicu ketika pemain menerima kerusakan.
    public event Action HealthGiven; // Event yang dipicu ketika pemain menerima penyembuhan.
    public GameObject deathFX;       // Efek khusus yang ditampilkan saat pemain mati dalam permainan.
    public int numberOfFlashes;      // Jumlah kali pemain akan berkedip saat dalam mode invulnerable.
    public Color flashColor;         // Warna yang digunakan saat pemain berkedip dalam mode invulnerable.
    public Vector3 lastDeathPosition;
    Color originalColor;            // Warna asli pemain sebelum berkedip dalam mode invulnerable.
    bool invulnerable = false;      // Menunjukkan apakah pemain sedang dalam mode invulnerable.

    private static bool hasSaveData = false;

    // Metode Start() dijalankan saat pemain diinisialisasi
    protected virtual void Start()
    {
        // Mengatur status awal pemain menjadi "Walk" saat permainan dimulai
        ChangeState(PlayerState.Walk);

        // Mengambil komponen Rigidbody2D yang terpasang pada objek pemain
        rb = GetComponent<Rigidbody2D>();

        // Mengambil komponen SpriteRenderer yang terpasang pada objek pemain
        sr = GetComponent<SpriteRenderer>();

        // Menyimpan warna asli pemain sebagai referensi
        originalColor = sr.color;

        // Mengambil data kesehatan pemain dari InfoManager (jika tersedia)
        if (InfoManager.Instance.PlayerHealth != 0)
            currHealth = InfoManager.Instance.PlayerHealth;
        else
            currHealth = maxHealth;

        // Mengambil data kesehatan maksimum pemain dari InfoManager (jika tersedia)
        if (InfoManager.Instance.PlayerMaxHealth != 0)
            maxHealth = InfoManager.Instance.PlayerMaxHealth;

        // Mengambil posisi pemain dari InfoManager (jika tersedia)
        if (InfoManager.Instance.NewPlayerPosition != Vector2.zero)
            transform.position = InfoManager.Instance.NewPlayerPosition;

        hasSaveData = Player.HasSaveData();
    }


    // Metode untuk menghancurkan objek pemain
    public void Destroy()
    {
        lastDeathPosition = transform.position;

        GameObject deathEffects;
        if (deathFX != null)
            deathEffects = Instantiate(deathFX, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    // Metode untuk menangani pemain saat menerima kerusakan
    public void TakeDamage(float damageTaken, GameObject damageGiver)
    {
        // Memeriksa jika pemain dalam mode invulnerabilitas
        if (invulnerable) return;

        // Memulai mode invulnerabilitas
        StartCoroutine(IFrames());

        // Mengurangi kesehatan pemain sesuai kerusakan yang diterima
        currHealth -= damageTaken;

        // Memainkan suara kerusakan pemain
        SoundsManager.instance.PlayClip(SoundsManager.Sound.PlayerDamaged);

        // Memanggil event DamageTaken
        DamageTaken?.Invoke();

        // Jika kesehatan pemain habis, panggil Destroy()
        if (currHealth <= 0f)
        {
            Destroy();
            UIManager.instance.ShowLosePanel();
        }
        else
        {
            Knockback kB = GetComponent<Knockback>();
            if (kB != null)
            {
                // Memulai knockback jika komponen Knockback terpasang
                StartCoroutine(kB.KnockBack(damageGiver.gameObject.transform));
                // Mengubah keadaan pemain menjadi Stagger
                GetComponent<PlayerMovement>().ChangeState(PlayerState.Stagger);
            }
        }
    }

    // Metode untuk memberikan pemain invulnerabilitas sementara
    IEnumerator IFrames()
    {
        invulnerable = true;
        int temp = 0;

        // Simpan warna asli pemain sebelum berkedip
        originalColor = sr.color;

        // Selama jumlah kedipan yang diperlukan
        while (temp < numberOfFlashes)
        {
            // Ubah warna pemain menjadi flashColor
            sr.color = flashColor;
            yield return new WaitForSeconds(.1f);

            // Kembalikan warna pemain ke warna aslinya
            sr.color = originalColor;
            yield return new WaitForSeconds(.1f);
            temp++;
        }

        // Pastikan warna pemain dikembalikan ke warna aslinya setelah berkedip selesai
        sr.color = originalColor;

        invulnerable = false;
    }

    // Metode untuk menyembuhkan pemain
    public void Heal(float healthGiven)
    {
        currHealth += healthGiven;

        // Menentukan batasan kesehatan berdasarkan apakah kesehatan maksimum adalah genap atau ganjil
        if (maxHealth % 2 == 0)
        {
            currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        }
        else
        {
            // Kesehatan maksimum ganjil, jadi satu hati tidak ditampilkan dan kesehatan maksimum dikurangi satu
            currHealth = Mathf.Clamp(currHealth, 0, maxHealth - 1);
        }

        // Memanggil event HealthGiven
        HealthGiven?.Invoke();
    }

    public void Respawn()
    {
        invulnerable = false;
        
        transform.position = lastDeathPosition; // Ganti dengan posisi awal yang sesuai
        sr.color = originalColor;
    }

    // Metode untuk mengubah keadaan pemain
    public void ChangeState(PlayerState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }

    public void IncreaseDamage(float value)
    {
        strength += value;
        Debug.Log("Damage increased by " + value + ". Current damage: " + strength);
    }


    public void IncreaseSpeed(float value)
    {
        // Assuming PlayerMovement.cs is attached to the same GameObject as Player.cs
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.Speed += value;
            Debug.Log("Speed increased by " + value + ". Current speed: " + playerMovement.Speed);
        }
        else
        {
            Debug.LogError("PlayerMovement component not found.");
        }
    }

    public void SavePlayer()
    {
        PlayerData dataToSave = new PlayerData(this);

        // Debug untuk melihat data sebelum disimpan
        Debug.Log("Data to save:\n" +
                  "currHealth: " + dataToSave.currHealth + "\n" +
                  "maxHealth: " + dataToSave.maxHealth + "\n" +
                  "strength: " + dataToSave.strength + "\n" +
                  "speed: " + dataToSave.speed + "\n" +
                  "money: " + dataToSave.money + "\n" +
                  "scene: " + dataToSave.sceneName + "\n" +
                  // Tambahkan item data lainnya
                  "position: (" + dataToSave.position[0] + ", " + dataToSave.position[1] + ", " + dataToSave.position[2] + ")");

        // Menyimpan data
        SaveSystem.SavePlayer(this);
        hasSaveData = true;
        Debug.Log("Player data saved successfully!");
    
}

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            Debug.Log("Loading player data...");

            currHealth = data.currHealth;
            Debug.Log("Current Health " + data.currHealth);
            maxHealth = data.maxHealth;
            strength = data.strength;

            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                Debug.Log("Player speed before load: " + playerMovement.Speed);
                playerMovement.Speed = data.speed;
                Debug.Log("Player speed after load: " + playerMovement.Speed);
            }

            Inventory playerInventory = GetComponent<Inventory>();
            if (playerInventory != null)
            {
                Debug.Log("Player money before load: " + playerInventory.money);
                playerInventory.money = data.money;
                Debug.Log("Player money after load: " + playerInventory.money);
                playerInventory.commonKeys = data.commonKeys;
                playerInventory.uncommonKeys = data.uncommonKeys;
                playerInventory.bossKeys = data.bossKeys;
                playerInventory.currentAmmo = data.currentAmmo;
                playerInventory.maxAmmo = data.maxAmmo;

                Debug.Log("Player inventory loaded: Money=" + playerInventory.money + ", CommonKeys=" + playerInventory.commonKeys);
            }

            Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
            transform.position = position;
            Debug.Log("Position: " + position);

            // Memeriksa dan memuat scene jika perlu
            if (!string.IsNullOrEmpty(data.sceneName) && data.sceneName != SceneManager.GetActiveScene().name)
            {
                SceneManager.LoadScene(data.sceneName);
                Debug.Log("Scene loaded: " + data.sceneName);
            }

            // Perbarui hasSaveData
            hasSaveData = true;

                Debug.Log("Loaded player data: " +
        "currHealth=" + data.currHealth +
        ", maxHealth=" + data.maxHealth +
        ", strength=" + data.strength +
        ", speed=" + data.speed +
        ", money=" + data.money +
        ", commonKeys=" + data.commonKeys +
        ", uncommonKeys=" + data.uncommonKeys +
        ", bossKeys=" + data.bossKeys +
        ", currentAmmo=" + data.currentAmmo +
        ", maxAmmo=" + data.maxAmmo +
        ", position=" + data.position[0] + ", " + data.position[1] + ", " + data.position[2] +
        ", sceneName=" + data.sceneName);
        }
    }


    public static bool HasSaveData()
    {
        return hasSaveData;
    }

   

}
