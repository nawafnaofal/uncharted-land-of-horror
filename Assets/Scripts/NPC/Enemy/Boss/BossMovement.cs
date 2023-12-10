using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    Boss thisEnemy;
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

    [Header("Ink Json")]
    [SerializeField] private TextAsset inkJSON;

    private bool isDialoguePlaying = false;

    private Boss thisBoss;
    private float teleportCooldown = 5f; // Waktu pendingin teleportasi (misalnya 5 detik)
    private bool canTeleport = false;
    private bool teleporting = false;
    private float lastTeleportTime = 0f; // Menyimpan waktu terakhir teleportasi
    private Vector2 lastTeleportPosition; // Menyimpan posisi terakhir sebelum teleportasi
    public float teleportDelay = 2f; // Waktu penundaan setelah teleportasi sebelum menyerang
    public float teleportDistance = 5f; // Jarak teleportasi menjauh dari pemain
    private float distFromTarget = 0f;
    private bool hasTeleportedTwice = false; // Tambahkan variabel ini
    private int teleportCount = 0; // Menghitung berapa kali boss telah melakukan teleportasi
    public int maxTeleportCount = 3; // Jumlah maksimum teleportasi yang diizinkan sebelum kembali ke keadaan normal
    private bool bossIsTeleporting = false; // Menandakan apakah boss sedang dalam proses teleportasi

    private bool isDialogueTriggered = false;
    private bool isBossMovementEnabled = true;

    void Start()
    {
        thisBoss = GetComponent<Boss>();
        thisBoss.OnTakeDamage += HandleDamageTaken;
        ResetTeleportCount();
    }

    void HandleDamageTaken(float currentHealth, float damage)
    {
        if (currentHealth <= 0)
        {
            thisBoss.OnTakeDamage -= HandleDamageTaken;
            canTeleport = false;
            ResetTeleportCount();
            GameManager.instance.PlayerWin();
        }
        else if (currentHealth == 5 && !isDialogueTriggered)
        {
            isDialogueTriggered = true;
            isBossMovementEnabled = false;
            StartCoroutine(RunOpeningDialogue());
        }
    }

    void Update()
    {
        if (canTeleport && Time.time - lastTeleportTime >= teleportCooldown && !bossIsTeleporting && !isDialoguePlaying)
        {
            if (teleportCount < maxTeleportCount)
            {
                if (thisBoss.currentState == BossState.Idle && !hasTeleportedTwice)
                {
                    StartCoroutine(TeleportAwayFromPlayerAndDelay());
                }
                else
                {
                    StartCoroutine(TeleportToPlayerAndAttack());
                }
            }
            else
            {
                // Kembalikan boss ke keadaan normal setelah mencapai maksimum teleportasi
                ResetTeleportCount();
                ResetBossToInitialState();
            }
        }
    }

    private IEnumerator TeleportToPlayerAndAttack()
    {
        bossIsTeleporting = true;
        Vector2 teleportPosition = target.position;
        TeleportToPosition(teleportPosition);

        yield return new WaitForSeconds(teleportDelay);

        // Melakukan serangan
        Attack();

        // Kembalikan bos ke keadaan normal setelah serangan
        ResetBossToInitialState();

        bossIsTeleporting = false;
    }

    private void TeleportToPosition(Vector2 teleportPosition)
    {
        lastTeleportPosition = transform.position;
        transform.position = teleportPosition;
        Debug.Log("Boss melakukan teleportasi!");
        lastTeleportTime = Time.time;
        IncrementTeleportCount();
    }

    private void ResetBossToInitialState()
    {
        hasTeleportedTwice = false;
        thisBoss.ChangeState(BossState.Idle); // Gantilah dengan status awal yang sesuai
    }

    // Implementasi IncrementTeleportCount dan ResetTeleportCount seperti sebelumnya

    // Metode dan event handler lainnya seperti yang telah ada

    private IEnumerator TeleportAwayFromPlayerAndDelay()
    {
        bossIsTeleporting = true;
        Vector2 teleportPosition = CalculateTeleportPositionAwayFromPlayer();
        TeleportToPosition(teleportPosition);

        yield return new WaitForSeconds(teleportDelay);

        hasTeleportedTwice = true;
        bossIsTeleporting = false;
    }



    private Vector2 CalculateTeleportPositionAwayFromPlayer()
    {
        // Menghitung posisi yang lebih jauh dari pemain
        Vector2 playerDirection = ((Vector2)transform.position - (Vector2)target.position).normalized;
        Vector2 teleportPosition = (Vector2)target.position + playerDirection * teleportDistance;
        return teleportPosition;
    }

    private IEnumerator AttackAfterTeleport()
    {
        // Tunggu beberapa detik setelah teleportasi
        yield return new WaitForSeconds(teleportDelay);

        // Teleportasi kembali ke pemain dan serang
        transform.position = target.position;
        Debug.Log("Boss melakukan teleportasi dan menyerang!");

        // Melakukan serangan
        Attack();
    }

    private void TeleportToLastPosition()
    {
        // Teleportasi untuk menjauh dari pemain
        Vector2 teleportPosition = CalculateTeleportPositionAwayFromPlayer();
        transform.position = teleportPosition;
        Debug.Log("Boss melakukan teleportasi menjauh dari pemain!");

        // Tunggu beberapa saat sebelum melakukan teleportasi ketiga
        StartCoroutine(TeleportToChaseRadius());
    }

    private IEnumerator TeleportToChaseRadius()
    {
        // Tunggu beberapa saat sebelum melakukan teleportasi ketiga
        yield return new WaitForSeconds(teleportDelay);

        // Teleportasi ke posisi di sekitar chase radius
        Vector2 randomPosition = CalculateRandomPositionInChaseRadius();
        transform.position = randomPosition;
        Debug.Log("Boss melakukan teleportasi kembali ke posisi chase radius!");

        IncrementTeleportCount(); // Tambahkan teleportCount setelah teleportasi ketiga
    }

    private Vector2 CalculateRandomPositionInChaseRadius()
    {
        // Mendapatkan posisi awal (homePosition) bos
        Vector2 startPosition = homePosition;

        // Menghitung posisi acak di dalam lingkaran dengan jari-jari chaseRadius
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomRadius = Random.Range(0f, chaseRadius);
        Vector2 randomPosition = startPosition + new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * randomRadius;

        return randomPosition;
    }

    void Awake()
    {
        homePosition = transform.position;
        target = GameObject.FindWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        GetComponentInChildren<CircleCollider2D>().radius = chaseRadius - .15f;
        thisEnemy = GetComponent<Boss>();
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

    private IEnumerator CheckDistance()
    {
        while (true)
        {
            if (playerInRange)
            {
                // Menghitung jarak antara musuh dan pemain
                distFromTarget = Vector2.Distance(target.position, transform.position);

                // Blok ini menangani situasi di mana pemain berada dalam jarak "chaseRadius" musuh
                // tetapi tidak dalam jarak "attackRadius", dan pemain masih hidup.
                if (distFromTarget <= chaseRadius && distFromTarget > attackRadius && target.gameObject.activeInHierarchy)
                {
                    if (patroller != null)
                    {
                        // Menghentikan perilaku patroli jika ada
                        patroller.patroling = false;
                    }

                    if (thisEnemy.currentState == BossState.Idle)
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
                        thisEnemy.ChangeState(BossState.Chase);
                    }
                    else if (thisEnemy.currentState == BossState.Patrol)
                    {
                        // Mengubah status musuh menjadi "Chase" jika sebelumnya dalam status "Patrol"
                        thisEnemy.ChangeState(BossState.Chase);
                    }
                    else if (thisEnemy.currentState == BossState.Chase)
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
                        thisEnemy.ChangeState(BossState.Attack);
                        UpdateAnimation((target.position - transform.position).normalized);
                    }
                    else if (thisEnemy.currentState == BossState.Chase)
                    {
                        // Mengubah status musuh menjadi "Attack" jika sebelumnya dalam status "Chase"
                        thisEnemy.ChangeState(BossState.Attack);
                        StartCoroutine(Attack());
                    }
                }
            }
            // Situasi di mana musuh berada jauh dari pemain atau pemain sudah mati
            if (!playerInRange || !target.gameObject.activeInHierarchy)
            {
                if (thisEnemy.currentState != BossState.Stagger)
                {
                    if (patroller != null)
                    {
                        // Mengaktifkan kembali perilaku patroli jika ada
                        patroller.patroling = true;

                        if (thisEnemy.currentState == BossState.Idle)
                        {
                            // Memicu animasi "Wakeup" jika musuh dalam status "Idle"
                            anim.SetTrigger("Wakeup");

                            yield return new WaitForSeconds(.5f);
                        }

                        // Mengubah status musuh menjadi "Patrol"
                        thisEnemy.ChangeState(BossState.Patrol);

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

                    if ((Vector2)transform.position == homePosition && thisEnemy.currentState != BossState.Idle)
                    {
                        // Memicu animasi "Sleep" jika musuh sudah kembali ke posisi awal
                        anim.SetTrigger("Sleep");
                        thisEnemy.ChangeState(BossState.Idle);
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

    private IEnumerator Attack()
    {
        float delay = 0;
        Vector2 temp = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        anim.SetTrigger("Attack");
        target.GetComponent<IDamageable>().TakeDamage(thisEnemy.baseAttack, gameObject);
        yield return new WaitForEndOfFrame();
        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
        delay = clips[0].clip.length;
        yield return new WaitForSeconds(Mathf.Max(.5f, delay));
        thisEnemy.ChangeState(BossState.Chase);
        UpdateAnimation((temp - (Vector2)transform.position).normalized);
    }

    private void UpdateAnimation(Vector2 direction)
    {
        anim.SetFloat("moveX", direction.x);
        anim.SetFloat("moveY", direction.y);
    }

    void IncrementTeleportCount()
    {
        teleportCount++;
        if (teleportCount >= maxTeleportCount)
        {
            // Jika boss sudah melakukan jumlah maksimum teleportasi, kembalikan ke keadaan normal
            ResetTeleportCount();
            // ... Lakukan hal-hal lain untuk mengembalikan ke keadaan normal
        }
    }

    void ResetTeleportCount()
    {
        teleportCount = 0;
        canTeleport = false;
    }

    private IEnumerator RunOpeningDialogue()
    {
        // Nonaktifkan pergerakan boss saat dialog berlangsung
        if (!isBossMovementEnabled)
        {
            // Tambahan: nonaktifkan komponen Rigidbody atau skrip pergerakan jika ada
             rb.velocity = Vector2.zero;
             rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        

        DialogueManager.GetInstance()?.EnterDialogueMode(inkJSON);

        yield return new WaitUntil(() => !DialogueManager.GetInstance().dialogueIsPlaying);

        // Setelah dialog selesai, aktifkan kembali pergerakan boss
        isBossMovementEnabled = true;
        canTeleport = true;

        // Aktifkan kembali komponen Rigidbody atau skrip pergerakan jika ada
        rb.constraints = RigidbodyConstraints2D.None;
    }
}