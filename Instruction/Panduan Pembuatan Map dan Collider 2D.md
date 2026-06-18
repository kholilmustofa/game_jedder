# Panduan Pembuatan Map & Sistem Collider 2D (Anti-Tembus)

Panduan ini berisi langkah-langkah detail untuk merakit peta permainan 2D top-down di Unity Editor, serta cara memberikan komponen tabrakan (*collision*) pada tebing, air, atau dinding agar tidak dapat ditembus oleh Player.

---

## 🗺️ BAGIAN 1: Menyusun Struktur Tilemap di Hierarchy

Untuk mempermudah manajemen tabrakan, Anda wajib memisahkan antara ubin tanah biasa (yang boleh diinjak) dan ubin rintangan (tebing/tembok).

1. Klik kanan pada **`Grid`** di Hierarchy -> **2D Object** -> **Tilemap** -> **Rectangular**.
2. Buatlah minimal **2 layer Tilemap** dengan struktur berikut:
   * **`Tilemap_Ground`**: Khusus melukis rumput dasar, pasir, atau jalan setapak. *(Tanpa komponen tabrakan).*
   * **`Tilemap_Obstacles`**: Khusus melukis tebing, tembok, air dalam, pagar, atau pohon yang **tidak boleh ditembus**.

---

## 🛑 BAGIAN 2: Memasang Collider pada Tebing (Anti-Tembus)

Pilih objek **`Tilemap_Obstacles`** di Hierarchy Anda, lalu pasang komponen-komponen berikut di Inspector sebelah kanan:

### Langkah 1: Pasang `Tilemap Collider 2D`
* Klik **Add Component** -> cari dan pasang **`Tilemap Collider 2D`**.
* *Fungsi*: Unity secara otomatis akan membuat kotak tabrakan (garis hijau) di setiap kotak ubin tebing yang Anda lukis.

### Langkah 2: Pasang `Composite Collider 2D` (Optimasi & Anti-Sangkut)
Jika hanya menggunakan `Tilemap Collider 2D`, tabrakan antar ubin akan terpisah-pisah, membuat gerakan Player sering tersangkut di sambungan kotak ubin.
* Klik **Add Component** -> cari dan pasang **`Composite Collider 2D`**.
* Unity akan otomatis memasang komponen **`Rigidbody 2D`** secara bersamaan.

### Langkah 3: Konfigurasi Rigidbody 2D & Penyatuan Collider
1. Pada komponen **`Rigidbody 2D`** yang baru muncul:
   * Ubah **`Body Type`** menjadi **`Static`** (PENTING! Jika tetap *Dynamic*, peta tebing Anda akan jatuh ke bawah karena gravitasi).
2. Pada komponen **`Tilemap Collider 2D`**:
   * Centang kotak **`Used By Composite`**.
   * *Hasil*: Semua kotak tabrakan ubin yang menempel akan bersatu menjadi satu garis pembatas yang mulus tanpa celah!

---

## 🤠 BAGIAN 4: Pengaturan Collider pada Player (Penting!)

Agar sistem tabrakan tebing ini bekerja, objek **Player** Anda juga harus dikonfigurasi dengan benar:

1. Pilih objek **`Player`** di Hierarchy.
2. Pastikan memiliki komponen **`Capsule Collider 2D`** (atau `Box Collider 2D`):
   * Atur posisinya di Inspector agar kotak hijau hanya menutupi area **kaki** Player saja. Ini bertujuan agar bagian kepala/tubuh Player masih bisa menutupi bagian bawah tebing/pohon (efek kedalaman 3D/y-sorting).
3. Pastikan memiliki komponen **`Rigidbody 2D`**:
   * **`Gravity Scale`**: Ubah menjadi **`0`** (karena ini game top-down dari atas, kita tidak ingin Player jatuh karena gravitasi).
   * **`Collision Detection`**: Ubah ke **`Continuous`** (agar tabrakan lebih presisi dan tidak menembus tebing saat bergerak sangat cepat).
   * **`Constraints`** -> **`Freeze Rotation Z`**: **Centang (Enable)** (agar tubuh Player tidak berputar-putar seperti gasing saat menabrak dinding).

---

## 🎨 BAGIAN 5: Mengatur Urutan Gambar (Sorting Layer)

Agar tebing terlihat di atas rumput dan Player terlihat di depan tebing:

1. **`Tilemap_Ground`**:
   * Pada komponen **Tilemap Renderer** -> **Order in Layer**: Isi **`0`**.
2. **`Tilemap_Obstacles`** (Tebing):
   * Pada komponen **Tilemap Renderer** -> **Order in Layer**: Isi **`1`** atau **`2`**.
3. **`Player`**:
   * Pada komponen **Sprite Renderer** -> **Order in Layer**: Isi **`10`** (selalu di atas peta).
