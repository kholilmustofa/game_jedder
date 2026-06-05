# Panduan Pembuatan Level Select (Scene Terpisah / Opsi 1)

Pilihan yang sangat tepat! Membuat layar pilih level dalam **Scene Terpisah** adalah standar industri game agar proyek Anda tetap rapi, performa RAM stabil, dan tidak membingungkan saat mengedit di Unity.

Saya telah membuatkan dua script C# siap pakai di dalam proyek Anda:
1. **[LevelSelectController.cs](file:///d:/Unity/jedder/Assets/Scripts/LevelSelectController.cs)** (Untuk mengontrol gembok dan pemuatan level di scene baru).
2. **[LevelCompleteTrigger.cs](file:///d:/Unity/jedder/Assets/Scripts/LevelCompleteTrigger.cs)** (Untuk membuka kunci Level 2 secara permanen setelah menyelesaikan Level 1).

Berikut adalah panduan lengkap dari awal untuk merakit sistem pilih level scene terpisah seperti di Figma Anda:

---

## 🎬 LANGKAH 1: Membuat & Mendaftarkan Scene Baru

1. **Buat Scene Baru:**
   - Di jendela **Project** Unity (bagian bawah), masuk ke folder **`Assets > Scenes`**.
   - Klik kanan di area kosong -> **Create** -> **Scene**.
   - Beri nama scene baru ini: **`LevelSelect`**.
2. **Daftarkan di Build Settings (SANGAT PENTING):**
   - Pergi ke menu atas Unity: **File** -> **Build Settings...**
   - Seret scene **`LevelSelect`** Anda dari folder Project ke dalam kotak daftar **Scenes In Build** (bersama scene `lobby` atau scene menu utama Anda).
   - Tutup jendela Build Settings.

---

## 🎨 LANGKAH 2: Merancang UI Level Select di Scene Baru

1. Buka scene **`LevelSelect`** baru Anda dengan cara klik dua kali pada file scene-nya di folder Project.
2. **Buat Canvas:**
   - Klik kanan di Hierarchy -> **UI** -> **Canvas**.
3. **Ubah Warna Background Layar:**
   - Pada **Main Camera** di Hierarchy, ubah **Background Color** menjadi warna biru-teal indah yang Anda gunakan di Figma.
4. **Merancang Tombol Klik Transparan di Atas Tebing Tilemap:**
   Karena tebing Level 1 & 2 Anda dilukis menggunakan **Tilemap (Grid)** di latar belakang, kita akan menggunakan **Tombol Transparan (Invisible Button)** di Canvas agar melayang tepat di atas tebing tersebut:
   
   * **Tombol Level 1 (`Level1Button`):**
     - Klik kanan pada **Canvas** -> **UI** -> **Button (TextMesh Pro)**. Beri nama **`Level1Button`**.
     - **Hapus Teks Bawaan:** Hapus objek anak **`Text (TMP)`** di dalam tombol tersebut.
     - **Jadikan Transparan:** Klik `Level1Button`, cari komponen **Image** di Inspector, klik kotak **Color**, lalu ubah nilai **`Alpha (A)`** menjadi **`0`** (transparan penuh).
     - **Posisikan:** Di Game View, geser dan sesuaikan ukuran (*Width & Height*) tombol transparan ini agar menutupi permukaan tebing Level 1 pada Tilemap Anda.
   
   * **Tombol Level 2 (`Level2Button`):**
     - Lakukan hal yang sama untuk membuat tombol transparan kedua, beri nama **`Level2Button`**.
     - Jadikan transparan penuh (Alpha A = 0), lalu posisikan tepat di atas tebing Level 2 pada Tilemap Anda.

5. **Menambahkan Bayangan Hitam & Gembok (Lock Container) di atas Tebing Level 2:**
   Agar tebing Level 2 terlihat agak kehitaman (tertutup bayangan gelap) saat terkunci, kita akan membuat wadah penutup bayangan dan gembok di dalam `Level2Button`:
   
   - **Buat Wadah Kontainer:** Klik kanan pada **`Level2Button`** -> **Create Empty** (sebagai anak tombol). Beri nama objek ini **`LockContainer`**.
   - **Buat Lapisan Bayangan Gelap (Shadow Overlay):**
     - Klik kanan pada **`LockContainer`** -> **UI** -> **Image**. Beri nama **`ShadowOverlay`**.
     - Klik objek `ShadowOverlay`, cari komponen **Image** di Inspector -> klik kotak **Color**.
     - Ubah warnanya menjadi **Hitam** (`#000000`) dan kurangi **Alpha (A)** menjadi sekitar **`130`** (sesuaikan tingkat kegelapan bayangan yang Anda inginkan).
     - Atur ukurannya (*Width & Height*) agar pas menutupi tebing Level 2 di belakangnya.
   - **Buat Gambar Gembok (Lock Icon):**
     - Klik kanan pada **`LockContainer`** -> **UI** -> **Image**. Beri nama **`lockIcon`**.
     - Di Inspector pada komponen **Image**, masukkan gambar gembok putih Anda ke **Source Image**.
     - Centang **Preserve Aspect** agar tidak gepeng, lalu atur ukurannya agar pas di tengah-tengah tebing.
   
   *Penting: Pastikan urutan susunannya di Hierarchy seperti di bawah ini, agar gembok tetap terlihat terang di depan bayangan hitam:*
   ```text
   ▼ Level2Button
       ▼ LockContainer
           ShadowOverlay  <-- (Di atas/pertama, agar meredupkan tebing)
           lockIcon       <-- (Di bawah/kedua, agar tetap terang di depan bayangan)
   ```

6. **Tambahkan Teks & Awan:**
   - Tambahkan objek **Text (TMP)** anak dari Canvas, beri nama **`Select Level`** untuk judul di bagian atas.
   - Tambahkan objek **Image** anak dari Canvas untuk dekorasi awan-awan bergerak yang indah di sudut layar.

---

## ⚙️ LANGKAH 3: Menghubungkan Logika Lock & Unlock Level 2

1. **Buat Pengontrol Level Select:**
   - Di scene `LevelSelect` Anda, klik kanan di Hierarchy -> **Create Empty**. Beri nama **`LevelSelectManager`**.
   - Tarik/drag script **`LevelSelectController`** ke objek `LevelSelectManager` tersebut.
2. **Hubungkan Objek di Inspector `LevelSelectManager`:**
   - Klik `LevelSelectManager` di Hierarchy. Di Inspector sebelah kanan, isi kolom berikut:
     * **`Level 2 Button`**: Seret objek **`Level2Button`** dari Hierarchy ke sini.
     * **`Level 2 Lock Icon`**: Seret objek **`LockContainer`** (kontainer berisi bayangan & gembok) dari Hierarchy ke sini.
     * **`Level 1 Scene Name`**: Isi dengan nama scene gameplay level 1 Anda (misalnya `"lobby"`).
     * **`Level 2 Scene Name`**: Isi dengan nama scene gameplay level 2/Boss Anda (misalnya `"Level2"`).
3. **Hubungkan Event Klik Tombol:**
   - **Pada `Level1Button`:**
     - Cari komponen **On Click ()** di Inspector -> klik **`+`**.
     - Seret objek **`LevelSelectManager`** ke kotak objek kosong.
     - Pilih dropdown -> **`LevelSelectController`** -> **`LoadLevel1()`**.
   - **Pada `Level2Button`:**
     - Cari komponen **On Click ()** di Inspector -> klik **`+`**.
     - Seret objek **`LevelSelectManager`** ke kotak objek.
     - Pilih dropdown -> **`LevelSelectController`** -> **`LoadLevel2()`**.

---

## 🚪 LANGKAH 4: Menghubungkan Main Menu ke Scene `LevelSelect`

Sekarang, kita harus memastikan tombol **PLAY** di Main Menu Anda memuat scene pilih level baru ini:

1. Buka kembali scene **Main Menu / Menu Utama** Anda.
2. Pilih objek **`MainMenuPanel`** Anda di Hierarchy (yang memegang script `MainMenuController`).
3. Di Inspector sebelah kanan pada komponen **MainMenuController**:
   - **Centang pilihan `Use Load Scene`** (diubah menjadi True).
   - Isi kolom **`Level Select Scene Name`** dengan tulisan: **`LevelSelect`** (harus sama persis dengan nama file scene).
4. Sekarang, saat pemain mengklik tombol **PLAY** di Main Menu, game akan otomatis memuat layar pilih level Anda yang indah!

---

## 🏆 LANGKAH 5: Membuka Kunci Level 2 Secara Permanen

Saat pemain berhasil memenangkan Level 1 (misalnya menyentuh gerbang/pintu keluar):

1. Masuk ke scene gameplay Level 1 Anda.
2. Pilih objek **Pintu Keluar / Portal** di akhir level.
3. Tambahkan **BoxCollider2D** dan centang **`Is Trigger`**.
4. Tarik script **`LevelCompleteTrigger.cs`** ke objek Pintu Keluar tersebut.
5. Pastikan karakter utama (Player) Anda memiliki tag **`Player`** di Inspector-nya.
6. *Ketika pemain melewati pintu keluar ini, Level 2 akan otomatis terbuka selamanya!*

---

## 🛠️ LANGKAH 6: Cara Menguji Sistem (Testing Rahasia)

Untuk mengetes kunci level tanpa harus bermain Level 1 dari awal:
1. Masuk ke scene **`LevelSelect`**.
2. Klik objek **`LevelSelectManager`** di Hierarchy.
3. Di Inspector, **klik kanan** pada tulisan komponen *Level Select Controller (Script)*.
4. Pilih menu rahasia ini:
   - **`Unlock Level 2`**: Untuk langsung membuka gembok Level 2 secara instan.
   - **`Reset Progress`**: Untuk mengunci kembali Level 2 menjadi abu-abu dan gembok muncul.
