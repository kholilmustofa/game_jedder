using UnityEngine;
using System.Collections;

public class EnemyMelee : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float chaseRadius = 5f;   // Jarak mulai mengejar Player
    [SerializeField] private float attackRadius = 0.7f; // Jarak untuk mulai menyerang Player

    [Header("Combat Settings")]
    [SerializeField] private int attackDamage = 15;
    [SerializeField] private float attackCooldown = 1.5f; // Jeda waktu antar serangan
    private float nextAttackTime;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private bool isAttacking = false;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        // Cari Player di awal game
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (playerTransform == null || playerHealth == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Abaikan gerakan jika sedang dalam animasi menyerang
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (distanceToPlayer <= attackRadius)
        {
            // Stop bergerak dan lakukan serangan jika cooldown selesai
            rb.linearVelocity = Vector2.zero;
            if (Time.time >= nextAttackTime)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
        else if (distanceToPlayer <= chaseRadius)
        {
            // Kejar player
            MoveTowardsPlayer();
        }
        else
        {
            // Diam (Idle)
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.Play("Idle");
        }
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        // Hitung arah ke Player
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // --- SISTEM SEPARASI (Mencegah Musuh Saling Menumpuk) ---
        Vector2 separation = Vector2.zero;
        Collider2D[] surroundingColliders = Physics2D.OverlapCircleAll(transform.position, 0.8f);
        int neighborsCount = 0;

        foreach (var col in surroundingColliders)
        {
            // Jika ada musuh lain, hitung gaya dorong menjauh
            if (col.gameObject != gameObject && col.CompareTag("Enemy"))
            {
                Vector2 pushDirection = (Vector2)transform.position - (Vector2)col.transform.position;
                float distance = pushDirection.magnitude;
                if (distance < 0.1f) distance = 0.1f;

                separation += pushDirection.normalized / distance;
                neighborsCount++;
            }
        }

        // Gabungkan arah ke player dengan arah pemisah
        Vector2 finalDirection = direction;
        if (neighborsCount > 0)
        {
            finalDirection = (direction + separation * 0.6f).normalized;
        }

        rb.linearVelocity = finalDirection * moveSpeed;

        // Putar wajah musuh mengikuti arah jalan
        if (spriteRenderer != null)
        {
            if (finalDirection.x < 0)
                spriteRenderer.flipX = true; // Hadap kiri
            else if (finalDirection.x > 0)
                spriteRenderer.flipX = false; // Hadap kanan
        }

        // Putar animasi jalan
        if (animator != null)
        {
            animator.Play("Run");
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        // Putar animasi serang
        if (animator != null)
        {
            animator.Play("Attack 1");
        }

        // Tunggu sedikit agar damage terasa pas saat animasi pedang mengayun (seperempat detik)
        yield return new WaitForSeconds(0.25f);

        // Cek lagi apakah player masih dalam jarak serang saat hit terjadi
        if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= attackRadius + 0.3f)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }

        // Tunggu hingga durasi animasi serang selesai (misal total 0.5 detik)
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
    }

    /// <summary>
    /// Fungsi untuk memberikan damage ke musuh dari peluru Player.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} terkena serangan! Sisa HP: {currentHealth}");

        // Feedback visual: Kedip merah saat kena hit
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRedCoroutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRedCoroutine()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} mati!");
        // Anda bisa menambahkan animasi mati atau efek partikel di sini sebelum dihancurkan
        Destroy(gameObject);
    }
}
