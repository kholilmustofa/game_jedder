using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject infoPanel; // Panel Pop-up Info Kontrol

    [Header("Audio Settings")]
    [SerializeField] private AudioSource playButtonAudio; // Seret AudioSource tombol play ke sini
    [SerializeField] private AudioSource infoButtonAudio; // Seret AudioSource tombol info ke sini

    [Header("Scene Settings")]
    [SerializeField] private string levelSelectSceneName = "LevelSelect"; // Nama scene Level Select Anda

    private void Start()
    {
        // Pastikan panel info tertutup saat game dimulai
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Fungsi saat tombol Play diklik. Memuat scene Level Select secara terpisah.
    /// </summary>
    public void PlayGame()
    {
        if (playButtonAudio != null)
        {
            playButtonAudio.Play();
            // Berikan jeda 0.2 detik agar suara klik selesai berbunyi sebelum scene berganti
            Invoke("LoadLevelSelectScene", 0.2f);
        }
        else
        {
            LoadLevelSelectScene();
        }
    }

    private void LoadLevelSelectScene()
    {
        Debug.Log("Memulai Game... Memuat scene: " + levelSelectSceneName);
        SceneManager.LoadScene(levelSelectSceneName);
    }

    /// <summary>
    /// Fungsi untuk membuka Panel Info (kontrol game).
    /// </summary>
    public void OpenInfoPanel()
    {
        if (infoButtonAudio != null)
        {
            infoButtonAudio.Play();
        }

        if (infoPanel != null)
        {
            Debug.Log("Membuka Panel Info");
            infoPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Fungsi untuk menutup Panel Info.
    /// </summary>
    public void CloseInfoPanel()
    {
        if (infoButtonAudio != null)
        {
            infoButtonAudio.Play(); // Memutar suara klik saat menutup panel info
        }

        if (infoPanel != null)
        {
            Debug.Log("Menutup Panel Info");
            infoPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Fungsi untuk keluar dari game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();
    }
}