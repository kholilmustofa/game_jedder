# Panduan Pembuatan Fitur Tenggelam di Air (Water Drowning & Splash Effect)

Panduan ini menjelaskan langkah demi langkah untuk mengonfigurasi area air pada **Tilemap** agar bisa mendeteksi ketika Player (MC) melangkah masuk ke dalam air, memutar efek cipratan air (**WaterSplash**), dan memicu Game Over.

---

## Cara Kerja Sistem

1. **Trigger Collider**: Area air menggunakan `Tilemap Collider 2D` yang disetel sebagai Trigger agar Player bisa memasukinya tanpa terbentur dinding fisik.
2. **Tag Detection**: Area air diberi Tag khusus bernama `"Water"`.
3. **Drown Sequence**: Begitu Player menyentuh area ber-tag `"Water"`, script `PlayerController.cs` akan:
   * Menghentikan kecepatan fisik (`linearVelocity = Vector2.zero`).
   * Menonaktifkan kontrol input Player.
   * Menyembunyikan tampilan sprite MC (`spriteRenderer.enabled = false`) agar terlihat benar-benar tenggelam masuk ke air.
   * Memunculkan (**Instantiate**) prefab efek cipratan air (`WaterSplash`) tepat di koordinat posisi Player berdiri.
   * Mengirimkan damage sangat besar (`999`) ke script `PlayerHealth` untuk memicu kematian dan layar Game Over.

---

## Langkah 1: Menyiapkan Tag "Water" di Unity

1. Klik salah satu objek apa saja di **Hierarchy** (misalnya `TilemapWater`).
2. Di jendela **Inspector**, klik menu dropdown **Tag** (di bagian paling atas kiri) -> pilih **Add Tag...**.
3. Di jendela *Tags & Layers*, klik ikon **`+`** (plus) di bawah kolom *Tags*.
4. Masukkan nama tag baru: **`Water`** (pastikan huruf W besar). Klik **Save**.

---

## Langkah 2: Mengonfigurasi Tilemap Air (`TilemapWater`)

1. Cari objek **`TilemapWater`** (di bawah objek `Grid`) pada jendela Hierarchy Anda.
2. Pilih objek tersebut, lalu lihat ke jendela **Inspector**.
3. Ubah **Tag** objek tersebut dari *Untagged* menjadi **`Water`**.
4. Klik tombol **`Add Component`** di bagian bawah Inspector -> cari dan tambahkan **`Tilemap Collider 2D`**.
5. Pada komponen **Tilemap Collider 2D** yang baru ditambahkan, beri centang pada kotak pilihan **`Is Trigger`**.

---

## Langkah 3: Menghubungkan Prefab WaterSplash & Mengatur Offset Posisi

1. Pilih objek **`Player`** di Hierarchy Anda.
2. Di jendela **Inspector**, cari komponen **Player Controller (Script)**.
3. Sekarang ada dua field baru:
   * **`Water Splash Prefab`**: Buka folder `Assets/Tiny Swords (Free Pack)/Particle FX/` di jendela Project. Seret (`drag & drop`) file prefab `WaterSplash` dari folder tersebut ke kolom ini.
   * **`Water Splash Offset`**: Kolom ini digunakan untuk mengatur letak visual cipratan air.
     * Jika letak cipratan air dirasa kurang pas (terlalu ke atas atau ke bawah dibanding tubuh Player), Anda bisa mengubah nilai **`Y`** (misalnya menjadi `-0.5` atau `-1`) agar cipratan air tepat berada di kaki/pusat tenggelam Player.

---

## 💡 Tips Profesional Game Dev: Mengatur Collider Kaki Player

Agar Player tidak langsung tenggelam hanya karena berdiri di pinggir pantai (rumput yang berbatasan dengan air), atur collider fisik player agar fokus di bagian kaki saja:

1. Pilih objek **`Player`** di Hierarchy.
2. Cari komponen collider 2D miliknya (biasanya **Capsule Collider 2D** atau **Circle Collider 2D**).
3. Klik tombol **`Edit Collider`** di Inspector.
4. Kecilkan ukuran lingkar collider tersebut dan letakkan **hanya di area kaki/alas kaki Player** (tidak mencakup seluruh badan).
5. Dengan begini, Player baru akan dianggap tenggelam saat kakinya benar-benar menyentuh air, bukan saat kepala atau badannya menyenggol batas air!

---

## Langkah 4: Melakukan Uji Coba (Testing)

1. Tekan tombol **Play** di Unity Editor.
2. Jalankan karakter Player ke arah air (tengah map yang ada bebek-bebek berenang).
3. **Hasil yang Diharapkan**:
   * Begitu menyentuh air, Player langsung berhenti bergerak.
   * Badan Player menghilang seketika.
   * Efek cipratan air (`WaterSplash`) terputar di posisi tersebut.
   * Layar Game Over muncul setelah 1.5 detik.
