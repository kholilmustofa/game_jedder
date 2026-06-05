# Panduan Membuat Peluru Animasi Sci-Fi (Bullet & Explosion)

Aset proyektil energi merah Anda (**`scifi_warp_002_small_red`**) **sangat luar biasa!** Aset ini memiliki urutan frame dari peluru kecil melayang (`frame0000` - `frame0001`) hingga efek ledakan energi pecah (`frame0002` - `frame0009`).

Untuk membuat game Anda terasa sangat memuaskan (*juicy*), saya baru saja memperbarui script **[Bullet.cs](file:///d:/Unity/jedder/Assets/Scripts/Bullet.cs)** Anda agar secara otomatis:
1. Meluncur lurus dengan animasi berdenyut.
2. Saat menabrak musuh atau tembok, peluru **berhenti bergerak**, **colider dinonaktifkan**, dan **animasi ledakan energi langsung diputar**.
3. Objek peluru hancur otomatis setelah animasi ledakan selesai.

Berikut adalah panduan lengkap cara merakitnya di Unity Editor Anda:

---

## ✂️ LANGKAH 1: Memotong Spritesheet Peluru

1. Di Project Window, cari folder tempat gambar tersebut berada:
   `Assets > Map > scifi_warp_002_small_red`
2. Pilih file gambar utama proyektil merah tersebut.
3. Di jendela **Inspector** sebelah kanan, atur importnya:
   - **Texture Type:** `Sprite (2D and UI)`
   - **Sprite Mode:** `Multiple`
   - **Filter Mode:** **`Point (no filter)`** *(Wajib agar pixel art-nya tetap tajam)*
   - **Compression:** `None`
   - Klik **Apply** di bagian bawah.
4. **Memotong Gambar (Sprite Editor):**
   - Klik tombol **Sprite Editor** di Inspector.
   - Klik menu dropdown **Slice** di bagian atas jendela Sprite Editor.
   - Pilih **Type:** **`Grid By Cell Count`** (Karena gambar berjajar rapi 1 baris berisi 10 frame).
   - Atur **Columns (Kolom):** **`10`** dan **Rows (Baris):** **`1`**.
   - Klik **Slice**, lalu klik **Apply** di pojok kanan atas, lalu tutup jendela Sprite Editor.
   *(Sekarang file gambar Anda sudah terpotong rapi dari `frame0000` sampai `frame0009`!)*

---

## 🏗️ LANGKAH 2: Merakit GameObject Peluru (Bullet)

1. Tarik sprite **`frame0000`** dari Project window langsung ke jendela Hierarchy. Beri nama objek baru ini **`RedBullet`**.
2. **Tambahkan Komponen yang Dibutuhkan:**
   Pilih `RedBullet` di Hierarchy, klik **Add Component** di Inspector untuk menambahkan komponen berikut:
   * **`Rigidbody 2D`**:
     - **Body Type:** `Kinematic` (Karena peluru kita digerakkan secara presisi lewat kode).
     - **Collision Detection:** `Continuous`
   * **`Circle Collider 2D`** (Untuk mendeteksi tabrakan):
     - **Is Trigger:** **`Centang (True)`** *(SANGAT PENTING! Agar peluru mendeteksi tabrakan tanpa mendorong fisik Player/Musuh).*
     - Atur ukuran radiusnya agar pas melingkari bagian inti bola energi merah (`frame0000`).
   * **`Animator`** (Untuk memutar animasi terbang & meledak).
   * **`Bullet`** (Script C# peluru Anda yang sudah saya perbarui).
3. **Hubungkan Referensi di Script `Bullet` (Inspector):**
   - **`Speed`**: Isi `15` (kecepatan luncur).
   - **`Damage`**: Isi `10` (sesuai catatan statistik Anda).
   - **`Life Time`**: Isi `5`.
   - **`Animator`**: Seret objek **`RedBullet`** itu sendiri ke kolom ini.
   - **`Bullet Collider`**: Seret objek **`RedBullet`** itu sendiri ke kolom ini.
   - **`Destroy Delay After Explosion`**: Isi **`0.4`** (waktu tunggu agar animasi ledakannya selesai diputar sebelum hancur).

---

## 🎬 LANGKAH 3: Membuat Animasi Terbang & Ledakan

Buka jendela **Animation** (Menu *Window* -> *Animation* -> *Animation*).

### 1. Buat Animasi Terbang (`Bullet_Fly`):
1. Pilih **`RedBullet`** di Hierarchy, lalu klik **Create** di jendela Animation.
2. Simpan klip dengan nama **`Bullet_Fly`**.
3. Seret sprite **`frame0000`** dan **`frame0001`** (bola energi menyala) dari Project window ke timeline jendela Animation.
4. *Animasi terbang ini akan memutar bola energi merah yang berdenyut secara berulang-ulang (loop) saat meluncur di udara.*

### 2. Buat Animasi Ledakan (`Bullet_Explode`):
1. Klik dropdown nama animasi di pojok kiri atas jendela Animation -> pilih **Create New Clip**.
2. Simpan dengan nama **`Bullet_Explode`**.
3. Seret sprite **`frame0002`** sampai **`frame0009`** (efek energi pecah/meledak) ke dalam timeline.
4. **Matikan Loop ledakan:**
   - Cari file animasi **`Bullet_Explode`** yang baru terbuat di folder Project Anda.
   - Klik file tersebut, lalu di Inspector **hilangkan centang** pada pilihan **`Loop Time`** *(PENTING! Agar ledakan hanya meledak sekali saja dan tidak meledak terus-menerus).*

---

## 🎛️ LANGKAH 4: Mengatur Animator Controller Peluru

1. Klik dua kali file **Animator Controller** peluru Anda di Project window untuk membuka jendela **Animator**.
2. **Buat Parameter:**
   - Di tab **Parameters** (kiri atas), klik **`+`** -> pilih **Trigger**. Beri nama **`Explode`** (Harus sama persis huruf besar-kecilnya dengan yang di script!).
3. **Mengatur Transisi (`Bullet_Fly` -> `Bullet_Explode`):**
   - Klik kanan pada state **`Bullet_Fly`** -> **Make Transition** -> arahkan ke **`Bullet_Explode`**.
   - Klik garis panah transisi tersebut, lalu di Inspector sebelah kanan:
     - Hilangkan centang **`Has Exit Time`**.
     - Ubah **`Transition Duration`** menjadi **`0`**.
     - Di bagian **Conditions**, klik **`+`** dan pilih parameter **`Explode`**.

---

## 📦 LANGKAH 5: Menjadikan Prefab & Menghubungkan ke MC

1. Tarik objek **`RedBullet`** dari jendela Hierarchy ke dalam folder **`Assets > Prefab`** di Project window untuk menjadikannya **Prefab**.
2. **Hapus** objek `RedBullet` yang ada di Hierarchy (karena kita hanya butuh template Prefab-nya saja).
3. Buka scene **gameplay** Anda (misalnya scene `lobby`).
4. Pilih objek **`Player`** (MC) di Hierarchy.
5. Pada komponen script **`PlayerController`** di Inspector, seret file prefab **`RedBullet`** yang ada di folder Project ke dalam kolom **`Bullet Prefab`**.

---

### 🎉 SELESAI!
Sekarang saat Anda menembak di dalam game:
1. Senjata MC akan menembakkan bola energi merah berdenyut indah yang meluncur cepat.
2. Ketika menabrak musuh atau dinding, peluru akan berhenti meluncur secara instan, kehilangan kemampuan menabrak, lalu **berubah menjadi ledakan percikan energi sci-fi yang sangat memuaskan** sebelum akhirnya hancur bersih dari layar!
