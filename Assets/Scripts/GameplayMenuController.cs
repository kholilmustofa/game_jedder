using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Tambahkan untuk mendukung New Input System

public class GameplayMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;   // Tarik Panel Pause/Pengaturan ke sini
    [SerializeField] private GameObject gameOverPanel;    // Tarik Panel Game Over ke sini
    [SerializeField] private GameObject gameSuccessPanel; // Tarik Panel Game Success ke sini
    [SerializeField] private CursorManager cursorManager;     // Tarik objek CursorManager ke sini

    [Header("Scene Settings")]
    [SerializeField] private string lobbySceneName = "lobby";             // Nama scene lobby/Home
    [SerializeField] private string levelSelectSceneName = "LevelSelect"; // Nama scene Level Select/Exit

    [Header("Audio Settings")]
    [SerializeField] private AudioSource clickAudioSource; // Tarik AudioSource suara klik ke sini
    [SerializeField] private AudioSource pauseSoundAudio;   // Tarik AudioSource suara jeda/resume ke sini (seperti suara tombol info)

    private string pendingSceneName;
    private int pendingSceneIndex = -1;
    private bool useIndexLoad = false;
    private bool shouldRestart = false;
    private bool isPaused = false;

    private void Start()
    {
        // Pastikan panel pause, game over, dan success tertutup rapat saat level dimulai
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (gameSuccessPanel != null)
        {
            gameSuccessPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Deteksi tombol ESC menggunakan New Input System
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Membalikkan status Pause (jika sedang pause maka lanjut, jika sedang main maka jeda).
    /// Dipanggil oleh tombol Gear (Pengaturan) di layar utama.
    /// </summary>
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    /// <summary>
    /// Menjeda game dan menampilkan panel pause.
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Hentikan aliran waktu game (fisika, animasi, pergerakan berhenti)
        
        // Putar suara jeda/pause
        if (pauseSoundAudio != null)
        {
            pauseSoundAudio.Play();
        }

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        if (cursorManager != null)
        {
            cursorManager.SetMenuCursor(); // Kembalikan ke kursor panah saat menu pause aktif
        }
    }

    /// <summary>
    /// Melanjutkan game dan menutup panel pause.
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Kembalikan aliran waktu game ke normal
        
        // Putar suara resume menggunakan suara tombol biasa (clickAudioSource)
        if (clickAudioSource != null)
        {
            clickAudioSource.Play();
        }

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        if (cursorManager != null)
        {
            cursorManager.SetGameplayCursor(); // Gunakan kursor bidikan saat bermain kembali
        }
    }

    /// <summary>
    /// Kembali ke scene Lobby (Home).
    /// </summary>
    public void GoToLobby()
    {
        Time.timeScale = 1f; // PENTING: Kembalikan waktu ke normal dulu agar scene lobby tidak macet!
        
        if (cursorManager != null)
        {
            cursorManager.SetMenuCursor(); // Pastikan kursor kembali ke panah di Main Menu
        }

        pendingSceneName = lobbySceneName;
        shouldRestart = false;
        useIndexLoad = false;
        PlayClickSoundAndExecute();
    }

    /// <summary>
    /// Kembali ke scene Level Select (Exit).
    /// </summary>
    public void GoToLevelSelect()
    {
        Time.timeScale = 1f; // PENTING: Kembalikan waktu ke normal dulu agar scene LevelSelect tidak macet!

        if (cursorManager != null)
        {
            cursorManager.SetMenuCursor(); // Pastikan kursor kembali ke panah di Level Select
        }

        pendingSceneName = levelSelectSceneName;
        shouldRestart = false;
        useIndexLoad = false;
        PlayClickSoundAndExecute();
    }

    /// <summary>
    /// Menampilkan panel Game Over dan menjeda game.
    /// </summary>
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (cursorManager != null)
        {
            cursorManager.SetMenuCursor(); // Aktifkan kursor menu
        }

        Time.timeScale = 0f; // Jeda jalannya game (fisika, gerakan)
    }

    /// <summary>
    /// Menampilkan panel Game Success (Level Clear) dan menjeda game.
    /// </summary>
    public void ShowGameSuccess()
    {
        if (gameSuccessPanel != null)
        {
            gameSuccessPanel.SetActive(true);
        }

        if (cursorManager != null)
        {
            cursorManager.SetMenuCursor(); // Aktifkan kursor menu
        }

        Time.timeScale = 0f; // Jeda jalannya game (fisika, gerakan)
    }

    /// <summary>
    /// Memuat level berikutnya berdasarkan build index saat ini.
    /// </summary>
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Kembalikan waktu ke normal agar game tidak membeku
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            shouldRestart = false;
            useIndexLoad = true;
            pendingSceneIndex = nextSceneIndex;
            PlayClickSoundAndExecute();
        }
        else
        {
            Debug.LogWarning("Tidak ada level berikutnya dalam Build Settings! Kembali ke lobby.");
            GoToLevelSelect();
        }
    }

    /// <summary>
    /// Memuat ulang level/scene yang sedang aktif dari awal.
    /// </summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f; // PENTING: Kembalikan waktu ke normal agar game tidak membeku saat memuat ulang!
        shouldRestart = true;
        useIndexLoad = false;
        pendingSceneName = "";
        PlayClickSoundAndExecute();
    }

    private void PlayClickSoundAndExecute()
    {
        if (clickAudioSource != null)
        {
            clickAudioSource.Play();
            // Jeda 0.2 detik agar suara selesai berbunyi sebelum scene berganti
            Invoke("ExecutePendingSceneLoad", 0.2f);
        }
        else
        {
            ExecutePendingSceneLoad();
        }
    }

    private void ExecutePendingSceneLoad()
    {
        if (shouldRestart)
        {
            shouldRestart = false;
            Debug.Log("Mengulang level... Memuat scene: " + SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (useIndexLoad && pendingSceneIndex != -1)
        {
            useIndexLoad = false;
            Debug.Log("Memuat scene index berikutnya: " + pendingSceneIndex);
            SceneManager.LoadScene(pendingSceneIndex);
            pendingSceneIndex = -1;
        }
        else if (!string.IsNullOrEmpty(pendingSceneName))
        {
            Debug.Log("Memuat scene: " + pendingSceneName);
            SceneManager.LoadScene(pendingSceneName);
        }
    }
}
