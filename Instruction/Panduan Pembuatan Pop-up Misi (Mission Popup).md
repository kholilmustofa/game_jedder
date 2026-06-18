# Panduan Konfigurasi Pop-up Misi di Unity Editor (Level 1 & Level 2)

Panduan ini menjelaskan langkah demi langkah untuk menyusun dan menghubungkan UI **Mission Popup Panel** di Unity Editor menggunakan **GameplayMenuController** yang telah kita perbarui.

---

## Langkah 1: Membuat UI Panel Misi di Hierarchy

1. Buka scene `Level1` atau `Level2` di Unity.
2. Di jendela **Hierarchy**, temukan objek **Canvas** utama (tempat diletakkannya panel UI lain seperti `PauseMenuPanel`, `GameOverPanel`, dll).
3. Klik kanan pada **Canvas** -> **UI** -> **Panel** (atau **Image**).
4. Beri nama GameObject baru tersebut: `MissionPopupPanel`.
5. Di komponen **Image** pada `MissionPopupPanel`, Anda bisa memasukkan sprite gulungan kertas/scroll (seperti gambar referensi) jika ada di aset proyek Anda. Atur warnanya agar pas.
6. Buat struktur objek anak (Child Objects) di bawah `MissionPopupPanel`:
   * **Title (Text - TextMeshPro)**: 
     * Isi dengan: `Mission:`
     * Sesuaikan ukuran, font, warna merah/cokelat gelap, dan posisi di bagian atas gulungan.
   * **Description (Text - TextMeshPro)**:
     * Isi dengan misi level tersebut.
     * **Level 1**:
       > Kalahkan semua prajurit yang ada dan temukan kekuatan tersembunyi. Bersihkan setiap petak dan cari pintu keluar.
     * **Level 2**:
       > (Isi sesuai deskripsi misi Level 2 Anda)
   * **Close Button (Button / Button - TextMeshPro)**:
     * Buat tombol bertipe **Button** (atau **UI Button**).
     * Berikan gambar silang merah ("X") sebagai sprite tombol tersebut.
     * Posisikan tombol di pojok kanan atas gulungan kertas sesuai referensi visual.

---

## Langkah 2: Menghubungkan Tombol Close ke Script

1. Klik objek tombol **Close Button** ("X") yang baru saja Anda buat.
2. Di jendela **Inspector**, cari komponen **Button (Script)** dan temukan bagian **On Click ()**.
3. Klik tanda **+** (plus) untuk menambahkan event click baru.
4. Seret objek **GameplayMenuController** (atau objek manager lain tempat script ini menempel) dari **Hierarchy** ke kolom objek kosong (berlabel *None (Object)*).
5. Pada menu dropdown fungsi, pilih:
   `GameplayMenuController` -> `CloseMissionPopup`
6. Simpan perubahan.

---

## Langkah 3: Menghubungkan Panel ke GameplayMenuController

1. Klik objek di **Hierarchy** tempat script **GameplayMenuController** terpasang.
2. Di jendela **Inspector**, lihat komponen **Gameplay Menu Controller (Script)**.
3. Sekarang ada field baru bernama **Mission Popup Panel**.
4. Seret objek `MissionPopupPanel` yang Anda buat pada Langkah 1 dari **Hierarchy** ke slot **Mission Popup Panel** di inspector tersebut.
5. Nonaktifkan (uncheck/deactivate) `MissionPopupPanel` di Hierarchy terlebih dahulu agar ia otomatis diaktifkan lewat kode program saat game dimulai, atau biarkan aktif (kode program `Start()` akan otomatis mengaktifkannya dan melakukan jeda game secara instan).

---

## Langkah 4: Menambahkan Musik khusus saat Misi Muncul

1. Buat Game Object baru di Hierarchy (klik kanan -> **Audio** -> **Audio Source**) atau gunakan objek audio yang sudah ada. Beri nama: `MissionPopupMusic`.
2. Masukkan klip suara/musik bertema pembukaan misi ke dalam kolom **AudioClip** pada komponen Audio Source tersebut.
3. Hilangkan centang pada opsi **Play On Awake** pada `MissionPopupMusic` agar musiknya dikontrol secara penuh oleh script.
4. Jika musiknya merupakan lagu pendek looping, centang opsi **Loop**.
5. Pilih objek di **Hierarchy** tempat script **GameplayMenuController** terpasang.
6. Seret objek **`MissionPopupMusic`** dari Hierarchy ke slot **Mission Popup Music** di jendela Inspector.
7. **Cara Kerjanya secara Otomatis**:
   * Saat Level baru dimuat, BGM level utama (`Background Music`) akan dihentikan sementara agar suaranya tidak bertabrakan.
   * Musik popup misi (`Mission Popup Music`) akan mulai diputar.
   * Begitu panel misi ditutup, musik misi akan dimatikan dan BGM level utama akan otomatis mulai diputar secara normal.

---

## Langkah 5: Menambahkan Animasi Keluar (Exit Animation) pada Pop-up

Agar pop-up menutup dengan animasi yang halus (misalnya mengecil ke ukuran 0 atau memudar):

1. **Membuat File Animasi**:
   * Pilih objek `MissionPopupPanel` di Hierarchy.
   * Buka jendela **Animation** (**Window** -> **Animation** -> **Animation**).
   * Klik tombol **Create** untuk membuat Animator Controller baru secara otomatis beserta clip animasi. Simpan dengan nama `MissionPopup_Close.anim`.
   * Pada timeline animasi:
     * Tambahkan properti **Transform** -> **Scale**.
     * Pada detik `0.00`, biarkan scale bernilai `(1, 1, 1)`.
     * Pada detik `0.30` (atau durasi yang Anda inginkan, misal 0.3 detik), ubah scale menjadi `(0, 0, 0)`.
2. **Mengatur Animator Controller**:
   * Buka jendela **Animator** (**Window** -> **Animation** -> **Animator**).
   * Buat state default baru untuk keadaan diam/terbuka (klik kanan di area kosong -> **Create State** -> **Empty**), beri nama `Idle`.
   * Set state `Idle` ini sebagai state default (**Set as Layer Default State**).
   * Buat transisi dari `Idle` ke state `MissionPopup_Close` (klik kanan `Idle` -> **Make Transition** -> arahkan ke `MissionPopup_Close`).
   * Di tab **Parameters** (pojok kiri atas jendela Animator), klik tanda **+** dan pilih **Trigger**. Beri nama trigger tersebut: `Close`.
   * Klik garis transisi dari `Idle` ke `MissionPopup_Close`:
     * Di bagian **Conditions**, tambahkan kondisi baru dan pilih trigger `Close`.
     * Hilangkan centang pada opsi **Has Exit Time** dan atur **Transition Duration** ke `0` agar transisi langsung terjadi secara instan saat tombol diklik.
3. **PENTING: Mengatur Update Mode ke Unscaled Time**:
   * Pilih objek `MissionPopupPanel` di Hierarchy.
   * Di jendela **Inspector**, temukan komponen **Animator**.
   * Ubah opsi **Update Mode** dari **Normal** menjadi **Unscaled Time**. 
     > [!IMPORTANT]
     > Langkah ini wajib dilakukan karena game sedang dalam kondisi jeda (`Time.timeScale = 0f`). Jika tetap disetel ke Normal, animasi tidak akan bisa berjalan saat game di-pause.
4. **Hubungkan Animator ke Script**:
   * Pilih objek tempat script **GameplayMenuController** berada di Hierarchy.
   * Tarik komponen **Animator** dari objek `MissionPopupPanel` ke kolom **Mission Popup Animator** di Inspector.
   * Masukkan angka durasi animasi keluar Anda (misal `0.3`) pada kolom **Mission Close Anim Duration** agar jeda transisi script sesuai dengan panjang animasi.

---

## Langkah 6: Melakukan Uji Coba (Testing)

1. Tekan tombol **Play** di Unity Editor.
2. **Hasil yang Diharapkan**:
   * Game langsung masuk kondisi **Pause** di awal level.
   * Panel misi (`MissionPopupPanel`) muncul menutupi layar.
   * Musik pembukaan misi (`MissionPopupMusic`) mulai berputar secara terpisah, sementara BGM level utama (`Background Music`) mati/diam.
   * Kursor mouse berupa **Panah (Menu Cursor)**.
   * Klik tombol **"X"** (atau tekan **ESC** pada keyboard):
     * Suara klik tombol terputar.
     * Musik pembukaan misi langsung berhenti berputar, dan BGM level utama (`Background Music`) mulai berputar normal mengisi suasana level.
     * Panel misi akan memutar animasi mengecil (`Scale` menuju `0`) selama 0.3 detik.
     * Setelah animasi selesai, panel misi dinonaktifkan (`SetActive(false)`), permainan berlanjut normal, dan kursor kembali menjadi crosshair (bidikan).
