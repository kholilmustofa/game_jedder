using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossController : MonoBehaviour
{
    [Header("Boss Stats")]
    [SerializeField] private int maxHealth = 200;
    private int currentHealth;
    private bool isDead = false;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float chaseRadius = 10f;   // Jarak deteksi mulai mengejar Player
    [SerializeField] private float attackRadius = 1.8f;  // Jarak jangkauan serangan melee Boss

    [Header("Combat Settings")]
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private float attackCooldown = 2.5f;     // Jeda antar serangan
    [SerializeField] private float delayBeforeDamage = 0.5f;   // Jeda sinkronisasi hantaman animasi
    [SerializeField] private float delayAfterDamage = 0.6f;    // Jeda pemulihan setelah hantaman
    private float nextAttackTime;
    private bool isAttacking = false;

    [Header("Phase 2 (Rage Mode) Settings")]
    [SerializeField] private bool enablePhase2 = true;
    [SerializeField] private float phase2HPPercent = 0.5f;        // Masuk fase 2 jika darah di bawah 50%
    [SerializeField] private float phase2SpeedMultiplier = 1.5f;  // Kecepatan meningkat 1.5x
    [SerializeField] private float phase2CooldownMultiplier = 0.7f; // Cooldown lebih cepat (0.7x)
    [SerializeField] private int phase2DamageBonus = 10;          // Tambahan damage pada fase 2
    [SerializeField] private Color phase2SpriteColor = new Color(1f, 0.6f, 0.6f); // Warna sprite memerah/marah
    private bool isInPhase2 = false;

    [Header("UI References")]
    [SerializeField] private Slider healthBarSlider; // Drag Slider HP Boss di sini

    [Header("Audio Settings")]
    [SerializeField] private AudioSource attackAudioSource;
    [SerializeField] private AudioSource hitAudioSource;
    [SerializeField] private AudioSource rageAudioSource; // Suara raungan masuk fase 2
    [SerializeField] private AudioSource deathAudioSource;

    [Header("Animation States")]
    [SerializeField] private string idleAnimationName = "Idle";
    [SerializeField] private string walkAnimationName = "walk";
    [SerializeField] private string attackAnimationName = "attack";
    [SerializeField] private string deathAnimationName = "death";

    [Header("Victory Trigger Settings")]
    [SerializeField] private bool triggerVictoryOnDeath = true;
    [SerializeField] private float victoryDelay = 2.5f; // Jeda setelah Boss mati baru memunculkan Victory Panel

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private Color originalSpriteColor = Color.white;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        // Pastikan konfigurasi Rigidbody2D benar untuk game top-down 2D
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
        isInPhase2 = false;

        if (spriteRenderer != null)
        {
            originalSpriteColor = spriteRenderer.color;
        }

        if (healthBarSlider != null)
        {
            healthBarSlider.gameObject.SetActive(true); // Tampilkan health bar Boss
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
        if (isDead || playerTransform == null || playerHealth == null) return;

        // Jika Player sudah mati, Boss kembali diam
        if (playerHealth.IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.Play(idleAnimationName);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Jangan bergerak atau memproses AI jika sedang menyerang
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            FacePlayer();
            return;
        }

        if (distanceToPlayer <= attackRadius)
        {
            // Jarak serang tercapai: Berhenti bergerak dan serang jika cooldown selesai
            rb.linearVelocity = Vector2.zero;
            FacePlayer();

            if (Time.time >= nextAttackTime)
            {
                StartCoroutine(AttackCoroutine());
            }
            else
            {
                if (animator != null) animator.Play(idleAnimationName);
            }
        }
        else if (distanceToPlayer <= chaseRadius)
        {
            // Kejar Player
            MoveTowardsPlayer();
        }
        else
        {
            // Di luar jarak kejar: Diam
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.Play(idleAnimationName);
        }
    }

    private void FacePlayer()
    {
        if (playerTransform == null || spriteRenderer == null) return;

        if (playerTransform.position.x < transform.position.x)
            spriteRenderer.flipX = true; // Hadap kiri
        else
            spriteRenderer.flipX = false; // Hadap kanan
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        float currentSpeed = moveSpeed;

        // Terapkan peningkatan kecepatan di Fase 2
        if (isInPhase2)
        {
            currentSpeed *= phase2SpeedMultiplier;
        }

        rb.linearVelocity = direction * currentSpeed;

        // Arah hadap saat berlari
        if (spriteRenderer != null)
        {
            if (direction.x < 0)
                spriteRenderer.flipX = true;
            else if (direction.x > 0)
                spriteRenderer.flipX = false;
        }

        if (animator != null)
        {
            animator.Play(walkAnimationName);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        // Hitung cooldown serangan
        float currentCooldown = attackCooldown;
        if (isInPhase2)
        {
            currentCooldown *= phase2CooldownMultiplier;
        }
        nextAttackTime = Time.time + currentCooldown;

        if (attackAudioSource != null)
        {
            attackAudioSource.Play();
        }

        if (animator != null)
        {
            animator.Play(attackAnimationName);
        }

        // Tunggu hingga hantaman visual animasi terjadi
        yield return new WaitForSeconds(delayBeforeDamage);

        // Berikan damage jika Player masih dalam jangkauan
        if (playerTransform != null && Vector2.Distance(transform.position, playerTransform.position) <= attackRadius + 0.5f)
        {
            if (playerHealth != null && !playerHealth.IsDead)
            {
                int currentDamage = attackDamage;
                if (isInPhase2)
                {
                    currentDamage += phase2DamageBonus;
                }
                playerHealth.TakeDamage(currentDamage);
                Debug.Log($"Boss menghantam Player! Memberikan {currentDamage} damage.");
            }
        }

        // Tunggu sisa animasi selesai
        yield return new WaitForSeconds(delayAfterDamage);

        if (animator != null && !isDead)
        {
            animator.Play(idleAnimationName);
        }

        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Boss menerima {damage} damage. HP Sekarang: {currentHealth}/{maxHealth}");

        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        if (hitAudioSource != null)
        {
            hitAudioSource.Play();
        }

        // Cek transisi ke Fase 2 (Rage Mode)
        if (enablePhase2 && !isInPhase2 && currentHealth <= (maxHealth * phase2HPPercent))
        {
            StartCoroutine(EnterPhase2Coroutine());
        }
        else
        {
            // Flash merah biasa jika tidak masuk fase 2
            if (spriteRenderer != null && gameObject.activeInHierarchy)
            {
                StartCoroutine(FlashRedCoroutine());
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator EnterPhase2Coroutine()
    {
        isInPhase2 = true;
        Debug.Log("BOSS MASUK FASE 2: RAGE MODE!");

        if (rageAudioSource != null)
        {
            rageAudioSource.Play();
        }

        // Efek visual masuk ke fase 2 (berkedip cepat ungu/merah)
        if (spriteRenderer != null)
        {
            for (int i = 0; i < 5; i++)
            {
                spriteRenderer.color = new Color(0.8f, 0.2f, 0.8f); // Ungu kemarahan
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = originalSpriteColor;
                yield return new WaitForSeconds(0.1f);
            }
            
            // Setel warna sprite permanen di Fase 2
            spriteRenderer.color = phase2SpriteColor;
        }
    }

    private IEnumerator FlashRedCoroutine()
    {
        if (spriteRenderer != null)
        {
            Color currentNormalColor = isInPhase2 ? phase2SpriteColor : originalSpriteColor;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = currentNormalColor;
        }
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; // Matikan gerakan fisik sepenuhnya

        // Matikan semua Collider agar Player tidak menabrak jasad Boss
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var col in colliders)
        {
            col.enabled = false;
        }

        if (deathAudioSource != null)
        {
            deathAudioSource.Play();
        }

        if (animator != null)
        {
            animator.Play(deathAnimationName);
        }

        Debug.Log("BOSS DIKALAHKAN!");

        // Sembunyikan Slider HP Boss
        if (healthBarSlider != null)
        {
            Destroy(healthBarSlider.gameObject, victoryDelay);
        }

        if (triggerVictoryOnDeath)
        {
            StartCoroutine(VictorySequenceCoroutine());
        }

        // Hancurkan objek Boss setelah animasi selesai
        Destroy(gameObject, victoryDelay + 1f);
    }

    private IEnumerator VictorySequenceCoroutine()
    {
        yield return new WaitForSeconds(victoryDelay);

        GameplayMenuController menu = FindFirstObjectByType<GameplayMenuController>();
        if (menu != null)
        {
            // Buka kunci Level 2 (jika di Level 1) atau langsung munculkan panel sukses
            // Gunakan LevelCompleteTrigger untuk membuka kunci secara aman
            LevelCompleteTrigger lct = FindFirstObjectByType<LevelCompleteTrigger>();
            if (lct != null)
            {
                lct.UnlockNextLevel();
            }
            else
            {
                menu.ShowGameSuccess();
            }
        }
        else
        {
            Debug.LogWarning("GameplayMenuController tidak ditemukan di scene! Tidak bisa memicu layar kemenangan.");
        }
    }
}
