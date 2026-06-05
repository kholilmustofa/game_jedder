# Panduan Kamera Mengikuti Karakter (Camera Follow)

Agar kamera otomatis mengikuti gerakan MC Anda di tengah layar saat map membesar, kita membutuhkan sistem **Camera Follow**. 

Ada 2 cara terbaik untuk menerapkannya di Unity. Saya telah membuatkan sebuah script C# siap pakai bernama **[CameraController.cs](file:///d:/Unity/jedder/Assets/Scripts/CameraController.cs)** di proyek Anda. 

Berikut adalah panduan cara menyetel kedua metode tersebut:

---

## Opsi 1: Menggunakan Script Custom (`CameraController.cs`) - *Paling Cepat!*

Ini adalah metode paling ringan karena tidak memerlukan instalasi *package* tambahan dan sudah memiliki fitur gerakan halus (*smooth damping*) serta pembatas peta (*map bounds*).

### Langkah 1: Memasang Script ke Kamera
1. Buka scene gameplay Anda (misalnya scene `lobby` atau `Level1`).
2. Pilih objek **`Main Camera`** di jendela Hierarchy.
3. Seret script **`CameraController.cs`** dari folder `Assets/Scripts/` dan letakkan di jendela Inspector **`Main Camera`** Anda.

### Langkah 2: Hubungkan Target
1. Pilih **`Main Camera`**.
2. Di jendela Inspector pada komponen **CameraController**:
   - Seret objek **`Player`** dari Hierarchy ke kolom **`Target`**.
   - Biarkan nilai **`Smooth Time`** di angka **`0.2`** (ini membuat pergerakan kamera memiliki efek *delay* elastis yang sangat halus).
   - Biarkan **`Offset`** di koordinat **`X: 0, Y: 0, Z: -10`** (Z harus minus agar kamera tidak menabrak objek game).

---

### 🧱 Bonus: Membatasi Kamera Agar Tidak Keluar Map (Map Bounds)
Jika map Anda memiliki batas ujung, kamera bisa memperlihatkan area kosong abu-abu di luar peta jika terus mengikuti Player. Untuk mencegahnya:

1. Di komponen **CameraController** pada **Main Camera**, centang **`Use Bounds`** menjadi True.
2. Geser kamera Anda secara manual di Editor ke **pojok kiri bawah** ujung peta Anda, lalu catat posisi koordinat X dan Y kamera saat itu. Isi angka tersebut ke kolom **`Min Camera Bounds`** (X dan Y).
3. Geser kamera Anda secara manual ke **pojok kanan atas** ujung peta Anda, lalu catat koordinat X dan Y-nya. Isi angka tersebut ke kolom **`Max Camera Bounds`** (X dan Y).
4. Kembalikan kamera ke tengah. Sekarang, kamera Anda tidak akan pernah bisa meluncur keluar batas peta meskipun Player mencoba berjalan keluar peta!

---

## Opsi 2: Menggunakan Paket Resmi "Cinemachine" - *Standar Industri*

Jika Anda menggunakan Unity 6, Unity sudah menyediakan paket kamera canggih bernama **Cinemachine** yang bisa digunakan tanpa menulis kode sama sekali.

### Langkah 1: Membuat Virtual Camera 2D
1. Di jendela Hierarchy, klik kanan pada area kosong -> pilih **2D Object** -> **Cinemachine Camera** (atau **Cinemachine** -> **Create 2D Camera** pada versi Unity lama).
2. Objek baru bernama **`CM vcam1`** atau **`Cinemachine Camera`** akan otomatis terbuat di Hierarchy Anda.
3. Unity otomatis akan mengambil alih kendali **`Main Camera`** utama Anda agar dipandu oleh kamera virtual ini.

### Langkah 2: Mengatur Target Follow
1. Klik objek **`Cinemachine Camera`** di Hierarchy.
2. Di jendela Inspector sebelah kanan, cari kolom **`Follow`**.
3. Seret objek **`Player`** dari Hierarchy ke dalam kolom **`Follow`** tersebut.
4. *(Opsional)* Di bagian **`Lens Settings`**, Anda bisa mengubah ukuran **`Orthographic Size`** untuk memperbesar/memperkecil zoom kamera Anda.

---

### Kesimpulan & Rekomendasi:
* Jika ingin cepat, langsung gunakan **Opsi 1** dengan memasang **[CameraController.cs](file:///d:/Unity/jedder/Assets/Scripts/CameraController.cs)** ke **Main Camera** Anda.
* Jika ingin fitur yang lebih kompleks di masa depan (seperti efek layar bergetar / *screen shake* saat kena tembakan), Anda bisa mencoba menginstal **Cinemachine (Opsi 2)** lewat Window -> Package Manager.
