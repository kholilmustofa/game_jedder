using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyArrow : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float arcHeight = 1.5f; // Tinggi maksimal lengkungan panah

    [Header("Components")]
    [SerializeField] private Collider2D arrowCollider;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Vector2 previousPosition;
    
    private float travelDuration;
    private float timeElapsed;
    private bool isHit = false;

    private void Awake()
    {
        if (arrowCollider == null) arrowCollider = GetComponent<Collider2D>();

        // Pastikan ada Rigidbody2D dan konfigurasinya benar agar deteksi tabrakan stabil
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void SetDamage(int damageValue)
    {
        damage = damageValue;
    }

    private void Start()
    {
        startPosition = transform.position;
        previousPosition = startPosition;

        // Cari posisi Player saat anak panah ini ditembakkan
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 playerPos = player.transform.position;

            // Coba dapatkan Rigidbody2D player untuk memprediksi arah jalannya
            if (player.TryGetComponent<Rigidbody2D>(out var playerRb))
            {
                Vector2 playerVelocity = playerRb.linearVelocity;

                // Hitung jarak awal ke player
                float initialDistance = Vector2.Distance(startPosition, playerPos);

                // Hitung perkiraan waktu tempuh panah ke player
                float estimatedTravelTime = initialDistance / speed;

                // Prediksi posisi target: Posisi sekarang + (Kecepatan lari Player * perkiraan waktu terbang)
                targetPosition = playerPos + (playerVelocity * estimatedTravelTime);
            }
            else
            {
                targetPosition = playerPos;
            }
        }
        else
        {
            // Jika player tidak ditemukan, terbang lurus ke depan
            targetPosition = startPosition + (Vector2)transform.right * 8f;
        }

        // Hitung jarak dan durasi waktu terbang berdasarkan kecepatan ke titik prediksi
        float totalDistance = Vector2.Distance(startPosition, targetPosition);
        
        // Mencegah pembagian dengan nol jika jarak terlalu dekat
        if (totalDistance < 0.1f) totalDistance = 0.1f;
        
        travelDuration = totalDistance / speed;
        timeElapsed = 0f;
    }

    private void Update()
    {
        if (isHit) return;

        timeElapsed += Time.deltaTime;
        float progress = Mathf.Clamp01(timeElapsed / travelDuration);

        // 1. Hitung posisi koordinat tanah (Linear Lerp)
        Vector2 currentGroundPos = Vector2.Lerp(startPosition, targetPosition, progress);

        // 2. Hitung tinggi parabola menggunakan fungsi Sinus (0 hingga Pi)
        float height = Mathf.Sin(progress * Mathf.PI) * arcHeight;

        // 3. Gabungkan posisi tanah dengan tinggi visual parabola
        Vector2 newPosition = currentGroundPos + new Vector2(0, height);

        // 4. Hitung sudut rotasi agar ujung anak panah selalu menghadap ke arah terbangnya (tangent)
        Vector2 velocity = newPosition - previousPosition;
        if (velocity.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        previousPosition = newPosition;
        transform.position = newPosition;

        // 5. Jika waktu terbang habis dan belum menabrak player, anggap menabrak tanah
        if (progress >= 1f)
        {
            Hit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit) return;

        // Cek jika menabrak Player
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"EnemyArrow: Menabrak objek dengan tag Player: {collision.gameObject.name}");
            
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                playerHealth = collision.GetComponentInParent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log("EnemyArrow: PlayerHealth ditemukan di Parent GameObject!");
                }
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Anak panah musuh mengenai Player! Memberikan {damage} damage.");
            }
            else
            {
                Debug.LogWarning("EnemyArrow: Menabrak objek ber-tag Player, tetapi PlayerHealth tidak ditemukan!");
            }

            Hit();
        }
        // Cek jika menabrak Tembok/Rintangan
        else if (collision.CompareTag("Wall"))
        {
            Debug.Log("Anak panah musuh menabrak tembok.");
            Hit();
        }
    }

    private void Hit()
    {
        isHit = true;

        if (arrowCollider != null)
        {
            arrowCollider.enabled = false;
        }

        // Sembunyikan visual anak panah agar tidak terlihat melayang diam saat hancur tertunda
        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.enabled = false;
        }

        // Jika terdapat Trail Renderer, matikan pancaran ekor dan tunggu hingga memudar sebelum dihancurkan
        if (TryGetComponent<TrailRenderer>(out var trail))
        {
            trail.emitting = false;
            Destroy(gameObject, trail.time); // Hancurkan setelah durasi waktu (lifetime) trail habis
        }
        else
        {
            // Jika tidak menggunakan trail, langsung hancurkan instan
            Destroy(gameObject);
        }
    }
}
