using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyRanged : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 25;
    private int currentHealth;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float chaseRadius = 8f;   // Jarak mulai mengejar Player
    [SerializeField] private float attackRadius = 5f;  // Jarak tembak anak panah

    [Header("Combat Settings")]
    [SerializeField] private int attackDamage = 10; // Kerusakan dari anak panah
    [SerializeField] private float attackCooldown = 0f; // Jeda waktu antar tembakan
    [SerializeField] private float delayBeforeShoot = 1f; // Jeda sebelum menembak (sinkronisasi animasi)
    [SerializeField] private float delayAfterShoot = 1f;  // Jeda setelah menembak (selesai animasi)
    private float nextAttackTime;

    [Header("Ranged Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("UI Settings")]
    [SerializeField] private Slider healthBarSlider;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource attackAudioSource;

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

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }

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

        // Abaikan gerakan jika sedang dalam animasi menyerang/menembak
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            // Wajah tetap menghadap ke player saat menembak
            FacePlayer();
            return;
        }

        if (distanceToPlayer <= attackRadius)
        {
            // Stop bergerak dan lakukan tembakan jika cooldown selesai
            rb.linearVelocity = Vector2.zero;
            FacePlayer();
            if (Time.time >= nextAttackTime)
            {
                StartCoroutine(ShootCoroutine());
            }
            else
            {
                if (animator != null) animator.Play("Idle");
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

    private void FacePlayer()
    {
        if (playerTransform == null || spriteRenderer == null) return;

        // Hadap ke arah player
        if (playerTransform.position.x < transform.position.x)
            spriteRenderer.flipX = true; // Hadap kiri
        else
            spriteRenderer.flipX = false; // Hadap kanan
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

    private IEnumerator ShootCoroutine()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        // Putar animasi menyerang/menembak
        if (animator != null)
        {
            animator.Play("Shoot");
        }

        // Tunggu jeda sebelum anak panah keluar agar pas dengan visual animasi menarik busur
        yield return new WaitForSeconds(delayBeforeShoot);

        // Putar suara tembakan jika AudioSource terpasang (suara panah meluncur)
        if (attackAudioSource != null)
        {
            attackAudioSource.Play();
        }

        // Tembakkan anak panah
        if (arrowPrefab != null && firePoint != null && playerTransform != null)
        {
            // Hitung arah panah dari FirePoint ke Player
            Vector2 shootDir = (playerTransform.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            
            // Instansiasi anak panah dengan rotasi menghadap Player
            GameObject arrowObj = Instantiate(arrowPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
            if (arrowObj.TryGetComponent<EnemyArrow>(out var arrowComponent))
            {
                arrowComponent.SetDamage(attackDamage);
            }
            Debug.Log($"{gameObject.name} menembakkan anak panah ke arah Player dengan damage {attackDamage}!");
        }

        // Tunggu sisa durasi animasi menembak selesai sebelum musuh bisa jalan lagi
        yield return new WaitForSeconds(delayAfterShoot);

        if (animator != null)
        {
            animator.Play("Idle");
        }

        isAttacking = false;
    }

    /// <summary>
    /// Fungsi untuk memberikan damage ke musuh dari peluru Player.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} terkena serangan! Sisa HP: {currentHealth}");

        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

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
        Destroy(gameObject);
    }
}
