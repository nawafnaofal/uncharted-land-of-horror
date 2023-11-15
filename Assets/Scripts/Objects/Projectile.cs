using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    public float damage = 1; // Mendefinisikan jumlah kerusakan yang akan diberikan oleh proyektil
    public float moveSpeed = 1; // Mendefinisikan kecepatan gerak proyektil
    public float lifetime = 5; // Mendefinisikan waktu hidup maksimal proyektil
    public float torque; // Mendefinisikan torsi proyektil
    protected float lifetimeTimer; // Timer untuk menghitung waktu hidup sisa
    protected Rigidbody2D rb; // Komponen Rigidbody2D untuk mengendalikan fisika proyektil
    public ParticleSystem particles; // Partikel efek saat proyektil dihancurkan

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Mengambil komponen Rigidbody2D dari proyektil
        lifetimeTimer = lifetime; // Mengatur timer waktu hidup
        rb.AddForce(transform.right.normalized * moveSpeed, ForceMode2D.Impulse); // Menerapkan gaya untuk menggerakkan proyektil
        rb.AddTorque(torque); // Menerapkan torsi ke proyektil
    }

    void Update()
    {
        lifetimeTimer -= Time.deltaTime; // Mengurangi timer waktu hidup
        if (lifetimeTimer <= 0 && gameObject.activeInHierarchy)
        {
            Destroy(gameObject); // Menghancurkan proyektil jika melebihi waktu hidup maksimal
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, gameObject); // Menyebabkan kerusakan pada objek yang dapat menerima kerusakan
            if (other.gameObject.GetComponent<Projectile>())
                return; // Jika proyektil bertabrakan dengan proyektil lain, tidak ada tindakan tambahan yang diperlukan
            else
                Destroy(); // Hancurkan proyektil setelah bertabrakan dengan objek yang dapat menerima kerusakan
        }
        else
        {
            Destroy(); // Hancurkan proyektil jika bertabrakan dengan objek selain yang dapat menerima kerusakan
        }
    }

    public virtual void TakeDamage(float damageTaken, GameObject damageGiver)
    {
        Destroy(); // Menghancurkan proyektil saat menerima kerusakan
    }

    public void Destroy()
    {
        Instantiate(particles, transform.position, Quaternion.identity); // Menciptakan efek partikel pada lokasi proyektil saat dihancurkan
        gameObject.SetActive(false); // Menonaktifkan game object proyektil
        Destroy(gameObject, 2); // Menghancurkan game object proyektil setelah 2 detik
    }
}
