using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    readonly float thrust = 8.5f; // Mendeklarasikan konstanta thrust dengan nilai 8.5f.
    Rigidbody2D myRB; // Mendeklarasikan variabel myRB bertipe Rigidbody2D.

    private void Start()
    {
        myRB = GetComponent<Rigidbody2D>(); // Mengambil komponen Rigidbody2D dari objek ini dan menyimpannya di myRB.
    }

    public IEnumerator KnockBack(Transform otherTransform)
    {
        Vector2 forceDirection = transform.position - otherTransform.position; // Menghitung arah gaya dengan mengurangkan posisi objek ini dengan posisi objek lain.
        Vector2 force = forceDirection.normalized * thrust; // Menghitung gaya yang dinormalisasi dan dikalikan dengan thrust.
        myRB.velocity = force; // Mengatur kecepatan Rigidbody2D objek ini dengan gaya yang dihitung.
        yield return new WaitForSeconds(.15f); // Menunggu selama 0.15 detik.
        myRB.velocity = Vector2.zero; // Mengatur kecepatan menjadi nol.
        if (transform.CompareTag("Enemy")) // Jika objek ini memiliki tag "Enemy".
            GetComponent<Enemy>().ChangeState(EnemyState.Chase); // Mengganti status objek musuh menjadi "Chase".
        if (transform.CompareTag("Boss")) // Jika objek ini memiliki tag "Enemy".
            GetComponent<Boss>().ChangeState(BossState.Chase); // Mengganti status objek musuh menjadi "Chase".
        if (transform.CompareTag("Player")) // Jika objek ini memiliki tag "Player".
            GetComponent<Player>().ChangeState(PlayerState.Walk); // Mengganti status objek pemain menjadi "Walk".
    }
}
