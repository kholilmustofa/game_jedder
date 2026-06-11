using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Header("Lifetime Settings")]
    [SerializeField] private float destroyDelay = 1f; // Waktu (detik) sebelum objek dihancurkan otomatis

    private void Start()
    {
        // Hancurkan GameObject ini setelah waktu delay selesai
        Destroy(gameObject, destroyDelay);
    }
}
