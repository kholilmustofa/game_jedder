# 🎮 Game Jedder - 2D Top-Down Action Adventure

Selamat datang di **Jedder**, sebuah game 2D Top-Down Action Adventure seru yang dibangun menggunakan **Unity 6**. Pemain akan mengendalikan karakter utama (MC) untuk menjelajahi area, memecahkan teka-teki peti rahasia, menghadapi musuh jarak dekat dan pemanah, hingga menantang Boss raksasa di akhir level!

---

## 🌟 Fitur Utama Game

### 1. Sistem Karakter Utama (MC) & Combat
* **Pergerakan Dinamis**: Kendali pergerakan menggunakan WASD yang responsif.
* **Bidikan Kursor**: MC menembak menggunakan bidikan kursor mouse (Crosshair) secara real-time.
* **Upgrade Permanen (PlayerPrefs)**: Data upgrade damage peluru yang didapatkan dari Level 1 otomatis ditransfer ke Level 2.

### 2. Peti Rahasia Modular (Secret Chest)
Peti interaktif serbaguna yang dapat dikonfigurasi langsung di Inspector untuk tipe:
* **Damage Upgrade (Level 1)**: Memberikan bonus kekuatan tembakan peluru sebesar +5 Damage secara permanen.
* **Health Upgrade (Level 2)**: Meningkatkan batas kapasitas darah maksimum Player dari 100 menjadi 150 HP, memulihkan darah hingga penuh, serta memunculkan efek partikel penyembuhan hijau (`HealEffect`).

### 3. Ragam Musuh & Kecerdasan Buatan (AI)
* **Musuh Jarak Dekat (Melee Enemy)**: Mengejar Player dan melancarkan tebasan pedang.
* **Musuh Jarak Jauh (Ranged Archer)**: Menembakkan anak panah parabola dengan perhitungan prediksi posisi Player.
* **Ekor Garis Panah (Arrow Trail)**: Efek garis ekor putih mengikuti lesatan panah musuh dan memudar anggun saat membentur target.
* **Stabilitas Fisika**: Dilengkapi dengan Rigidbody 2D Kinematic & Continuous Collision Detection untuk menjamin akurasi deteksi damage.
* **Optimasi Kematian**: Semua musuh otomatis berhenti menyerang dan masuk ke mode diam (Idle) ketika Player mati.

### 4. Boss Epic & Rage Mode (Fase 2)
* Pertarungan Boss raksasa dengan sistem dua fase bertarung.
* Ketika darah Boss berada di bawah **50%**, Boss akan masuk ke **Fase Rage**:
  * Tubuh Boss memerah dan berkedip ungu.
  * Kecepatan lari Boss meningkat sebesar **+50%**.
  * Jeda menyerang (cooldown) berkurang **30%** (menyerang lebih cepat).
  * Damage serangan bertambah sebesar **+10 Damage**.
* Kematian Boss otomatis memicu layar kemenangan secara anggun.

### 5. Bahaya Lingkungan & Drowning System
* Kolam air di dalam level bertindak sebagai rintangan mematikan.
* Player yang melangkah masuk ke air akan tenggelam secara visual, memunculkan efek cipratan air (`WaterSplash VFX`), dan memicu Game Over secara instan.

### 6. Antarmuka UI Premium & Sistem Audio Terpadu
* **Pop-up Misi Awal Level**: Layar misi otomatis muncul di awal level, menjeda game, dan memutarkan musik tema misi khusus sebelum masuk ke BGM utama.
* **Panel UI Animatif**: Panel Pause, Game Over, dan Victory menggunakan transisi animasi masuk dan keluar (menggunakan *Unscaled Time* agar animasi tetap berjalan saat game dijeda).
* **Sound Effect (SFX) Lengkap**: Terintegrasi dengan efek suara tembakan, tebasan, raungan kemarahan boss, suara terkena hit, lagu kemenangan, dan lagu kekalahan.

---

## 🛠️ Teknologi & Tools
* **Game Engine**: Unity 6 (Versi 6000.4.1f1)
* **Bahasa Pemrograman**: C# (C-Sharp)
* **Render Pipeline**: Universal Render Pipeline (URP) untuk visual 2D premium.
* **Input System**: Unity New Input System.
* **Text Rendering**: TextMesh Pro (TMP).

---

## 📂 Dokumen Instruksi & Panduan Developer
Seluruh panduan teknis cara perakitan komponen dan penjelasan detail script telah dikelompokkan dengan rapi di dalam folder [Instruction/](file:///d:/Unity/game_jedder/Instruction/). 

Kumpulan file Markdown (`.md`) di dalam folder tersebut merupakan **file instruksi acuan saya sebagai developer** dalam merancang, mengatur, dan mengembangkan setiap fitur mekanik game ini di Unity Editor secara teratur.

---

## 🎮 Cara Bermain (Controls)
* **Bergerak**: Tombol `W` `A` `S` `D`
* **Membidik**: Arahkan kursor mouse
* **Menembak**: Klik `Kiri Mouse`
* **Jeda / Pause / Tutup Misi**: Tombol `ESC` atau tombol UI di layar.

---

## 🚀 Cara Menjalankan Proyek
1. Clone repositori ini ke komputer lokal Anda:
   ```bash
   git clone https://github.com/kholilmustofa/game_jedder.git
   ```
2. Buka aplikasi **Unity Hub** -> Klik **Add** -> Pilih folder hasil clone tersebut.
3. Pastikan Anda menggunakan versi editor **Unity 6 (6000.4.1f1)** atau versi di atasnya yang kompatibel.
4. Buka scene awal di folder `Assets/Scenes/lobby` atau `LevelSelect`, tekan tombol **Play**, dan mulailah berpetualang!
