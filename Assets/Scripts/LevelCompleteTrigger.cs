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
    /// Fungsi untuk membuka kunci Level 2 secara permanen menggunakan PlayerPrefs dan memunculkan panel sukses.
    /// </summary>
    public void UnlockNextLevel()
    {
        Debug.Log("Selamat! Level Selesai. Membuka kunci Level 2...");
        
        // 1. Menyimpan data "Level2Unlocked" = 1 (Artinya Terbuka) secara permanen di memori
        PlayerPrefs.SetInt("Level2Unlocked", 1);
        PlayerPrefs.Save(); // Simpan perubahan ke penyimpanan perangkat
        
        // 2. Munculkan panel Game Success dari GameplayMenuController
        GameplayMenuController menu = FindFirstObjectByType<GameplayMenuController>();
        if (menu != null)
        {
            menu.ShowGameSuccess();
        }
        else
        {
            Debug.LogWarning("GameplayMenuController tidak ditemukan di scene!");
        }
    }
}
