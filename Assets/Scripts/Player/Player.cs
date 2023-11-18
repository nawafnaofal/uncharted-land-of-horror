using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
}
