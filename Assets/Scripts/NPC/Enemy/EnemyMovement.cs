using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    Enemy thisEnemy;
    Transform target;
    Animator anim;
    Patrol patroller;
    Rigidbody2D rb;

    public float moveSpeed = 2f;
    public float chaseRadius;
    public float attackRadius;
    public AudioClip[] wakeUpSounds;

    Vector2 homePosition;
    bool playerInRange = false;

    AudioSource audioSource;

    void Awake()
    {
        homePosition = transform.position;
        target = GameObject.FindWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        GetComponentInChildren<CircleCollider2D>().radius = chaseRadius - .15f;
        thisEnemy = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        patroller = GetComponent<Patrol>();
        rb = GetComponent<Rigidbody2D>();

    }

    void OnEnable()
    {
        playerInRange = false;

        if (moveSpeed == 0)
            anim.SetBool("stationary", true);

        if ((Vector2)transform.position != homePosition)
            transform.position = homePosition;
        StartCoroutine(CheckDistance());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }

    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            if (playerInRange)
            {
                // Menghitung jarak antara musuh dan pemain
                float distFromTarget = Vector2.Distance(target.position, transform.position);

                // Blok ini menangani situasi di mana pemain berada dalam jarak "chaseRadius" musuh
                // tetapi tidak dalam jarak "attackRadius", dan pemain masih hidup.
                if (distFromTarget <= chaseRadius && distFromTarget > attackRadius && target.gameObject.activeInHierarchy)
                {
                    if (patroller != null)
                    {
                        // Menghentikan perilaku patroli jika ada
                        patroller.patroling = false;
                    }

                    if (thisEnemy.currentState == EnemyState.Idle)
                    {
                        float delay = 0;

                        // Memicu animasi "Wakeup" jika musuh dalam status "Idle"
                        anim.SetTrigger("Wakeup");

                        // Memainkan suara "Wakeup" jika tersedia
                        if (wakeUpSounds.Length > 0)
                        {
                            audioSource.PlayOneShot(wakeUpSounds[Random.Range(0, wakeUpSounds.Length)]);
                        }

                        yield return new WaitForEndOfFrame();

                        // Mendapatkan informasi tentang klip animasi saat ini
                        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
                        delay = clips[0].clip.length;

                        // Menunggu hingga animasi "Wakeup" selesai
                        yield return new WaitForSeconds(delay);

                        // Mengubah status musuh menjadi "Chase"
                        thisEnemy.ChangeState(EnemyState.Chase);
                    }
                    else if (thisEnemy.currentState == EnemyState.Patrol)
                    {
                        // Mengubah status musuh menjadi "Chase" jika sebelumnya dalam status "Patrol"
                        thisEnemy.ChangeState(EnemyState.Chase);
                    }
                    else if (thisEnemy.currentState == EnemyState.Chase)
                    {
                        // Memindahkan musuh menuju pemain dalam status "Chase"
                        Vector2 temp = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                        UpdateAnimation((temp - (Vector2)transform.position).normalized);
                        rb.MovePosition(temp);
                    }
                }
                else if (distFromTarget <= attackRadius)
                {
                    if (thisEnemy.shooter != null)
                    {
                        // Memicu penembakan jika musuh memiliki komponen penembak
                        thisEnemy.shooter.firing = true;
                        thisEnemy.ChangeState(EnemyState.Attack);
                        UpdateAnimation((target.position - transform.position).normalized);
                    }
                    else if (thisEnemy.currentState == EnemyState.Chase)
                    {
                        // Mengubah status musuh menjadi "Attack" jika sebelumnya dalam status "Chase"
                        thisEnemy.ChangeState(EnemyState.Attack);
                        StartCoroutine(Attack());
                    }
                }
            }
            // Situasi di mana musuh berada jauh dari pemain atau pemain sudah mati
            if (!playerInRange || !target.gameObject.activeInHierarchy)
            {
                if (thisEnemy.currentState != EnemyState.Stagger)
                {
                    if (patroller != null)
                    {
                        // Mengaktifkan kembali perilaku patroli jika ada
                        patroller.patroling = true;

                        if (thisEnemy.currentState == EnemyState.Idle)
                        {
                            // Memicu animasi "Wakeup" jika musuh dalam status "Idle"
                            anim.SetTrigger("Wakeup");

                            yield return new WaitForSeconds(.5f);
                        }

                        // Mengubah status musuh menjadi "Patrol"
                        thisEnemy.ChangeState(EnemyState.Patrol);

                        yield return null;
                    }

                    if (thisEnemy.shooter != null)
                    {
                        // Mematikan penembakan jika musuh memiliki komponen penembak
                        thisEnemy.shooter.firing = false;
                    }

                    // Memindahkan musuh kembali ke posisi awal (homePosition)
                    Vector2 temp = Vector2.MoveTowards(transform.position, homePosition, moveSpeed * Time.deltaTime);
                    rb.MovePosition(temp);

                    if ((Vector2)transform.position == homePosition && thisEnemy.currentState != EnemyState.Idle)
                    {
                        // Memicu animasi "Sleep" jika musuh sudah kembali ke posisi awal
                        anim.SetTrigger("Sleep");
                        thisEnemy.ChangeState(EnemyState.Idle);
                        yield return null;
                    }

                    // Memperbarui animasi berdasarkan arah pergerakan musuh
                    UpdateAnimation((temp - (Vector2)transform.position).normalized);
                }
            }

            // Menunggu hingga fixed update selanjutnya
            yield return new WaitForFixedUpdate();
        }
    }


    IEnumerator Attack()
    {
        float delay = 0;
        Vector2 temp = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        anim.SetTrigger("Attack");
        target.GetComponent<IDamageable>().TakeDamage(thisEnemy.baseAttack, gameObject);
        yield return new WaitForEndOfFrame();
        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
        delay = clips[0].clip.length;
        yield return new WaitForSeconds(Mathf.Max(.5f, delay));
        thisEnemy.ChangeState(EnemyState.Chase);
        UpdateAnimation((temp - (Vector2)transform.position).normalized);
    }

    void UpdateAnimation(Vector2 direction)
    {
        anim.SetFloat("moveX", direction.x);
        anim.SetFloat("moveY", direction.y);
    }
}
