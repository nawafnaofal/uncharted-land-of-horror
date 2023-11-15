using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PebbleProjectile : Projectile
{
    // Membuat variabel Vector2 untuk menyimpan kecepatan saat ini
    Vector2 currVelocity;

    // Metode Start() yang dijalankan saat objek diinisialisasi
    protected override void Start()
    {
        // Memanggil metode Start() dari kelas dasar (kelas Projectile)
        base.Start();

        // Menyimpan kecepatan saat ini dari rigidbody dalam variabel currVelocity
        currVelocity = rb.velocity;
    }

    // Metode TakeDamage untuk menangani ketika objek ini menerima kerusakan
    public override void TakeDamage(float damageTaken, GameObject damageGiver)
    {
        // Mengambil komponen Rigidbody2D dari objek ini dan membalikkan kecepatannya
        GetComponent<Rigidbody2D>().velocity = -currVelocity;
        // Mengatur ulang timer seumur hidup proyektil
        lifetimeTimer = lifetime;
    }
}
