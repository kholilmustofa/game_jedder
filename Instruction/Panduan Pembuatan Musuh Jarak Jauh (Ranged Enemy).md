# Panduan Pembuatan Musuh Jarak Jauh (Ranged Enemy - Archer)

Panduan ini menjelaskan langkah demi langkah untuk merakit musuh memanah (**Archer**) di Unity Editor menggunakan aset spritesheet merah dan script **`EnemyRanged.cs`** serta **`EnemyArrow.cs`**.

---

## 🛠️ Langkah 1: Menyiapkan GameObject Musuh
1. Masuk ke folder **`Assets/Tiny Swords (Free Pack)/Units/Red Units/`** di Project view.
2. Karena Anda sudah membuat animasi lari/diam untuk **Archer Red** tadi, seret objek **`Enemy_ArcherRed`** dari folder prefab/Project ke dalam **Hierarchy** atau ke **Scene View** Anda.

---

## 🏷️ Langkah 2: Mengatur Tag & Layer
Agar peluru Player bisa mendeteksi musuh, berikan Tag pada musuh:
1. Pilih **`Enemy_ArcherRed`** di Hierarchy.
2. Di bagian paling atas panel **Inspector**, ubah **Tag** menjadi **`Enemy`**.

---

## 🛑 Langkah 3: Menambahkan Komponen Fisika (Physics 2D)
Agar musuh bisa berjalan, mendeteksi tabrakan, dan terkena peluru:

### 1. Pasang `Rigidbody 2D`
* Klik **Add Component** pada `Enemy_ArcherRed` -> pasang **`Rigidbody 2D`**.
* Di panel Inspector pada komponen **Rigidbody 2D**:
  * **Body Type**: **`Dynamic`**.
  * **Gravity Scale**: **`0`** (karena ini game top-down).
  * **Constraints**: Centang **`Freeze Rotation Z`** (agar badan musuh tidak berputar saat menabrak sesuatu).

### 2. Pasang `Capsule Collider 2D` (Batas Fisik Kaki)
* Klik **Add Component** -> pasang **`Capsule Collider 2D`**.
* Gunakan **Edit Collider** untuk mengecilkan ukurannya agar hanya melingkari area kaki musuh saja (agar bisa berjalan di belakang tebing/pohon dengan rapi).

### 3. Pasang `Box Collider 2D` (Sebagai Pemicu Hit/Trigger)
* Klik **Add Component** -> pasang **`Box Collider 2D`**.
* Centang kotak **`Is Trigger`** pada Box Collider kedua ini.
* Atur ukurannya agar sedikit lebih besar dari badan musuh. Collider ini berfungsi khusus untuk mendeteksi tabrakan dari peluru player.

---

## 🏹 Langkah 4: Membuat Titik Tembak (Fire Point)
Kita butuh titik keluar anak panah pada tubuh musuh:
1. Klik kanan pada objek **`Enemy_ArcherRed`** di Hierarchy -> **Create Empty**.
2. Beri nama objek anak ini: **`FirePoint`**.
3. Di Scene view, geser posisi `FirePoint` agar berada sedikit di depan tubuh atau tangan Archer Anda.

---

## 🚀 Langkah 5: Membuat Prefab Anak Panah Musuh (EnemyArrow)
Anak panah musuh harus bisa melukai Player:

1. Di folder **`Red Units`** -> **`Archer`**, cari gambar **`Arrow`** (atau spritesheet anak panah).
2. Seret gambar **`Arrow`** tersebut ke dalam Scene view.
3. Beri nama objek ini: **`EnemyArrow`**.
4. Klik **Add Component** pada `EnemyArrow` -> cari dan pasang script **`EnemyArrow`**.
5. Klik **Add Component** -> pasang **`Box Collider 2D`**.
   * Centang kotak **`Is Trigger`**.
6. Jadikan **Prefab**: Seret objek **`EnemyArrow`** dari Hierarchy ke dalam folder **`Assets/Prefab/`** di Project view Anda.
7. **Hapus** objek `EnemyArrow` yang ada di Hierarchy (karena kita hanya butuh file prefab-nya saja untuk ditembakkan).

---

## 🧠 Langkah 6: Memasang Script `EnemyRanged` pada Musuh
1. Pilih **`Enemy_ArcherRed`** di Hierarchy.
2. Klik **Add Component** -> cari dan pasang script **`EnemyRanged`**.
3. Di panel Inspector, isi kolom-kolom script **`EnemyRanged`** sebagai berikut:
   * **Max Health**: Nyawa musuh (contoh: `25`).
   * **Move Speed**: Kecepatan lari mengejar player (contoh: `2.5`).
   * **Chase Radius**: Jarak deteksi mulai mengejar player (contoh: `8` unit).
   * **Attack Radius**: Jarak tembak anak panah (contoh: `5` unit. Archer akan berhenti mengejar jika sudah sedekat ini dan langsung menembak).
   * **Attack Cooldown**: Jeda waktu antar tembakan (contoh: `2` detik).
   * **Arrow Prefab**: Seret file prefab **`EnemyArrow`** yang sudah Anda buat di Langkah 5 ke kolom ini.
   * **Fire Point**: Seret objek anak **`FirePoint`** dari objek Archer ke kolom ini.
   * **Rb / Animator / Sprite Renderer**: Biarkan kosong karena script akan mendeteksinya secara otomatis saat bermain.

---

Sekarang musuh Archer Anda siap diuji! Musuh akan menembakkan anak panah ke arah Anda secara dinamis dari jarak jauh dan akan otomatis mati jika terkena tembakan Anda beberapa kali!
