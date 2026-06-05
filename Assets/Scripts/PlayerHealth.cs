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
        // Di sini Anda bisa memanggil GameManager untuk memunculkan modal Game Over
    }
}
