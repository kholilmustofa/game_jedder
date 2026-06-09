using UnityEngine;
using System.Collections;

public class SecretChest : MonoBehaviour
{
    [Header("Upgrade Settings")]
    [SerializeField] private int damageUpgradeAmount = 5; // Jumlah damage yang ditambah ke peluru Player
    [SerializeField] private float openAnimationDuration = 0.8f; // Jeda waktu (detik) dari animasi "open" sampai berganti ke "opened"

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource openSoundAudio; // Suara peti saat terbuka (opsional)

    [Header("VFX Settings")]
    [SerializeField] private GameObject upgradeEffectPrefab; // Prefab efek visual upgrade (Spell Attack Up)
    [SerializeField] private bool spawnEffectOnPlayer = true;   // Jika true, efek muncul di Player. Jika false, muncul di peti.
    [SerializeField] private float effectDestroyDelay = 1.5f;   // Durasi sebelum objek efek dihancurkan otomatis

    private bool isPlayerNear = false;
    private bool isOpened = false;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (openSoundAudio == null)
        {
            openSoundAudio = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek apakah Player masuk ke dalam area jangkauan peti
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Player berada di dekat peti. Klik peti untuk membuka!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Cek apakah Player meninggalkan area peti
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player meninggalkan area peti.");
        }
    }

    private void OnMouseDown()
    {
        // OnMouseDown dipanggil otomatis oleh Unity saat kursor mouse mengklik Collider2D objek ini
        if (isPlayerNear && !isOpened)
        {
            OpenChest();
        }
        else if (!isPlayerNear && !isOpened)
        {
            Debug.Log("Peti terlalu jauh! Dekati peti terlebih dahulu.");
        }
    }

    private void OpenChest()
    {
        StartCoroutine(OpenChestCoroutine());
    }

    private IEnumerator OpenChestCoroutine()
    {
        isOpened = true;

        // 1. Putar animasi membuka peti ("open")
        if (animator != null)
        {
            animator.Play("open");
        }

        // 2. Putar suara peti terbuka
        if (openSoundAudio != null)
        {
            openSoundAudio.Play();
        }

        // 3. Cari Player dan tingkatkan damage pelurunya
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            player.UpgradeBulletDamage(damageUpgradeAmount);
            Debug.Log($"Peti rahasia dibuka! Damage peluru bertambah +{damageUpgradeAmount}.");
        }
        else
        {
            Debug.LogWarning("PlayerController tidak ditemukan di Scene!");
        }

        // 4. Munculkan efek visual upgrade
        if (upgradeEffectPrefab != null)
        {
            Vector3 spawnPosition = (spawnEffectOnPlayer && player != null) ? player.transform.position : transform.position;
            GameObject effectObj = Instantiate(upgradeEffectPrefab, spawnPosition, Quaternion.identity);
            Destroy(effectObj, effectDestroyDelay); // Hancurkan otomatis setelah durasi tertentu
            Debug.Log("Efek visual upgrade berhasil dimunculkan!");
        }

        // Tunggu hingga animasi membuka ("open") selesai
        yield return new WaitForSeconds(openAnimationDuration);

        // 5. Ubah animasi ke keadaan peti sudah terbuka ("opened")
        if (animator != null)
        {
            animator.Play("opened");
        }
    }
}
