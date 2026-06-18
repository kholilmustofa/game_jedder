# Panduan Pembuatan Rule Tile (Tilemap Otomatis)

Panduan ini menjelaskan bagaimana cara membuat dan menggunakan **Rule Tile** di Unity agar Anda bisa melukis peta (seperti rumput, tanah, atau dinding) secara otomatis. Ketika Anda melukis, Unity akan mengatur sudut, tepi, dan bagian tengah secara otomatis berdasarkan ubin di sekitarnya.

---

## 🚀 BAGIAN 1: Cara Menggunakan Rule Tile yang Sudah Ada

Di dalam folder proyek Anda (`Assets/Tilemap/`), sudah terdapat asset Rule Tile bawaan seperti **`RumputOtomatis`**. Berikut cara menggunakannya:

1. Buka jendela **Tile Palette** di Unity editor Anda (seperti yang ada di kanan bawah screenshot Anda).
2. Di **Project Window** (bagian bawah), masuk ke folder **`Assets/Tilemap/`**.
3. Cari file **`RumputOtomatis`** (berlogo ikon ubin dengan aturan hijau/merah).
4. **Drag & Drop** (seret dan lepas) file **`RumputOtomatis`** tersebut langsung ke dalam area kotak-kotak di **Tile Palette** Anda.
5. Klik ikon **Brush** (Kuas) atau tekan tombol **`B`** pada keyboard.
6. Pilih ubin **`RumputOtomatis`** yang baru saja Anda masukkan di Tile Palette.
7. Mulailah melukis di **Scene View**! Ubin tersebut akan otomatis menyesuaikan diri (membentuk pulau/daratan dengan pinggiran yang rapi).

---

## 🛠️ BAGIAN 2: Cara Membuat Rule Tile Baru Dari Awal

Jika Anda memiliki aset sprite baru (misalnya tanah, air, atau pasir) dan ingin membuat ubin otomatis sendiri, ikuti langkah-langkah ini:

### Langkah 1: Buat File Rule Tile Baru
1. Klik kanan di folder **Project Window** (disarankan di dalam `Assets/Tilemap`).
2. Pilih **Create** -> **2D** -> **Tiles** -> **Rule Tile**.
3. Beri nama asset tersebut (contoh: `TanahOtomatis`).

### Langkah 2: Konfigurasi Default Sprite
1. Klik pada asset `TanahOtomatis` yang baru dibuat.
2. Lihat ke panel **Inspector** di sebelah kanan.
3. Pada pilihan **Default Sprite**, pilih gambar/sprite ubin bagian tengah (area rumput/tanah penuh tanpa pinggiran).

### Langkah 3: Menentukan Aturan Pencocokan (Tiling Rules)
Di bagian bawah Inspector, klik tombol **`+`** (Plus) untuk menambahkan aturan baru satu per satu untuk setiap jenis ubin (sudut kiri atas, tepi atas, sudut kanan atas, dsb.):

1. Pilih **Sprite** yang sesuai untuk aturan tersebut.
2. Klik kotak grid 3x3 di sebelah gambar sprite untuk menentukan hubungannya dengan ubin sekitarnya:
   - **Tanda Panah Hijau / Kotak Hijau**: Menandakan ubin sejenis **harus ada** di posisi tersebut.
   - **Tanda Silang Merah / Kotak Merah**: Menandakan ubin sejenis **tidak boleh ada** di posisi tersebut (biasanya digunakan untuk bagian tepi luar).
   - **Kotak Kosong**: Menandakan posisi tersebut **bebas** (tidak berpengaruh apakah ada ubin atau tidak).

#### 💡 Contoh Konfigurasi Rule Tile Dasar:
* **Sudut Kiri Atas**:
  * Silang Merah: Atas, Kiri, Atas-Kiri.
  * Panah Hijau: Bawah, Kanan, Bawah-Kanan.
* **Tepi Atas**:
  * Silang Merah: Atas.
  * Panah Hijau: Bawah, Kiri, Kanan.
* **Bagian Tengah (Fill)**:
  * Panah Hijau di semua 8 arah (atau cukup gunakan **Default Sprite** agar otomatis terisi jika tidak ada aturan lain yang cocok).

---

## 🎨 BAGIAN 3: Pengaturan Potongan Sprite (Sprite Slicing)

Agar Rule Tile bisa bekerja dengan baik, pastikan gambar tileset Anda sudah dipotong secara rapi:
1. Klik file gambar tileset/sprite sheet Anda di Project Window.
2. Di Inspector, ubah **Sprite Mode** menjadi **Multiple**.
3. Klik **Sprite Editor**.
4. Klik menu **Slice** di bagian atas, pilih tipe **Grid By Cell Size** (sesuaikan ukurannya, misalnya `16x16`, `32x32`, atau `64x64` pixel).
5. Klik **Slice** lalu klik **Apply** di sudut kanan atas Sprite Editor.
