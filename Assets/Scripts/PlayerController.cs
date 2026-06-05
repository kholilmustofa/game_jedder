using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Combat Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private float facingMouseDuration = 0.5f; // Durasi MC menghadap kursor setelah menembak (detik)
    [SerializeField] private AudioSource shootAudioSource;      // Tarik AudioSource suara tembak ke sini
    private float nextFireTime;
    private float lastShootTime = -99f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 moveInput;
    private Vector2 mousePos;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null) mainCamera = Camera.main;
        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Get mouse position in world space using the new Input System
        if (Mouse.current != null)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            mousePos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        }
    }

    private void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = moveInput * moveSpeed;

        // Update animations
        if (animator != null)
        {
            animator.SetFloat("Speed", moveInput.sqrMagnitude);
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
            // 1. Jika baru saja menembak, arahkan tubuh MC mengikuti arah kursor mouse (membidik)
            if (Time.time - lastShootTime < facingMouseDuration)
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
            // 2. Jika sedang tidak menembak, arahkan tubuh MC sesuai arah jalan (WASD)
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
        if (Time.time >= nextFireTime)
        {
            Debug.Log("OnAttack triggered!");
            
            // Trigger the shooting animation
            if (animator != null)
            {
                animator.SetTrigger("Shoot");
            }

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
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogWarning("Bullet Prefab or FirePoint is missing in the Inspector!");
        }
    }
}
