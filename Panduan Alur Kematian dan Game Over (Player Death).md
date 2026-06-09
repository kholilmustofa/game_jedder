# Panduan Alur Kematian & Layar Game Over (Player Death)

Panduan ini menjelaskan langkah demi langkah untuk merakit UI **Game Over** di Unity Editor serta menghubungkannya dengan script **`PlayerHealth.cs`** dan **`GameplayMenuController.cs`**.

---

## 🖥️ Langkah 1: Membuat UI Panel Game Over
Kita perlu membuat tampilan menu Game Over di dalam Canvas:
1. Di jendela **Hierarchy**, klik kanan pada Canvas Anda -> **Create Empty**, beri nama: **`GameOverPanel`**.
2. Di komponen **Rect Transform** milik `GameOverPanel`, klik kotak jangkar (Anchor Presets) sambil menahan tombol **`Alt`** di keyboard, lalu pilih kotak pojok kanan bawah (stretch/penuh) agar ukurannya memenuhi seluruh layar.
3. Di dalam `GameOverPanel`, tambahkan:
   * **Background:** Klik kanan pada `GameOverPanel` -> **UI -> Image**. Beri warna hitam transparan (misalnya Alpha/transparansi diset ke `180`) agar layar belakang sedikit meredup saat Game Over.
   * **Teks Judul:** Klik kanan pada `GameOverPanel` -> **UI -> Text - TextMeshPro**. Ubah isinya menjadi **`GAME OVER`**, atur ukuran font besar, warna merah, dan letakkan di bagian tengah atas.
   * **Tombol Restart:** Klik kanan pada `GameOverPanel` -> **UI -> Button - TextMeshPro**. Beri nama tombol: **`Btn_Restart`**. Ubah teks di dalamnya menjadi **`RESTART`**.
   * **Tombol Exit/Lobby:** Klik kanan pada `GameOverPanel` -> **UI -> Button - TextMeshPro**. Beri nama tombol: **`Btn_Lobby`**. Ubah teks di dalamnya menjadi **`MAIN MENU`** atau **`HOME`**.
4. Susun tata letak tombol secara rapi di tengah layar.
5. Setelah semuanya rapi, nonaktifkan panel ini dengan menghilangkan centang (uncheck) pada nama **`GameOverPanel`** di bagian paling atas Inspector (agar panel ini tersembunyi saat game dimulai).

---

## 🔗 Langkah 2: Menyambungkan Tombol ke Script
Agar tombol-tombol tersebut berfungsi saat diklik:

### 1. Tombol Restart (Mengulang Level)
1. Pilih **`Btn_Restart`** di Hierarchy.
2. Di panel Inspector, cari bagian **On Click ()** di komponen **Button**, lalu klik tombol **`+`**.
3. Tarik objek **`UI_Manager`** (atau objek yang memegang script **`GameplayMenuController`**) ke kolom kosong *Object*.
4. Klik dropdown *No Function* -> pilih **`GameplayMenuController` -> `RestartLevel()`**.

### 2. Tombol Main Menu (Kembali ke Lobby)
1. Pilih **`Btn_Lobby`** di Hierarchy.
2. Di panel Inspector pada bagian **On Click ()**, klik tombol **`+`**.
3. Tarik objek **`UI_Manager`** (atau objek pemegang script **`GameplayMenuController`**) ke kolom kosong *Object*.
4. Klik dropdown *No Function* -> pilih **`GameplayMenuController` -> `GoToLobby()`**.

---

## ⚙️ Langkah 3: Konfigurasi Inspector pada Player & UI Manager
Agar sistem mendeteksi panel dan animasi dengan benar:

### 1. Di Objek UI Manager (atau Pemegang `GameplayMenuController`):
1. Pilih objek pemegang **`GameplayMenuController`** di Hierarchy.
2. Tarik objek **`GameOverPanel`** yang sudah Anda buat di Langkah 1 ke kolom **`Game Over Panel`** pada script di Inspector.

### 2. Di Objek Player (Pemegang `PlayerHealth`):
1. Pilih objek **`Player`** di Hierarchy.
2. Di panel Inspector pada komponen script **`Player Health`**, isi kolom berikut:
   * **Animator:** Tarik komponen **Animator** milik Player Anda ke kolom ini (jika dibiarkan kosong, script akan mencarinya secara otomatis).
   * **Menu Controller:** Tarik objek pemegang script **`GameplayMenuController`** (misal objek `UI_Manager`) ke kolom ini.

---

Sistem alur kematian Anda sekarang sudah siap diuji! Ketika darah Player mencapai `0`, kontrol player akan terkunci, animasi `"Death"` akan diputar, dan layar Game Over akan muncul untuk memberi opsi mengulang level atau kembali ke menu utama.
