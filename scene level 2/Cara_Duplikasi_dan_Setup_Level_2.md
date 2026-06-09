# 📑 Panduan Cara Membuat & Setup Scene Level 2 (Duplikasi vs Mulai dari Nol)

Dokumen ini menjelaskan metode terbaik untuk membuat **Level 2** di Unity dengan memanfaatkan asset yang sudah ada dari **Level 1**, serta langkah-langkah detail cara melakukannya tanpa merusak referensi komponen.

---

## ❓ Duplikat Scene vs Mulai dari Nol?

Untuk membuat Level 2 yang memiliki basis gameplay yang sama (pemain yang sama, UI yang sama, kamera yang sama), cara yang **paling benar dan sangat direkomendasikan** adalah **Melakukan Duplikasi (Copy) dari Scene Level 1**.

### 🌟 Mengapa Duplikasi Jauh Lebih Baik?

1. **Efisiensi Waktu:** Anda tidak perlu menyusun ulang Grid Tilemap, Player, Main Camera, Canvas UI, EventSystem, dan pencahayaan global dari nol.
2. **Kestabilan Referensi (No Broken References):** Di Unity, jika Anda menyusun ulang UI dari awal, Anda harus menghubungkan kembali variabel di Inspector satu per satu (misal: menyeret UI Slider ke Script `PlayerHealth`). Duplikasi scene mempertahankan semua hubungan (wiring) antar komponen ini tetap utuh dan aktif.
3. **Konsistensi Gameplay:** Skala ukuran, pengaturan kamera, sistem input, dan fisika akan sama persis dengan Level 1, menjaga kenyamanan pemain.

---

## 🛠️ Langkah-Langkah Membuat Scene Level 2 dengan Benar

Berikut adalah instruksi langkah demi langkah untuk menduplikat dan mengonfigurasi Level 2 di Unity Editor:

### 👣 Langkah 1: Menduplikat Scene Level 1

1. Buka folder **`Assets/Scenes`** di jendela _Project_ Unity Anda.
2. Klik satu kali pada file scene **`Level1`** (ikon logo Unity).
3. Tekan tombol **`Ctrl + D`** (Windows) atau **`Cmd + D`** (Mac) pada keyboard Anda.
4. Unity akan membuat file duplikat bernama **`Level1 1`**.
5. Klik kanan pada file duplikat tersebut, pilih **Rename**, lalu ubah namanya menjadi **`Level2`**.
   _(Unity otomatis membuat file `.meta` baru dengan ID unik agar tidak bentrok)._

---

### 👣 Langkah 2: Mendaftarkan Scene Baru di Build Settings (PENTING!)

Jika scene baru tidak didaftarkan, fungsi perpindahan scene (`SceneManager.LoadScene`) akan memicu error.

1. Klik menu **File** -> **Build Settings...** di bagian atas Unity Editor.
2. Buka folder `Assets/Scenes`, lalu **seret (drag & drop)** file **`Level2`** baru Anda ke dalam kotak daftar **"Scenes In Build"** di bagian atas.
3. Pastikan scene `Level2` berada di daftar tersebut (biasanya mendapatkan indeks nomor baru). Tutup jendela Build Settings.

---

### 👣 Langkah 3: Mengubah Suasana menjadi Mode Gelap (Dark Theme)

Sesuai rancangan game, Level 2 bertema ruangan rahasia yang gelap dan menegangkan:

1. Klik dua kali pada scene **`Level2`** untuk membukanya.
2. Jika menggunakan **URP (Universal Render Pipeline)**:
   - Temukan objek **`Global Light 2D`** di Hierarchy Anda.
   - Ubah warnanya menjadi lebih gelap/biru tua temaram, atau turunkan intensitas cahayanya (_Intensity_) menjadi lebih rendah (misal: **`0.1`** atau **`0.2`**).
3. Tambahkan beberapa lampu lokal kecil (**`Spot Light 2D`** atau **`Point Light 2D`**) di lorong-lorong atau sudut ruangan sebagai pemandu jalan bagi pemain.

---

### 👣 Langkah 4: Mengonfigurasi Peti Level 2 (Medical Kit)

Di Level 2, peti yang diletakkan harus memberikan efek penambahan darah/stamina, bukan damage:

1. Hapus peti bawaan Level 1 yang ada di dalam scene.
2. Tempatkan prefab peti baru di area sudut sebelum memasuki pintu arena Boss.
3. Pasang script khusus peti medis (atau ganti opsi tipe peti di Inspector jika menggunakan script modular) agar saat disentuh MC, ia memanggil fungsi `PlayerHealth.Heal(50)` dan menaikkan batas maksimal HP ke **150**.

---

### 👣 Langkah 5: Menyiapkan Arena & Boss Battle

1. Hapus musuh-musuh biasa (kroco) bawaan Level 1 dari scene Level 2.
2. Buat satu ruangan luas di bagian akhir map sebagai **Arena Boss**.
3. Letakkan **Prefab Boss** di tengah-tengah arena tersebut.
4. _(Opsional)_ Buat pemicu kolisi (_Trigger_) di pintu masuk arena Boss, yang jika dilewati MC, akan memicu penutupan pintu masuk secara otomatis agar MC terkurung di dalam arena bersama Boss hingga pertarungan selesai.
