using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float maxDistance = 6.4f; // Jarak maksimal terbang dalam unit Unity (10 petak * 0.64 = 6.4)

    private Vector2 startPosition;

    [Header("Explosion Settings")]
    [SerializeField] private Animator animator;     // Komponen Animator peluru
    [SerializeField] private Collider2D bulletCollider; // Komponen Collider peluru
    [SerializeField] private float destroyDelayAfterExplosion = 0.5f; // Waktu tunggu setelah meledak baru hancur

    private bool isExploding = false;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (bulletCollider == null) bulletCollider = GetComponent<Collider2D>();
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    private void Start()
    {
        // Catat posisi awal peluru saat baru ditembakkan
        startPosition = transform.position;

        // Hancurkan peluru otomatis jika tidak menabrak apapun selama masa hidupnya
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Hanya bergerak jika tidak sedang meledak
        if (!isExploding)
        {
            // Bergerak maju (ke arah kanan lokal peluru)
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // Cek apakah jarak yang ditempuh sudah melebihi batas maksimal
            if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
            {
                Explode(); // Peluru meledak/hilang
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Cegah penabrakan ganda saat proses meledak
        if (isExploding) return;

        // Cek apakah menabrak Musuh atau Tembok
        if (collision.CompareTag("Enemy"))
        {
            // Terapkan damage ke musuh (jarak dekat atau jarak jauh)
            if (collision.TryGetComponent<EnemyMelee>(out var meleeEnemy))
            {
                meleeEnemy.TakeDamage(damage);
            }
            else if (collision.TryGetComponent<EnemyRanged>(out var rangedEnemy))
            {
                rangedEnemy.TakeDamage(damage);
            }

            Debug.Log($"Menabrak musuh! Memberikan {damage} damage.");
            Explode();
        }
        else if (collision.CompareTag("Wall"))
        {
            Debug.Log("Menabrak tembok.");
            Explode();
        }
    }

    /// <summary>
    /// Fungsi untuk memicu animasi ledakan peluru dan menghancurkan objek secara elegan.
    /// </summary>
    private void Explode()
    {
        isExploding = true;

        // 1. Matikan Collider agar tidak mendeteksi tabrakan lain saat meledak
        if (bulletCollider != null)
        {
            bulletCollider.enabled = false;
        }

        // 2. Pemicu Animasi Ledakan di Animator
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }

        // 3. Hancurkan GameObject setelah delay animasi ledakan selesai
        Destroy(gameObject, destroyDelayAfterExplosion);
    }
}
