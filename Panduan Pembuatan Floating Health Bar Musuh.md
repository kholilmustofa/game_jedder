# Panduan Pembuatan Floating Health Bar Musuh (UI Slider Melayang)

Panduan ini menjelaskan cara membuat bar nyawa merah tipis di atas kepala musuh menggunakan komponen **Canvas (World Space)** dan **UI Slider** di Unity.

---

## 🛠️ Langkah 1: Membuat World Space Canvas pada Musuh
1. Pilih GameObject musuh Anda (misalnya `Enemy_Warrior` atau `Enemy_ArcherRed`) di Hierarchy.
2. Klik kanan pada musuh tersebut -> **UI** -> **Canvas**.
   * *Akan terbuat objek anak Canvas di bawah musuh.*
3. Pilih objek **Canvas** tersebut, lalu atur di panel **Inspector**:
   * **Static**: **Matikan/Uncheck** (PENTING! Jangan centang Static di pojok kanan atas karena musuh ini bergerak).
   * **Render Mode**: Ubah dari *Screen Space - Overlay* menjadi **`World Space`**.
   * **Rect Transform**:
     * **Width**: **`100`** (Ini adalah lebar dalam piksel UI, setara 1 unit di dunia game jika dikali skala 0.01).
     * **Height**: **`15`** (Tinggi bar nyawa).
     * **Scale X / Y / Z**: Ubah menjadi **`0.01`** (PENTING! Skala ini akan mengecilkan 100 piksel Canvas menjadi 1 unit di dunia game agar pas dengan ukuran tubuh musuh).
     * **Pos X / Y / Z**: Atur Pos X = `0`, Pos Y = `1.2` (untuk menaruhnya tepat di atas kepala musuh), Pos Z = `0`.
   * **Canvas Scaler**: Biarkan bawaan (`Dynamic Pixels Per Unit` = `1`).
   * **Graphic Raycaster**: Klik kanan komponen ini dan pilih **Remove Component** (tidak dibutuhkan karena musuh tidak perlu menerima klik mouse pada UI nyawanya).

---

## 📊 Langkah 2: Membuat Slider Nyawa Merah
1. Klik kanan pada objek **Canvas** anak musuh tersebut di Hierarchy -> **UI** -> **Slider**.
   * *Akan terbuat objek Slider di bawah Canvas.*
2. Klik kanan komponen **Slider** tersebut di Hierarchy, lalu **Unpack Prefab** jika perlu (atau langsung edit saja).
3. Hapus objek anak yang tidak diperlukan:
   * Klik kanan pada **`Handle Slide Area`** -> pilih **Delete**.
4. Pilih objek **`Slider`** di Hierarchy, lalu atur di Inspector:
   * **Rect Transform**:
     * Set **Left, Top, Right, Bottom** semuanya menjadi **`0`** (klik Anchor Presets di kiri atas Rect Transform, tahan tombol **Alt**, lalu pilih kotak kanan bawah yang bertuliskan **stretch**). Ini bertujuan agar Slider membesar memenuhi seluruh area Canvas (100x15).
   * **Slider Component**:
     * **Transition**: Ubah menjadi **`None`**.
     * **Navigation**: Ubah menjadi **`None`**.
     * **Interactable**: **Matikan/Uncheck** (agar bar nyawa tidak bisa digeser manual oleh Player).

---

## 🎨 Langkah 3: Mengubah Warna Menjadi Merah Tipis
1. Di bawah objek `Slider`, buka objek anak **`Background`**:
   * Pada komponen **Image**, ubah warna (**Color**) menjadi **Abu-abu gelap** atau **Hitam transparan** (sebagai latar belakang bar kosong).
2. Buka objek anak **`Fill Area`** -> pilih **`Fill`**:
   * Pada komponen **Image**, ubah warna (**Color**) menjadi **Merah Terang** (sebagai indikator nyawa).
3. Pilih objek **`Fill Area`** di Hierarchy:
   * Di Inspector, atur **Rect Transform** -> **Left** = `0` dan **Right** = `0` (agar warna merah bisa terisi penuh dari ujung ke ujung).

---

## 🔌 Langkah 4: Menghubungkan Slider ke Script Musuh
1. Pilih objek musuh utama (`Enemy_Warrior` atau `Enemy_ArcherRed`) di Hierarchy.
2. Lihat ke panel **Inspector** pada script musuh (**`EnemyMelee`** atau **`EnemyRanged`**).
3. Pada bagian **UI Settings**, Anda akan melihat kolom baru bernama **`Health Bar Slider`**.
4. **Drag & Drop (seret)** objek **`Slider`** yang baru saja Anda buat ke kolom **`Health Bar Slider`** tersebut.

Selesai! Sekarang ketika Anda memainkan game dan menembak musuh tersebut, bar merah di atas kepalanya akan berkurang secara otomatis mengikuti sisa HP musuh!

> 💡 **Tips Tambahan (Optimasi)**:
> Jika Anda sudah berhasil membuat satu bar nyawa ini berjalan lancar di satu musuh, jadikan musuh tersebut sebagai **Prefab** (seret kembali dari Hierarchy ke Project view). Dengan begitu, semua musuh baru yang Anda taruh di peta akan otomatis memiliki bar nyawa melayang ini tanpa perlu merakitnya berulang-ulang!
