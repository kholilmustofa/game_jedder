using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    [Header("Level 2 UI References")]
    [SerializeField] private Button level2Button;       // Tarik Button Level 2 ke sini
    [SerializeField] private GameObject level2LockIcon;  // Tarik Objek Gambar Gembok Level 2 ke sini

    [Header("Scene Names")]
    [SerializeField] private string level1SceneName = "Level1"; // Nama scene untuk Level 1
    [SerializeField] private string level2SceneName = "Level2"; // Nama scene untuk Level 2 (Boss)

    private void Start()
    {
        UpdateLevelSelectionUI();
    }

    /// <summary>
    /// Mengecek progress level di PlayerPrefs dan mengupdate tampilan UI (aktif/nonaktif tombol & gembok).
    /// </summary>
    private void UpdateLevelSelectionUI()
    {
        // Mengecek apakah Level 2 sudah terbuka. 0 = Terkunci, 1 = Terbuka.
        // Default-nya adalah 0 (Terkunci) saat pertama kali bermain.
        bool isLevel2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;

        if (isLevel2Unlocked)
        {
            // Jika Level 2 TERBUKA:
            if (level2Button != null) level2Button.interactable = true; // Tombol bisa diklik
            if (level2LockIcon != null) level2LockIcon.SetActive(false); // Sembunyikan ikon gembok
        }
        else
        {
            // Jika Level 2 TERKUNCI:
            if (level2Button != null) level2Button.interactable = false; // Tombol tidak bisa diklik (abu-abu/mati)
            if (level2LockIcon != null) level2LockIcon.SetActive(true);  // Tampilkan ikon gembok
        }
    }

    /// <summary>
    /// Fungsi yang dipanggil saat Tombol Level 1 diklik.
    /// </summary>
    public void LoadLevel1()
    {
        Debug.Log("Memulai Level 1... Memuat scene: " + level1SceneName);
        SceneManager.LoadScene(level1SceneName);
    }

    /// <summary>
    /// Fungsi yang dipanggil saat Tombol Level 2 diklik.
    /// </summary>
    public void LoadLevel2()
    {
        // Pengaman tambahan jika tombol diklik lewat cara lain saat terkunci
        bool isLevel2Unlocked = PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;
        if (isLevel2Unlocked)
        {
            Debug.Log("Memulai Level 2 (Boss)... Memuat scene: " + level2SceneName);
            SceneManager.LoadScene(level2SceneName);
        }
        else
        {
            Debug.LogWarning("Level 2 masih terkunci!");
        }
    }

    /// <summary>
    /// Fungsi opsional untuk kembali ke scene Main Menu.
    /// </summary>
    public void BackToMainMenu(string mainMenuSceneName)
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Debug & Testing Tools
    [ContextMenu("Unlock Level 2")]
    public void DebugUnlockLevel2()
    {
        PlayerPrefs.SetInt("Level2Unlocked", 1);
        PlayerPrefs.Save();
        UpdateLevelSelectionUI();
        Debug.Log("DEBUG: Level 2 Berhasil Dibuka!");
    }

    [ContextMenu("Reset Progress")]
    public void DebugResetProgress()
    {
        PlayerPrefs.DeleteKey("Level2Unlocked");
        PlayerPrefs.Save();
        UpdateLevelSelectionUI();
        Debug.Log("DEBUG: Progress level berhasil direset!");
    }
}
