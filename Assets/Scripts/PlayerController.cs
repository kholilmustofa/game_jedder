using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Combat Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private float facingMouseDuration = 0.5f; // Durasi MC menghadap kursor setelah menembak (detik)
    [SerializeField] private int bulletDamage = 10;             // Damage dasar peluru MC
    [SerializeField] private AudioSource shootAudioSource;      // Tarik AudioSource suara tembak ke sini
    [SerializeField] private GameObject spawnEffectPrefab;       // Tarik prefab efek debu (dust) ke sini
    [SerializeField] private Vector3 spawnOffset = Vector3.zero; // Offset posisi muncul efek debu
    private float nextFireTime;
    private float lastShootTime = -99f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Drowning Settings")]
    [SerializeField] private GameObject waterSplashPrefab; // Tarik prefab WaterSplash ke sini
    [SerializeField] private Vector3 waterSplashOffset = Vector3.zero; // Offset posisi cipratan air
    private bool isDrowning = false;

    private Vector2 moveInput;
    private Vector2 mousePos;
    private string currentAnimationState;
    private bool isAttackPressed = false;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null) mainCamera = Camera.main;
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (spawnEffectPrefab != null)
        {
            Instantiate(spawnEffectPrefab, transform.position + spawnOffset, Quaternion.identity);
        }

        // Tentukan nilai damage berdasarkan scene saat ini
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneName == "Level1")
        {
            // Reset ke damage dasar (10) saat memulai ulang dari Level 1
            PlayerPrefs.SetInt("PlayerDamage", 10);
            PlayerPrefs.Save();
        }

        // Load damage dari PlayerPrefs (default: 10)
        bulletDamage = PlayerPrefs.GetInt("PlayerDamage", 10);
        Debug.Log($"[PlayerController] Damage peluru saat ini: {bulletDamage}");
    }

    private void Update()
    {
        // Get mouse position in world space using the new Input System
        if (Mouse.current != null)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            mousePos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

            // Update secara real-time apakah klik kiri mouse sedang ditekan
            isAttackPressed = Mouse.current.leftButton.isPressed;
        }
    }

    private void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = moveInput * moveSpeed;

        // Update animations using state machine
        if (animator != null)
        {
            bool isMoving = moveInput.sqrMagnitude > 0.01f;
            bool isShooting = (lastShootTime >= 0f) && (Time.time - lastShootTime < facingMouseDuration);

            if (isMoving && isShooting)
            {
                PlayAnimation("Player_RunAndShoot");
            }
            else if (isMoving && !isShooting)
            {
                PlayAnimation("Player_Walk");
            }
            else if (!isMoving && isShooting)
            {
                PlayAnimation("Player_Shoot");
            }
            else
            {
                PlayAnimation("Player_Idle");
            }
        }

        // Handle Shooting Direction (Rotate FirePoint only)
        if (firePoint != null)
        {
            Vector2 lookDir = mousePos - (Vector2)firePoint.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Logika Menghadap (Flipping Sprite):
        if (spriteRenderer != null)
        {
            // 1. Jika sedang menekan tombol tembak, arahkan tubuh MC mengikuti arah kursor mouse (membidik)
            if (isAttackPressed)
            {
                if (mousePos.x < transform.position.x)
                {
                    spriteRenderer.flipX = true; // Hadap kiri ke arah mouse
                }
                else
                {
                    spriteRenderer.flipX = false; // Hadap kanan ke arah mouse
                }
            }
            // 2. Jika baru saja melepas tembakan (dalam 0.35 detik), KUNCI arah hadap terakhir (jangan ikuti mouse dan jangan balik dulu)
            else if ((lastShootTime >= 0f) && (Time.time - lastShootTime < 0.35f))
            {
                // Biarkan flipX tetap seperti saat terakhir menembak (mencegah flickering saat klik cepat)
            }
            // 3. Jika sedang tidak menembak/membidik, arahkan tubuh MC sesuai arah jalan (WASD)
            else if (moveInput.x != 0)
            {
                if (moveInput.x < 0)
                {
                    spriteRenderer.flipX = true; // Hadap kiri mengikuti gerakan
                }
                else
                {
                    spriteRenderer.flipX = false; // Hadap kanan mengikuti gerakan
                }
            }
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        // Cegah menembak saat game sedang dijeda
        if (Mathf.Approximately(Time.timeScale, 0f)) return;

        // Pastikan menembak hanya saat tombol ditekan (bukan saat dilepas)
        if (value.isPressed && Time.time >= nextFireTime)
        {
            Debug.Log("OnAttack triggered!");
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        lastShootTime = Time.time; // Catat waktu tembakan agar MC menghadap kursor saat menembak

        // Putar suara tembakan jika AudioSource terpasang
        if (shootAudioSource != null)
        {
            shootAudioSource.Play();
        }

        if (bulletPrefab != null && firePoint != null)
        {
            Debug.Log("Shooting Bullet!");
            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (bulletObj.TryGetComponent<Bullet>(out var bulletComponent))
            {
                bulletComponent.SetDamage(bulletDamage);
            }
        }
        else
        {
            Debug.LogWarning("Bullet Prefab or FirePoint is missing in the Inspector!");
        }
    }

    /// <summary>
    /// Fungsi publik untuk meningkatkan damage peluru Player secara permanen.
    /// </summary>
    public void UpgradeBulletDamage(int amount)
    {
        bulletDamage += amount;
        PlayerPrefs.SetInt("PlayerDamage", bulletDamage);
        PlayerPrefs.Save();
        Debug.Log($"Upgrade Bullet Damage! Damage baru: {bulletDamage} (Tersimpan ke PlayerPrefs)");
    }

    private void PlayAnimation(string newState)
    {
        if (animator == null) return;
        if (currentAnimationState == newState) return;

        animator.Play(newState);
        currentAnimationState = newState;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Deteksi jika menyentuh air (Tilemap / trigger ber-tag "Water")
        if (collision.CompareTag("Water") && !isDrowning)
        {
            StartCoroutine(DrownSequence());
        }
    }

    private IEnumerator DrownSequence()
    {
        isDrowning = true;

        // 1. Hentikan kecepatan fisik player secara instan
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // 2. Matikan kontrol pergerakan player
        this.enabled = false;

        // 3. Sembunyikan sprite MC agar terkesan tenggelam masuk ke dalam air
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // 4. Instansiasi efek cipratan air (WaterSplash) di posisi player dengan offset
        if (waterSplashPrefab != null)
        {
            Vector3 spawnPosition = transform.position + waterSplashOffset;
            GameObject splash = Instantiate(waterSplashPrefab, spawnPosition, Quaternion.identity);
            // Hancurkan efek setelah animasi selesai (1.2 detik)
            Destroy(splash, 1.2f);
            Debug.Log($"DEBUG: Efek WaterSplash dimunculkan di posisi: {spawnPosition}");
        }
        else
        {
            Debug.LogWarning("DEBUG WARNING: Prefab WaterSplash belum dipasang di PlayerController!");
        }

        // 5. Hubungi PlayerHealth untuk mematikan player (Game Over)
        if (TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamage(999); // Berikan damage besar agar mati instan
        }
        else
        {
            Debug.LogWarning("PlayerHealth tidak ditemukan pada Player!");
        }

        yield return null;
    }
}
