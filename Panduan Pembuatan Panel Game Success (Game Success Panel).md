# Panduan Pembuatan Panel Game Success (Level Clear)

Panduan ini menjelaskan langkah demi langkah untuk membuat UI Panel Game Success di Unity Editor dan menghubungkannya dengan script `GameplayMenuController`.

---

## Langkah 1: Struktur Hierarchy Panel di Unity

1. Buka scene gameplay Anda (misal: `Level1.unity`).
2. Di dalam **Canvas** gameplay Anda:
   * Klik kanan pada Canvas -> **UI** -> **Panel**.
   * Ubah nama Panel baru ini menjadi **`SuccessMenuPanel`**.
   * Di Inspector `SuccessMenuPanel`, ubah warna background panel sesuai selera (atau gunakan aset Banner jika menggunakan manual 9-slicing).
3. Di dalam `SuccessMenuPanel`, tambahkan elemen-elemen berikut:
   * **Title Text**: Klik kanan pada `SuccessMenuPanel` -> **UI** -> **Text - TextMeshPro**. Beri tulisan seperti **"LEVEL CLEAR!"** atau **"VICTORY"** dengan gaya font premium (misal: warna emas/kuning).
   * **Button Container**: Klik kanan pada `SuccessMenuPanel` -> **UI** -> **Panel** atau **Create Empty**. Atur agar menjadi tempat berbarisnya tombol-tombol.
4. Di dalam Button Container, buat 3 tombol (menggunakan **UI** -> **Button - TextMeshPro**):
   * **`Button_Next`**: Tombol untuk lanjut ke level berikutnya.
   * **`Button_Retry`**: Tombol untuk mengulang level saat ini.
   * **`Button_LevelSelect`**: Tombol untuk kembali ke menu pilih level.

---

## Langkah 2: Menghubungkan Tombol dengan Script `GameplayMenuController`

Tarik fungsi-fungsi dari `GameplayMenuController` ke event **On Click ()** di Inspector masing-masing tombol:

1. **Tombol Next (`Button_Next`)**:
   * Klik tanda **`+`** pada event **On Click ()** di Inspector.
   * Seret GameObject yang memiliki script `GameplayMenuController` (biasanya `GameplayMenuController` itu sendiri atau GameObject `GameManager`) ke dalam kotak Object.
   * Pada dropdown fungsi, pilih **`GameplayMenuController`** -> **`LoadNextLevel()`**.

2. **Tombol Retry (`Button_Retry`)**:
   * Klik tanda **`+`** pada event **On Click ()**.
   * Seret GameObject dengan script `GameplayMenuController` ke kotak Object.
   * Pilih fungsi **`GameplayMenuController`** -> **`RestartLevel()`**.

3. **Tombol Level Select (`Button_LevelSelect`)**:
   * Klik tanda **`+`** pada event **On Click ()**.
   * Seret GameObject dengan script `GameplayMenuController` ke kotak Object.
   * Pilih fungsi **`GameplayMenuController`** -> **`GoToLevelSelect()`**.

---

## Langkah 3: Assign Panel ke Inspector `GameplayMenuController`

1. Pilih GameObject yang memiliki script `GameplayMenuController` di Hierarchy.
2. Di Inspector komponen `GameplayMenuController`, Anda sekarang akan melihat slot baru bernama **`Game Success Panel`**.
3. Seret GameObject **`SuccessMenuPanel`** dari Hierarchy ke slot **`Game Success Panel`** tersebut.
4. Matikan (uncheck) **`SuccessMenuPanel`** di Hierarchy agar tersembunyi saat game dimulai. Script akan mengaktifkannya secara otomatis ketika level selesai.

---

## Langkah 4: Cara Memasang Pintu/Portal Finish (`LevelCompleteTrigger`)

Untuk memicu panel Game Success dan membuka kunci Level 2 saat Player berjalan menyentuh pintu keluar/portal:

### Cara Konfigurasi di Unity Editor:
1. **Buat Objek Pintu/Portal**:
   * Klik kanan di Hierarchy -> **Create Empty** (atau gunakan Sprite pintu/portal yang Anda miliki).
   * Beri nama objek tersebut **`PortalFinish`**.
2. **Pasang Collider 2D**:
   * Di Inspector `PortalFinish`, klik **Add Component** -> cari **`Box Collider 2D`** (atau collider lainnya).
   * **PENTING**: Centang kotak **`Is Trigger`** (True) pada komponen Collider tersebut agar player bisa melewatinya dan memicu tabrakan tanpa terbentur secara fisik.
3. **Pasang Script `LevelCompleteTrigger`**:
   * Klik **Add Component** -> cari dan tambahkan script **`LevelCompleteTrigger`**.
4. **Pastikan Tag Player Sesuai**:
   * Pilih GameObject Player Anda di Hierarchy.
   * Pastikan **Tag** di bagian atas Inspector Player diset ke **`Player`** (karena script mengecek tag tabrakan `collision.CompareTag("Player")`).
5. **Jalankan & Tes Game**:
   * Gerakkan Player Anda untuk menyentuh objek `PortalFinish`.
   * Saat disentuh, data progres Level 2 otomatis tersimpan di `PlayerPrefs`, panel Game Success akan muncul di layar, dan gembok Level 2 di scene Level Select akan terbuka!

---

> [!IMPORTANT]
> Untuk membuat tombol **Next Level** bekerja dengan benar, pastikan semua scene level Anda sudah dimasukkan ke dalam daftar **Build Settings** di Unity:
> 1. Pergi ke **File** -> **Build Settings...**
> 2. Seret semua scene level Anda secara berurutan (misalnya: `Lobby` di index 0, `LevelSelect` di index 1, `Level1` di index 2, `Level2` di index 3, dst).
> 3. Tombol **Next** akan memuat scene dengan urutan build index saat ini + 1.
