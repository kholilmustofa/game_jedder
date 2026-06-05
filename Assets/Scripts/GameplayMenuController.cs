using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Tambahkan untuk mendukung New Input System

public class GameplayMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel; // Tarik Panel Pause/Pengaturan ke sini
    [SerializeField] private CursorManager cursorManager;   // Tarik objek CursorManager ke sini

    [Header("Scene Settings")]
    [SerializeField] private string lobbySceneName = "lobby";             // Nama scene lobby/Home
    [SerializeField] private string levelSelectSceneName = "LevelSelect"; // Nama scene Level Select/Exit

    private bool isPaused = false;

    private void Start()
    {
        // Pastikan panel pause tertutup rapat saat level dimulai
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
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

        Debug.Log("Kembali ke Lobby. Memuat scene: " + lobbySceneName);
        SceneManager.LoadScene(lobbySceneName);
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

        Debug.Log("Kembali ke Level Select. Memuat scene: " + levelSelectSceneName);
        SceneManager.LoadScene(levelSelectSceneName);
    }
}
