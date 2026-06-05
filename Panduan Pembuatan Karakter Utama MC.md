# Panduan Lengkap Pembuatan Karakter Utama (MC) di Unity

Aset spritesheet MC Anda **sangat bagus!** Karakter perempuan dengan kucir rambut dan pistol ini sangat cocok untuk game shooter top-down. Gambar spritesheet tersebut sudah memiliki baris animasi lengkap (Idle, Walk/Run, dan Shooting).

Script C# **`PlayerController.cs`** Anda sudah siap digunakan. Berikut adalah panduan langkah demi langkah untuk merakit karakter utama (MC) Anda dari gambar tersebut agar bisa digerakkan, berputar mengikuti arah mouse, dan menembak:

---

## ✂️ LANGKAH 1: Pengaturan Import Gambar (Sprite Slicing)

Sebelum membuat karakter, kita harus memotong-motong spritesheet tersebut menjadi bagian-bagian terpisah di Unity:

1. Pilih file gambar **`MC`** di jendela Project Anda.
2. Di jendela **Inspector** sebelah kanan, atur pengaturannya sebagai berikut:
   * **Texture Type:** `Sprite (2D and UI)`
   * **Sprite Mode:** `Multiple` (Karena gambar berisi banyak bingkai animasi)
   * **Pixels Per Unit:** Sesuaikan dengan ukuran pixel art Anda (biasanya `16`, `32`, atau `64` depending on asset size. Coba biarkan default `100` terlebih dahulu).
   * **Sprite Mesh Type:** `Full Rect`
   * **Filter Mode:** **`Point (no filter)`** *(PENTING! Agar karakter Pixel Art Anda tetap tajam, bersih, dan tidak blur/buram!)*
   * **Compression:** `None` (Agar warna pixel art tetap solid dan berkualitas tinggi).
3. Klik tombol **Apply** di bagian bawah.
4. **Memotong Gambar (Sprite Editor):**
   - Klik tombol **Sprite Editor** di Inspector.
   - Di jendela Sprite Editor, klik menu dropdown **Slice** di bagian atas.
   - Pilih **Type:** **`Grid By Cell Count`** (Karena format gambar Anda berbentuk kotak ubin baris & kolom yang rapi).
   - Atur **Columns (Kolom):** **`8`** dan **Rows (Baris):** **`8`** (Sesuai jumlah pembagian gambar Anda).
   - Klik tombol **Slice**, lalu klik **Apply** di pojok kanan atas Sprite Editor, kemudian tutup jendelanya.
   *(Sekarang di folder Project, gambar MC Anda bisa di-expand dan sudah terpotong rapi dari `MC_0` sampai `MC_63`!)*

---

## 🏗️ LANGKAH 2: Membuat GameObject MC di Hierarchy

1. Klik kanan di jendela **Hierarchy** -> **Create Empty**. Beri nama **`Player`**.
2. **Tambahkan Komponen yang Dibutuhkan:**
   Klik tombol **Add Component** di Inspector objek `Player`, lalu tambahkan komponen-komponen berikut:
   * **`Sprite Renderer`**:
     - Masukkan sprite **`MC_0`** (karakter berdiri menghadap kanan) ke kolom **Sprite**.
   * **`Rigidbody 2D`** (Untuk pergerakan fisika & kolisi):
     - **Body Type:** `Dynamic`
     - **Collision Detection:** `Continuous` (Mencegah karakter menembus dinding saat bergerak cepat).
     - **Sleeping Mode:** `Never Sleep`
     - **Gravity Scale:** **`0`** *(PENTING! Karena ini game top-down dari atas, jika tidak di-nol-kan, karakter Anda akan jatuh ke bawah!).*
     - **Constraints:** Expand menu ini, lalu centang **`Freeze Rotation Z`** *(PENTING! Agar karakter Anda tidak berputar-putar seperti gasing saat menabrak tembok).*
   * **`Capsule Collider 2D`** atau **`CircleCollider2D`** (Untuk kolisi tubuh):
     - Atur posisinya berada di area kaki karakter (bagian bawah tubuh) agar interaksi kolisi dengan dinding/rintangan terasa realistis.
   * **`Animator`** (Untuk mengontrol animasi).
   * **`Player Input`** (Jika Anda menggunakan Unity New Input System):
     - Masukkan file pengaturan input actions Anda ke kolom **Actions**.
   * **`PlayerController`** (Script C# pergerakan Anda).

---

## 🎯 LANGKAH 3: Menghubungkan C# Script PlayerController

Pilih objek **`Player`** di Hierarchy. Di komponen **Player Controller (Script)** pada Inspector, hubungkan referensi berikut:
1. **`Move Speed`:** Isi dengan **`5`**.
2. **`Bullet Prefab`:** Seret prefab peluru Anda dari folder Project ke kolom ini.
3. **`Fire Rate`:** Isi dengan **`0.3`**.
4. **`Rb`:** Seret objek **`Player`** itu sendiri ke kolom ini (atau biarkan kosong karena script akan mencarinya otomatis saat *Awake*).
5. **`Main Camera`:** Seret kamera utama Anda ke sini.
6. **`Animator`** & **`Sprite Renderer`**: Seret objek **`Player`** Anda sendiri ke kedua kolom ini.

### 🔫 Membuat Spawn Point Peluru (FirePoint):
1. Klik kanan pada objek **`Player`** di Hierarchy -> **Create Empty** (sebagai anak objek Player). Beri nama **`FirePoint`**.
2. Di layar Scene View, geser posisi `FirePoint` ini tepat di depan ujung moncong pistol karakter Anda.
3. Pilih kembali objek **`Player`**, lalu seret objek **`FirePoint`** dari Hierarchy ke kolom **`Fire Point`** di script *PlayerController* pada Inspector.

---

## 🎬 LANGKAH 4: Membuat Animasi (Idle, Walk, & Shoot)

Berdasarkan spritesheet Anda, pembagian baris animasinya adalah:
* **Idle (Diam):** Baris ke-7 (Misal `MC_48` - `MC_52`)
* **Walk (Berjalan):** Baris ke-3 (Misal `MC_16` - `MC_23`)
* **Shoot (Menembak):** Baris ke-5 (Misal `MC_32` - `MC_36`)

### 1. Membuat Klip Animasi:
1. Buka jendela **Animation** (Menu **Window** -> **Animation** -> **Animation**).
2. Pilih objek **`Player`** di Hierarchy, lalu klik tombol **Create** di jendela Animation.
3. Simpan dengan nama **`Player_Idle`**:
   - Seret sprite baris Idle (misal `MC_48` sampai `MC_52`) dari Project Window ke dalam timeline jendela Animation.
4. Klik dropdown nama animasi di pojok kiri atas jendela Animation -> pilih **Create New Clip**.
5. Simpan dengan nama **`Player_Walk`**:
   - Seret sprite baris jalan (misal `MC_16` sampai `MC_23`) ke dalam timeline.
6. Buat klip ketiga, simpan dengan nama **`Player_Shoot`**:
   - Seret sprite baris menembak (misal `MC_32` sampai `MC_36`) ke dalam timeline.

---

## 🎛️ LANGKAH 5: Mengatur Animator Controller (State Machine)

1. Klik dua kali file **Animator Controller** Anda (yang terbuat otomatis saat membuat klip animasi tadi) untuk membuka jendela **Animator**.
2. **Buat Parameter:**
   Di tab **Parameters** (pojok kiri atas jendela Animator), buat dua parameter baru:
   * Klik ikon **`+`** -> pilih **Float**. Beri nama **`Speed`** (Harus sama persis huruf besar-kecilnya karena dipanggil di script!).
   * Klik ikon **`+`** -> pilih **Trigger**. Beri nama **`Shoot`**.
3. **Mengatur Transisi Pergerakan (Idle <-> Walk):**
   * Klik kanan pada state **`Player_Idle`** -> **Make Transition** -> arahkan ke **`Player_Walk`**.
     - Klik garis panah transisi tersebut: di Inspector sebelah kanan, hilangkan centang **`Has Exit Time`**, ubah **`Transition Duration`** menjadi **`0`**, lalu di bagian **Conditions** klik `+` dan set: **`Speed > 0.1`**.
   * Klik kanan pada **`Player_Walk`** -> **Make Transition** -> arahkan ke **`Player_Idle`**.
     - Klik garis panah transisinya: hilangkan centang **`Has Exit Time`**, ubah **`Transition Duration`** menjadi **`0`**, lalu di bagian **Conditions** set: **`Speed < 0.1`**.
4. **Mengatur Animasi Tembakan (Any State -> Shoot):**
   * Klik kanan pada kotak **`Any State`** -> **Make Transition** -> arahkan ke **`Player_Shoot`**.
     - Klik garis panahnya: hilangkan centang **`Has Exit Time`**, ubah **`Transition Duration`** menjadi **`0`**, lalu set **Conditions** ke: **`Shoot`**.
   * Klik kanan pada **`Player_Shoot`** -> **Make Transition** -> arahkan ke **`Player_Idle`** (atau ke state default Anda).
     - Klik garis panahnya: **Biarkan `Has Exit Time` tetap tercentang** (agar animasi menembak selesai dimainkan terlebih dahulu sebelum kembali ke posisi diam).

---

### 🎉 SELESAI!
Sekarang jalankan game Anda di Unity. Karakter Anda akan:
1. **Diam (Idle)** secara default.
2. **Berjalan (Walk)** secara mulus saat Anda menekan tombol **WASD**, dan berganti ke animasi jalan.
3. **Menghadap Mouse:** Tubuh karakter otomatis berbalik arah (Flip X) mengikuti posisi kursor mouse Anda secara presisi!
4. **Menembak:** Saat Anda klik kiri mouse, moncong pistol akan mengeluarkan peluru prefab yang meluncur lurus, dan karakter memainkan animasi **Shoot** dengan sangat dinamis!
