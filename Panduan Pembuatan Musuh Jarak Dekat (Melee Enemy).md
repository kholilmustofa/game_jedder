# Panduan Pembuatan Musuh Jarak Dekat (Melee Enemy)

Panduan ini menjelaskan langkah demi langkah untuk merakit musuh jarak dekat (**Warrior**) di Unity Editor menggunakan aset Aseprite dan script **`EnemyMelee.cs`**.

---

## 🛠️ Langkah 1: Menyiapkan GameObject Musuh
1. Masuk ke folder **`Assets/Tiny Swords (Free Pack)/Units/Units (aseprite in Blue only)/`**.
2. **Drag & Drop** file **`Warrior`** (berlogo kubus biru/Aseprite) langsung ke dalam **Hierarchy** atau ke **Scene View**.
3. Klik GameObject **`Warrior`** yang baru dibuat di Hierarchy tersebut, lalu ubah namanya menjadi **`Enemy_Warrior`** (atau nama pilihan Anda).

---

## 🏷️ Langkah 2: Mengatur Tag & Layer
Agar peluru Player bisa mendeteksi musuh, kita harus memberikan Tag "Enemy" pada GameObject musuh:
1. Pilih **`Enemy_Warrior`** di Hierarchy.
2. Di bagian paling atas panel **Inspector**, cari menu **Tag** (di bawah nama objek).
3. Klik menu dropdown Tag, lalu pilih **`Enemy`**.
   * *(Jika Tag `Enemy` belum ada, klik **Add Tag...**, lalu tambahkan tag baru dengan nama `Enemy` (perhatikan huruf besar-kecilnya), lalu kembali ke objek musuh dan pilih tag tersebut).*

---

## 🛑 Langkah 3: Menambahkan Komponen Fisika (Physics 2D)
Agar musuh bisa berjalan, bertabrakan, dan terkena peluru:

### 1. Pasang `Rigidbody 2D`
* Klik **Add Component** pada `Enemy_Warrior` -> pasang **`Rigidbody 2D`**.
* Di panel Inspector pada komponen **Rigidbody 2D**:
  * **Body Type**: Ubah menjadi **`Dynamic`**.
  * **Gravity Scale**: Ubah menjadi **`0`** (karena ini game top-down, kita tidak ingin musuh jatuh ke bawah).
  * **Constraints**: Klik untuk membuka, lalu centang **Freeze Rotation Z** (agar badan musuh tidak berputar saat menabrak sesuatu).

### 2. Pasang `Capsule Collider 2D` (atau `Box Collider 2D`)
* Klik **Add Component** -> pasang **`Capsule Collider 2D`**.
* Atur ukurannya menggunakan tombol **Edit Collider** agar pas dengan kaki/badan musuh.
* Collider ini berfungsi untuk menghalangi musuh menembus tembok.

### 3. Pasang `Box Collider 2D` (Sebagai Sensor Peluru/Trigger)
* Klik **Add Component** -> pasang **`Box Collider 2D`**.
* Centang kotak **`Is Trigger`** pada Box Collider kedua ini.
* Atur ukurannya sedikit lebih besar dari badan musuh. Collider ini berfungsi khusus untuk mendeteksi tabrakan peluru player.

---

## 🧠 Langkah 4: Memasang Script `EnemyMelee`
1. Pilih **`Enemy_Warrior`** di Hierarchy.
2. Klik **Add Component** -> cari dan pasang script **`EnemyMelee`**.
3. Di panel Inspector, isi kolom-kolom script **`EnemyMelee`** sebagai berikut:
   * **Max Health**: Nyawa musuh (contoh: `30`).
   * **Move Speed**: Kecepatan lari mengejar player (contoh: `3`).
   * **Chase Radius**: Jarak deteksi mulai mengejar player (contoh: `8` unit).
   * **Attack Radius**: Jarak serang pedang (contoh: `1.2` unit).
   * **Attack Damage**: Kerusakan yang diterima Player (contoh: `15` HP).
   * **Attack Cooldown**: Jeda waktu antar serangan (contoh: `1.5` detik).
   * **Rb / Animator / Sprite Renderer**: Tarik komponen internal yang ada di objek `Enemy_Warrior` itu sendiri ke kolom-kolom tersebut (atau biarkan kosong karena script akan otomatis mendeteksinya saat game berjalan).

---

## 🎯 Langkah 5: Pastikan Tag Player Sudah Benar
Musuh mencari Player dengan mendeteksi tag `"Player"`.
1. Pilih objek **`Player`** Anda di Hierarchy.
2. Pastikan pada bagian **Tag** (di atas Inspector), terpilih tag **`Player`**.

Sekarang, jalankan game Anda! Musuh akan diam saat Anda jauh, lalu akan mengejar Anda saat mendekat, mengayunkan pedang ketika jaraknya dekat (mengurangi HP Player Anda), dan akan mati jika Anda tembak beberapa kali dengan peluru!
