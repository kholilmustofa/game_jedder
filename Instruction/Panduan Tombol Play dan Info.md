# Panduan Membuat Tombol Play dan Info (Unity UI)

Dokumen ini berisi panduan lengkap langkah demi langkah untuk membuat sistem **Main Menu** dengan tombol **Play (Mulai)** dan **Info** menggunakan aset **Tiny Swords** yang sudah ada di proyek Unity Anda.

---

## 1. Kode Script C# (`MainMenuController.cs`)

Pertama, buat sebuah script C# baru di Unity Anda dengan nama **`MainMenuController.cs`**. Letakkan script ini di dalam folder `Assets/Scripts/`. 

Berikut adalah isi kode lengkapnya:

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject infoPanel; // GameObject dari Panel Info/Popup

    [Header("Scene Settings")]
    [SerializeField] private string levelSelectSceneName = "LevelSelect"; // Nama scene Level Select Anda

    private void Start()
    {
        // Pastikan panel info tertutup saat game pertama kali dijalankan
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Fungsi untuk berpindah ke scene Level Select setelah menekan tombol Play.
    /// </summary>
    public void PlayGame()
    {
        Debug.Log("Membuka Level Select... Memuat scene: " + levelSelectSceneName);
        SceneManager.LoadScene(levelSelectSceneName);
    }

    /// <summary>
    /// Fungsi untuk membuka Panel Info (kontrol game).
    /// </summary>
    public void OpenInfoPanel()
    {
        if (infoPanel != null)
        {
            Debug.Log("Membuka Panel Info");
            infoPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("infoPanel belum dimasukkan di Inspector!");
        }
    }

    /// <summary>
    /// Fungsi untuk menutup Panel Info.
    /// </summary>
    public void CloseInfoPanel()
    {
        if (infoPanel != null)
        {
            Debug.Log("Menutup Panel Info");
            infoPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Fungsi opsional untuk keluar dari game (jika diperlukan tombol Exit).
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();
    }
}
```

---

## 2. Pengaturan Gambar Aset (Sprite Settings)

Sebelum merancang UI di Canvas, pastikan gambar aset UI dari **Tiny Swords** sudah diatur agar bisa digunakan sebagai elemen UI Unity:

1. Di Project Window Unity, cari folder:
   `Assets/Tiny Swords (Free Pack)/UI Elements/UI Elements/`
2. Pilih aset gambar berikut (gunakan `Ctrl + Klik` untuk memilih bersamaan):
   * **`Buttons/BigBlueButton_Regular`** & **`Buttons/BigBlueButton_Pressed`** (Untuk tombol Play)
   - **`Buttons/SmallBlueRoundButton_Regular`** & **`Buttons/SmallBlueRoundButton_Pressed`** (Untuk tombol Info)
   - **`Papers/RegularPaper`** (Untuk panel pop-up Info)
3. Lihat jendela **Inspector** di sebelah kanan:
   - Ubah **Texture Type** menjadi **Sprite (2D and UI)**.
   - Klik tombol **Apply** di bagian bawah Inspector.

---

## 3. Merancang Struktur UI di Hierarchy

Buat susunan objek UI di jendela Hierarchy dengan urutan berikut:

1. **Membuat Canvas:**
   - Klik kanan di Hierarchy -> **UI** -> **Canvas**. (Objek `EventSystem` otomatis terbuat, jangan dihapus).

2. **Membuat Judul Game ("JEDDER"):**
   - Klik kanan pada *Canvas* -> **UI** -> **Text (TMP)** atau **Text (Legacy)**.
   - Beri nama objek: `GameTitle`.
   - Ubah teks menjadi **"JEDDER"** dengan ukuran font yang besar dan dekorasi sesuai keinginan Anda.

3. **Membuat Tombol Play:**
   - Klik kanan pada *Canvas* -> **UI** -> **Button (Legacy)** atau **Button - TextMeshPro**.
   - Beri nama objek: `PlayButton`.
   - Pada komponen **Image** di `PlayButton`:
     - Seret sprite **`BigBlueButton_Regular`** ke dalam kotak **Source Image**.
     - Ubah pilihan **Transition** dari *Color Tint* menjadi **Sprite Swap**.
     - Seret sprite **`BigBlueButton_Pressed`** ke dalam kotak **Pressed Sprite**.
   - Di objek anak (Child) bernama **`Text`**, ubah tulisannya menjadi **"MULAI / PLAY"** dengan warna putih agar terlihat kontras.

4. **Membuat Tombol Info:**
   - Klik kanan pada *Canvas* -> **UI** -> **Button**.
   - Beri nama objek: `InfoButton`.
   - Pada komponen **Image** di `InfoButton`:
     - Seret sprite **`SmallBlueRoundButton_Regular`** ke dalam **Source Image**.
     - Ubah **Transition** menjadi **Sprite Swap**.
     - Seret sprite **`SmallBlueRoundButton_Pressed`** ke dalam **Pressed Sprite**.
   - Di objek anak **`Text`**, ubah tulisannya menjadi **"i"** atau **"INFO"**.

5. **Membuat Panel Pop-up Info:**
   - Klik kanan pada *Canvas* -> **UI** -> **Image**.
   - Beri nama objek: `InfoPanel`.
   - Pada komponen **Image** di `InfoPanel`:
     - Seret sprite **`RegularPaper`** ke dalam **Source Image**.
     - Atur posisinya di tengah layar dan perbesar ukurannya agar cukup menampung teks informasi kontrol game.
   - **Menambahkan Teks Kontrol Game:**
     - Klik kanan pada *InfoPanel* -> **UI** -> **Text (TMP)**.
     - Tulis informasi kontrol game sesuai catatan baru Anda:
       ```text
       KONTROL GAME JEDDER
       
       WASD = Bergerak
       Klik Kiri = Menembak
       Shift = Lari / Tambah Speed
       R = Reload Peluru (Opsional)
       ```
   - **Menambahkan Tombol Tutup Panel (Close):**
     - Klik kanan pada *InfoPanel* -> **UI** -> **Button**.
     - Beri nama objek: `CloseButton`.
     - Atur posisinya di pojok atas panel (Anda bisa memakai aset tombol merah seperti `SmallRedRoundButton_Regular` agar menarik).
     - Ubah teks tombol menjadi **"X"**.

---

## 4. Menghubungkan Script dengan Elemen UI (Inspector)

Setelah struktur visual selesai, hubungkan logika tombol menggunakan script:

1. **Buat GameManager:**
   - Klik kanan di Hierarchy -> **Create Empty**. Beri nama **`MainMenuManager`**.
   - Tarik/drag script **`MainMenuController`** ke objek `MainMenuManager` tersebut.
2. **Hubungkan Panel Info:**
   - Pilih `MainMenuManager` di Hierarchy.
   - Seret objek **`InfoPanel`** dari Hierarchy ke kolom kosong **Info Panel** di script komponen pada Inspector.
3. **Hubungkan Fungsi Klik pada Tombol:**
   - **Tombol Play (`PlayButton`):**
     - Pilih `PlayButton` di Hierarchy.
     - Di Inspector, cari komponen **On Click ()** di bagian bawah, lalu klik ikon **`+`**.
     - Seret objek **`MainMenuManager`** dari Hierarchy ke kolom objek kosong (di bawah Runtime Only).
     - Klik dropdown fungsi (No Function) -> pilih **`MainMenuController`** -> pilih **`PlayGame()`**.
   - **Tombol Info (`InfoButton`):**
     - Pilih `InfoButton` di Hierarchy.
     - Di komponen **On Click ()**, klik **`+`**, lalu seret objek **`MainMenuManager`**.
     - Pilih dropdown -> **`MainMenuController`** -> **`OpenInfoPanel()`**.
   - **Tombol Close Panel (`CloseButton`):**
     - Pilih `CloseButton` (yang ada di dalam `InfoPanel`).
     - Di komponen **On Click ()**, klik **`+`**, lalu seret objek **`MainMenuManager`**.
     - Pilih dropdown -> **`MainMenuController`** -> **`CloseInfoPanel()`**.

---

## 5. Mendaftarkan Scene di Build Settings

Terakhir, daftarkan scene Anda agar Unity mengenali nama scene saat berpindah level select:

1. Di menu atas Unity, pilih **File** -> **Build Settings...**
2. Seret scene Main Menu Anda dan scene Level Select Anda ke dalam kotak **Scenes In Build**.
3. Pastikan ejaan nama scene Level Select Anda sama persis dengan yang diisi di kolom **Level Select Scene Name** pada script `MainMenuManager` di Inspector (contoh: `"LevelSelect"`).

Sekarang Anda bisa menekan tombol **Play** di Unity Editor untuk menguji tombol **Play** (berpindah scene) dan tombol **Info** (membuka dan menutup pop-up instruksi kontrol game)!
