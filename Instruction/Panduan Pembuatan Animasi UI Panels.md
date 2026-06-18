# Panduan Pembuatan Animasi UI Panels (Pause, Game Over, & Success Panel)

Panduan ini menjelaskan cara menambahkan animasi masuk (**Open**) dan animasi keluar (**Close**) pada panel UI utama di game Anda menggunakan sistem Animator Unity yang terintegrasi dengan **`GameplayMenuController`**.

---

## 💡 Konsep Penting Animasi UI Saat Game Jeda (Pause)

Ketiga panel ini muncul ketika game dalam kondisi dijeda (`Time.timeScale = 0f`). Agar animasi panel dapat bergerak dalam kondisi jeda, komponen **Animator** pada setiap panel wajib diubah **`Update Mode`**-nya menjadi **`Unscaled Time`**.

* **Animasi Masuk (Open)**: Terjadi secara otomatis ketika panel diaktifkan via `SetActive(true)`. Animator akan langsung memutar state default pertamanya.
* **Animasi Keluar (Close)**: Terjadi ketika menekan tombol resume pada Pause Menu. Script akan memicu trigger `Close`, menunggu durasi animasi selesai (coroutine), baru menonaktifkan panel dan melanjutkan game.

---

## Langkah 1: Membuat File Animasi (Clip)

Buat animasi untuk masing-masing panel (misalnya animasi memperbesar dari skala `0` ke `1` untuk animasi **Open**, dan mengecil dari `1` ke `0` untuk animasi **Close**):

1. **PausePanel**:
   * Pilih objek `PausePanel` (atau `PauseMenuPanel`) di Hierarchy.
   * Buka jendela **Animation** -> klik **Create** -> buat clip **`PausePanel_Open.anim`**.
   * Buat keyframe Scale: dari `(0,0,0)` di detik `0:00` ke `(1,1,1)` di detik `0:30`.
   * Di dropdown clip, klik *Create New Clip...* -> buat clip **`PausePanel_Close.anim`**.
   * Buat keyframe Scale: dari `(1,1,1)` di detik `0:00` ke `(0,0,0)` di detik `0:30`.
2. **GameOverPanel**:
   * Pilih objek `GameOverPanel` di Hierarchy -> buka jendela Animation -> klik **Create** -> buat clip **`GameOverPanel_Open.anim`** (skala `0` ke `1`).
3. **SuccessMenuPanel**:
   * Pilih objek `SuccessMenuPanel` di Hierarchy -> buka jendela Animation -> klik **Create** -> buat clip **`SuccessPanel_Open.anim`** (skala `0` ke `1`).

*(Jangan lupa untuk mematikan centang **`Loop Time`** pada semua file clip `.anim` baru ini di jendela Project).*

---

## Langkah 2: Mengatur Animator Controller masing-masing Panel

### 1. Konfigurasi Animator `PausePanel` (Pause Menu)
* Buka jendela **Animator** untuk `PausePanel`.
* Pastikan state **`PausePanel_Open`** berwarna **Oranye** (Default State).
* Buat parameter **Trigger** baru bernama **`Close`** di tab Parameters.
* Klik kanan `PausePanel_Open` -> **Make Transition** -> hubungkan ke `PausePanel_Close`.
* Klik garis panah transisi tersebut:
  * **Hapus centang** pada **`Has Exit Time`**.
  * Ubah **`Transition Duration`** menjadi **`0`**.
  * Di bagian **Conditions**, tambahkan parameter trigger **`Close`**.

### 2. Konfigurasi Animator `GameOverPanel` dan `SuccessMenuPanel`
* Buka jendela **Animator** masing-masing panel.
* Pastikan clip **`GameOverPanel_Open`** atau **`SuccessPanel_Open`** diatur sebagai default state (warna Oranye). Karena panel ini tidak memiliki tombol resume untuk kembali bermain, panel ini **tidak membutuhkan transisi Close**.

---

## Langkah 3: Mengatur Update Mode ke Unscaled Time (WAJIB)

Lakukan langkah ini pada **ketiga** panel tersebut (`PausePanel`, `GameOverPanel`, dan `SuccessMenuPanel`):
1. Klik objek panel di Hierarchy.
2. Di jendela Inspector, temukan komponen **Animator**.
3. Ubah kolom **`Update Mode`** dari *Normal* menjadi **`Unscaled Time`**.

---

## Langkah 4: Menghubungkan Animator ke GameplayMenuController

1. Klik objek tempat script **`GameplayMenuController`** berada di Hierarchy.
2. Di jendela Inspector, sekarang ada slot baru:
   * **`Pause Menu Animator`**: Seret komponen **Animator** milik `PausePanel` dari Hierarchy ke slot ini.
   * **`Pause Close Anim Duration`**: Masukkan durasi penutupan (misal **`0.3`** detik, sesuaikan dengan panjang clip `PausePanel_Close`).
3. Simpan perubahan Scene Anda.

---

## Langkah 5: Melakukan Uji Coba (Testing)

1. **Pause Menu**: Tekan **ESC** atau tombol gear saat bermain. Panel pause harus membesar dengan mulus. Klik **Resume** (atau tekan ESC lagi), panel akan mengecil selama 0.3 detik lalu game dilanjutkan.
2. **Game Over / Success**: Biarkan MC mati atau capai pintu portal keluar. Panel yang muncul akan memutar animasi masuk (membesar) secara otomatis.
