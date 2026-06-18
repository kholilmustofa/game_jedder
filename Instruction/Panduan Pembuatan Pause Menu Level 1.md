# Panduan Pembuatan Pause Menu & Pengaturan Level 1

Panduan ini menjelaskan cara merakit UI **Pause Menu** dan menghubungkannya dengan script **[GameplayMenuController.cs](file:///d:/Unity/jedder/Assets/Scripts/GameplayMenuController.cs)** di scene `Level1`.

---

## 🔌 LANGKAH 1: Persiapan Script Controller
1. Buka scene **`Level1`** Anda di Unity Editor.
2. Di Hierarchy, klik kanan -> **Create Empty**. Beri nama objek baru ini: **`GameplayMenuController`**.
3. Seret/drag script **[GameplayMenuController.cs](file:///d:/Unity/jedder/Assets/Scripts/GameplayMenuController.cs)** dari folder `Assets/Scripts/` dan pasangkan ke objek tersebut di Inspector.

---

## 🎨 LANGKAH 2: Membuat Tombol Jeda (Pause Button)
Kita akan membuat tombol ikon roda gigi (gear) kecil di pojok kanan atas layar agar player bisa menjeda game.

1. Di bawah objek **`Canvas`** di Hierarchy, klik kanan -> **UI** -> **Button** (atau Button - TextMeshPro jika menggunakan TMP).
2. Beri nama tombol ini: **`PauseButton`**.
3. Di dalam `PauseButton`, hapus objek anak **`Text`** (karena kita akan menggunakan ikon gambar roda gigi, bukan teks).
4. Klik **`PauseButton`**. Di Inspector pada komponen **Image**:
   * Masukkan sprite ikon roda gigi ke dalam kolom **Source Image** (Anda bisa memakai aset tombol bundar biru/kuning dari pack *Tiny Swords*, misalnya ikon pengaturan).
5. Atur posisinya di pojok kanan atas layar menggunakan komponen **Rect Transform**:
   * Klik kotak **Anchor Presets**, tahan tombol **`Alt`**, lalu pilih **Top-Right** (pojok kanan atas).
   * Atur jaraknya agar rapi:
     * **Pos X**: `-60`
     * **Pos Y**: `-60`
     * **Width**: `50`
     * **Height**: `50`

---

## 🖼️ LANGKAH 3: Membuat Panel Jeda (Pause Panel)
Ketika tombol jeda diklik, panel ini akan muncul di tengah layar dan memburamkan latar belakang game.

1. Di bawah objek **`Canvas`** di Hierarchy, klik kanan -> **UI** -> **Panel**. Beri nama objek ini: **`PausePanel`**.
2. Di Inspector pada komponen **Image**:
   * Ubah warna **Color** menjadi hitam transparan (misal alpha `120`) agar game di latar belakang terlihat redup saat dijeda.
3. Di bawah **`PausePanel`**, buat objek gambar baru untuk bingkai menu jeda:
   * Klik kanan pada **`PausePanel`** -> **UI** -> **Image**. Beri nama: **`MenuFrame`**.
   * Gunakan aset **`SpecialPaper_Stitched`** (dari folder `Papers/`) sebagai **Source Image** pada `MenuFrame`.
   * Ubah **Image Type** menjadi **`Sliced`**.
   * Atur ukurannya di Rect Transform agar berada di tengah layar (misalnya Width: `450`, Height: `400`).
4. Di dalam **`MenuFrame`**, buat teks judul, teks petunjuk kontrol, dan tombol pilihan:
   * **Teks Judul**: Klik kanan `MenuFrame` -> **UI** -> **Text** (Legacy/TextMeshPro). Tulis `"Paused"`.
   * **Teks Petunjuk Kontrol**: Buat objek teks baru di bawah `MenuFrame` untuk menampilkan info pergerakan:
     * Teks 1: `Info` (dengan ikon kecil di sebelahnya jika ada).
     * Teks 2: `WASD = BERGERAK`
     * Teks 3: `MOUSE KIRI = MENEMBAK`
   * **Tombol Menu (Exit, Home, Resume)**: Buat tiga tombol di bagian bawah `MenuFrame`. Pasangkan sprite ikon yang sesuai (ikon panah melengkung untuk Exit, ikon kastil/rumah untuk Home, dan ikon play untuk Resume).

---

## 🔗 LANGKAH 4: Menghubungkan Event & Pemicu Tombol (On Click)

Sekarang mari kita hubungkan logika C# dengan tombol-tombol UI yang telah dibuat:

### A. Referensi di `GameplayMenuController`
1. Klik objek **`GameplayMenuController`** di Hierarchy.
2. Di Inspector, seret objek berikut ke kolom yang sesuai:
   * **Pause Menu Panel** -> Seret objek **`PausePanel`**

### B. Event Klik Tombol (On Click ())
Pilih setiap objek tombol di Hierarchy, cari bagian **On Click ()** di Inspector sebelah kanan, lalu atur fungsinya:

1. **`PauseButton`** (Roda Gigi di Layar Utama):
   * Klik tombol `+` di bawah On Click ().
   * Seret objek **`GameplayMenuController`** ke kolom objek kosong.
   * Pilih fungsi: **`GameplayMenuController`** -> **`TogglePause`**.

2. Tombol **`Resume`** (di dalam Pause Menu):
   * Tambahkan event klik baru.
   * Hubungkan ke **`GameplayMenuController`**.
   * Pilih fungsi: **`GameplayMenuController`** -> **`ResumeGame`**.

3. Tombol **`Home`** (di dalam Pause Menu):
   * Tambahkan event klik baru.
   * Hubungkan ke **`GameplayMenuController`**.
   * Pilih fungsi: **`GameplayMenuController`** -> **`GoToLobby`**.

4. Tombol **`Exit`** (di dalam Pause Menu):
   * Tambahkan event klik baru.
   * Hubungkan ke **`GameplayMenuController`**.
   * Pilih fungsi: **`GameplayMenuController`** -> **`GoToLevelSelect`**.

---

### 🎉 SELESAI & UJI COBA!
Jalankan game di scene `Level1`.
* Klik tombol roda gigi di pojok kanan atas, atau tekan tombol **`ESC`** pada keyboard.
* Karakter dan musuh akan otomatis berhenti bergerak karena waktu dijeda (`timeScale = 0`).
* Anda bisa langsung membaca informasi kontrol WASD/Menembak secara aman di tengah layar.
* Klik **Home** untuk kembali ke Lobby, atau **Exit** untuk kembali memilih level lain. Semua scene akan memuat dengan waktu berjalan normal kembali!
