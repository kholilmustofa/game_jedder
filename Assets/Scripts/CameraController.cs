using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // Seret objek Player (MC) ke sini

    [Header("Follow Settings")]
    [SerializeField] private float smoothTime = 0.2f; // Waktu redaman (makin kecil makin cepat mengikuti)
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f); // Jarak offset kamera (Z harus minus untuk 2D)

    [Header("Map Boundary Limits (Opsional)")]
    [SerializeField] private bool useBounds = false; // Centang untuk membatasi kamera agar tidak keluar map
    [SerializeField] private Vector2 minCameraBounds; // Batas minimal koordinat X dan Y kamera
    [SerializeField] private Vector2 maxCameraBounds; // Batas maksimal koordinat X dan Y kamera

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        // Cari otomatis objek Player jika lupa dimasukkan di Inspector
        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Tentukan posisi tujuan kamera
        Vector3 targetPosition = target.position + offset;

        // Batasi posisi kamera jika fitur batas map diaktifkan
        if (useBounds)
        {
            float clampedX = Mathf.Clamp(targetPosition.x, minCameraBounds.x, maxCameraBounds.x);
            float clampedY = Mathf.Clamp(targetPosition.y, minCameraBounds.y, maxCameraBounds.y);
            targetPosition = new Vector3(clampedX, clampedY, targetPosition.z);
        }

        // Lerp/SmoothDamp posisi kamera saat ini menuju posisi tujuan agar gerakannya halus (smooth)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
