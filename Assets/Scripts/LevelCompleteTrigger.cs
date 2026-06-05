using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string levelSelectSceneName = "LevelSelect"; // Nama scene menu pilih level
    
    // Dipanggil otomatis saat Player menyentuh area collider pintu keluar (Trigger 2D)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Pastikan yang menyentuh adalah Player
        if (collision.CompareTag("Player"))
        {
            UnlockNextLevel();
        }
    }

    /// <summary>
    /// Fungsi untuk membuka kunci Level 2 secara permanen menggunakan PlayerPrefs.
    /// Fungsi ini juga bisa dipanggil langsung oleh script kemenangan/Manager Anda lainnya!
    /// </summary>
    public void UnlockNextLevel()
    {
        Debug.Log("Selamat! Level 1 Selesai. Membuka kunci Level 2...");
        
        // Menyimpan data "Level2Unlocked" = 1 (Artinya Terbuka) secara permanen di memori
        PlayerPrefs.SetInt("Level2Unlocked", 1);
        PlayerPrefs.Save(); // Simpan perubahan ke penyimpanan perangkat
        
        // Optional: Anda bisa langsung memuat scene Level Select atau menampilkan modal kemenangan di sini
        // SceneManager.LoadScene(levelSelectSceneName);
    }
}
