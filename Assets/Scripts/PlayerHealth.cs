using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Invincibility Frame (iFrame)")]
    [SerializeField] private float invincibilityDuration = 1f; // Kebal selama 1 detik setelah kena hit
    private bool isInvincible = false;

    [Header("UI Reference")]
    [SerializeField] private Slider healthSlider; // Tarik UI Slider darah ke sini

    [Header("Visual Feedback (Optional)")]
    [SerializeField] private SpriteRenderer spriteRenderer; // Untuk efek berkedip saat kebal
    [SerializeField] private Color damageColor = Color.red;
    private Color originalColor = Color.white;

    [Header("Game Over Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameplayMenuController menuController;
    [SerializeField] private string deathAnimationName = "Player_Death"; // Nama state animasi mati di Animator Controller

    private void Start()
    {
        currentHealth = maxHealth;

        // Inisialisasi tampilan awal Slider Darah
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // Cari Animator jika belum diset
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Cari GameplayMenuController di Scene jika belum diset
        if (menuController == null)
        {
            menuController = FindFirstObjectByType<GameplayMenuController>();
        }
    }

    /// <summary>
    /// Fungsi untuk mengurangi HP Player. Dipanggil oleh musuh atau jebakan.
    /// </summary>
    public void TakeDamage(int damage)
    {
        // Abaikan damage jika sedang dalam masa kebal (iFrame)
        if (isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Batasi HP minimal 0

        Debug.Log($"Player terkena hit! Sisa HP: {currentHealth}");

        // Update visual Slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Cek apakah Player mati
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Aktifkan masa kebal sementara
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    /// <summary>
    /// Coroutine untuk mengontrol masa kebal dan efek berkedip merah.
    /// </summary>
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsed = 0f;
        bool toggle = false;

        while (elapsed < invincibilityDuration)
        {
            if (spriteRenderer != null)
            {
                // Efek berkedip merah dan normal secara bergantian
                spriteRenderer.color = toggle ? damageColor : originalColor;
                toggle = !toggle;
            }
            
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        // Kembalikan warna normal dan matikan masa kebal
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        isInvincible = false;
    }

    /// <summary>
    /// Fungsi untuk menambah HP (misal saat mengambil peti medis).
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        Debug.Log($"Player dipulihkan! HP sekarang: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("Player mati! Game Over.");

        // 1. Matikan kontrol pergerakan Player
        if (TryGetComponent<PlayerController>(out var playerController))
        {
            playerController.enabled = false;
        }

        // 2. Hentikan kecepatan fisik agar langsung berhenti diam di tempat
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero;
        }

        // 3. Putar animasi mati jika ada Animator
        if (animator != null)
        {
            animator.Play(deathAnimationName);
        }

        // 4. Putar musik Game Over segera saat animasi mati dimulai (agar tidak telat)
        if (menuController != null)
        {
            menuController.PlayGameOverMusic();
        }

        // 5. Jalankan coroutine untuk memunculkan layar Game Over setelah animasi selesai
        StartCoroutine(GameOverSequenceCoroutine());
    }

    private IEnumerator GameOverSequenceCoroutine()
    {
        // Tunggu 1.5 detik agar animasi kematian selesai diputar secara visual
        yield return new WaitForSeconds(1.5f);

        // Munculkan layar Game Over
        if (menuController != null)
        {
            menuController.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("GameplayMenuController tidak ditemukan! Tidak bisa memunculkan layar Game Over.");
        }
    }
}
