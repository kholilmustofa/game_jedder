# Panduan Konfigurasi Boss (Boss Settings)

Panduan ini menjelaskan langkah demi langkah untuk mengonfigurasi dan mengaktifkan objek **Boss** di scene game (seperti Level 2) menggunakan script **`BossController.cs`**.

---

## Fitur Unggulan Boss
1. **Rage Mode (Fase 2)**: Ketika HP Boss turun di bawah 50%, Boss akan mengamuk (berkedip warna ungu, suara raungan), mendapatkan peningkatan kecepatan (+50%), damage serangan bertambah, dan jeda antar serangan (cooldown) menjadi lebih cepat.
2. **Hit Feedback**: Boss berkedip merah saat terkena tembakan Player.
3. **Deteksi Kematian & Kemenangan Otomatis**: Saat Boss dikalahkan (HP = 0), ia akan memutar animasi kematian, mematikan fisiknya agar tidak menghalangi Player, dan secara otomatis memicu layar **Victory/Mission Success** setelah jeda beberapa detik.

---

## Langkah 1: Memasang Script pada Objek Boss
1. Buka scene tempat Boss berada (misalnya **Level 2**).
2. Pilih Game Object bernama **Boss** di jendela Hierarchy.
3. Di jendela Inspector sebelah kanan, klik **`Add Component`** -> cari dan pilih **`Boss Controller`**.
4. Komponen **`Rigidbody 2D`** dan **`Collider 2D`** akan otomatis ditambahkan jika belum ada.
5. Pastikan **Tag** dari Game Object Boss disetel ke **`Enemy`** di bagian atas jendela Inspector. *(Sangat penting agar peluru Player dapat mendeteksi Boss).*

---

## Langkah 2: Mengonfigurasi Animator Controller Boss
1. Pilih objek **Boss**.
2. Pastikan komponen **`Animator`** sudah terpasang.
3. Drag asset Animator Controller bernama **`Boss`** (dari folder `Assets/Prefab/BOSS/`) ke dalam kolom **Controller** di komponen Animator.
4. Parameter nama animasi default pada script sudah disesuaikan dengan aset Anda:
   * **`idleAnimationName`** = `Idle`
   * **`walkAnimationName`** = `walk`
   * **`attackAnimationName`** = `attack`
   * **`deathAnimationName`** = `death`

---

## Langkah 3: Membuat UI Health Bar Boss (Opsional & Direkomendasikan)
Agar pertempuran Boss terasa premium, buatlah slider darah khusus untuk Boss di layar Canvas:
1. Klik kanan pada jendela Hierarchy -> **`UI`** -> **`Slider`** (namai menjadi **`BossHealthBar`**).
2. Posisikan Slider di bagian atas tengah layar. Ubah ukurannya menjadi lebar dan besar agar terlihat mencolok.
3. Hias warna Slider (misalnya warna merah terang untuk area pengisi darah).
4. Pilih kembali objek **Boss** di Hierarchy.
5. Drag objek **`BossHealthBar`** dari Hierarchy ke kolom **`Health Bar Slider`** di script **`Boss Controller`** pada Inspector.
6. Slider ini akan otomatis muncul saat level dimulai dan mengikuti HP Boss secara real-time.

---

## Langkah 4: Menambahkan Efek Suara (Audio Settings)
Anda bisa memasang audio klip untuk memberikan sensasi pertarungan yang seru:
1. Tambahkan komponen **`Audio Source`** pada objek Boss jika ingin memutar efek suara.
2. Masukkan file audio klip yang sesuai ke kolom-kolom berikut di script **`Boss Controller`**:
   * **`Attack Audio Source`**: Bunyi tebasan/pukulan senjata Boss.
   * **`Hit Audio Source`**: Bunyi Boss mengaduh kesakitan saat terkena hit.
   * **`Rage Audio Source`**: Bunyi Boss meraung keras saat masuk ke Fase 2 (Rage Mode).
   * **`Death Audio Source`**: Bunyi erangan terakhir Boss saat kalah.

---

## Langkah 5: Uji Coba Pertarungan Boss
1. Klik **Play** untuk memulai game.
2. Dekati area Boss. Boss akan otomatis berjalan mengejar Anda ketika Anda memasuki radius deteksinya (`10` unit).
3. Tembak Boss menggunakan peluru MC:
   * HP Boss akan berkurang pada slider UI.
   * Boss berkedip merah saat terkena peluru Anda.
4. Ketika darah Boss berkurang hingga mencapai **50%**:
   * Boss akan berkedip warna ungu beberapa kali.
   * Boss berteriak (juga memutar suara raungan jika diisi).
   * Kecepatan gerak Boss meningkat pesat dan serangannya menjadi lebih mematikan.
5. Kurangi darah Boss hingga **0**:
   * Boss akan memutar animasi kematian (`death`).
   * Seluruh collider fisik Boss dinonaktifkan sehingga Player bisa berjalan menembus jasadnya.
   * Jasad Boss akan menghilang perlahan, dan layar kemenangan **Game Success** otomatis muncul!
