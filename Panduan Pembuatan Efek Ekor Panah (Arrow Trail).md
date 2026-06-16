# Panduan Pembuatan Efek Ekor Panah (Arrow Trail Effect)

Panduan ini menjelaskan langkah demi langkah untuk menambahkan efek ekor garis putih (**Trail Renderer**) pada anak panah musuh pemanah di Unity Editor agar terlihat melesat indah di udara.

---

## Cara Kerja di Script

Kita baru saja memperbarui script **`EnemyArrow.cs`**. Jika anak panah menabrak Player, dinding, atau tanah:
1. **`SpriteRenderer`** anak panah langsung dinonaktifkan agar gambar panah fisik langsung hilang.
2. **`TrailRenderer.emitting`** dimatikan agar tidak menghasilkan ekor baru.
3. Fungsi **`Destroy(gameObject, trail.time)`** dipanggil dengan jeda waktu sesuai umur (*lifetime*) trail, sehingga ekor panah yang sudah ada di udara memudar dengan anggun hingga habis sebelum objeknya benar-benar dihapus dari memori.

---

## Langkah 1: Membuka Prefab Anak Panah Musuh

1. Temukan file prefab anak panah musuh di folder **`Assets/Prefab/`** (atau di folder aset Anda). Biasanya bernama **`EnemyArrow`** atau sejenisnya.
2. Klik dua kali file prefab tersebut untuk masuk ke **Prefab Edit Mode**.

---

## Langkah 2: Menambahkan & Mengonfigurasi Komponen Trail Renderer

1. Klik **`Add Component`** di jendela Inspector sebelah kanan -> cari dan pilih **`Trail Renderer`**.
2. Konfigurasikan parameternya sebagai berikut:
   * **`Time` (Umur Ekor)**: Ubah nilainya menjadi sekitar **`0.15`** atau **`0.2`** detik. 
     *(Jeda yang pendek ini membuat ekor terlihat pas mengikuti kecepatan panah yang cepat).*
   * **`Width` (Lebar Ekor)**:
     * Ubah angka lebar default (1.0) menjadi sekitar **`0.1`** atau **`0.15`**.
     * **Tips Lancip**: Klik kanan pada garis merah lebar di parameter Width -> pilih **Add Key** di ujung kanan, lalu tarik ujung kanan garis tersebut ke bawah (ke angka 0). Ini akan membuat ekor panah mengecil secara lancip di ujung belakangnya.
   * **`Color` (Warna Ekor)**:
     * Klik kotak gradasi warna di samping parameter Color.
     * Atur agar ujung kiri berwarna **Putih penuh** (Alpha = 255 atau sekitar 150 agar agak transparan), dan ujung kanan berwarna **Putih transparan** (Alpha = 0). Ini membuat ekor memudar indah ke udara.
   * **`Material` (Bahan Garis)**:
     * Secara default materialnya kosong (*None/Default-Line*).
     * Klik lingkaran kecil di kanan kolom Material -> cari dan pilih **`Sprites-Default`** atau **`Default-Line`** agar garisnya berwarna putih bersih.
   * **`Sorting Layer` & `Order in Layer`**:
     * Samakan dengan sorting layer anak panah (misal: *Default*).
     * Atur **Order in Layer** menjadi sedikit lebih kecil dari anak panah (misalnya jika anak panah Order = 20, setel Trail Order = **`19`**) agar ekornya digambar tepat di belakang kepala anak panah.

---

## Langkah 3: Melakukan Uji Coba (Testing)

1. Keluar dari Prefab Edit Mode (klik tanda panah kiri `<` di Hierarchy).
2. Jalankan game (klik **Play**).
3. Biarkan musuh pemanah menembaki Anda atau meleset ke tanah.
4. **Hasil yang Diharapkan**:
   * Anak panah meluncur dengan garis ekor putih mengikuti di belakangnya.
   * Saat panah menabrak Anda atau tanah, panah fisik langsung menghilang dan ekor putihnya memudar perlahan selama 0.15 detik sebelum hilang total secara mulus.

---

## Catatan Penting: Rigidbody 2D untuk Stabilitas Deteksi Damage

Untuk memastikan deteksi tabrakan anak panah ke Player selalu stabil dan tidak pernah "menembus" atau "lolos" tanpa memberikan damage (terutama pada tembakan berturut-turut):
1. **Penyebab Masalah**: Jika anak panah tidak memiliki komponen **Rigidbody 2D**, Unity menganggapnya sebagai *Static Collider*. Ketika *Static Collider* digerakkan langsung via script (`transform.position`), deteksi tabrakannya menjadi tidak stabil dan bisa lolos/tembus (terutama jika frame rate tidak stabil atau panah bergerak cepat).
2. **Solusi Otomatis**: Kami telah memperbarui script [EnemyArrow.cs](file:///d:/Unity/jedder/Assets/Scripts/EnemyArrow.cs). Script ini sekarang secara otomatis mendeteksi dan menambahkan komponen **Rigidbody 2D** pada anak panah saat game berjalan (Runtime), lalu mengaturnya ke:
   * **`Body Type`** = **`Kinematic`** (agar pergerakan parabola dari script tidak dipengaruhi gaya gravitasi Unity).
   * **`Collision Detection`** = **`Continuous`** (untuk mencegah anak panah "menembus" objek/Player tanpa memicu *trigger*).
3. **Langkah Manual (Opsional & Direkomendasikan)**:
   * Buka kembali Prefab **`EnemyArrow`**.
   * Klik **`Add Component`** -> cari **`Rigidbody 2D`**.
   * Ubah **`Body Type`** menjadi **`Kinematic`**.
   * Ubah **`Collision Detection`** menjadi **`Continuous`**.
   * *Langkah ini membuat prefab Anda lebih rapi di Unity Editor.*
