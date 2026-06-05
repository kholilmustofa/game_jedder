# Panduan Pembuatan UI Health Bar (Slider di Pojok Kiri Atas)

Sesuai dengan konsep desain *Heads-Up Display (HUD)* Anda, kita akan membuat **Health Bar** menggunakan komponen **Slider** bawaan Unity UI. Posisinya akan berada statis di pojok kiri atas layar, memantau nyawa MC secara *real-time*.

Saya telah membuatkan script C# siap pakai bernama **[PlayerHealth.cs](file:///d:/Unity/jedder/Assets/Scripts/PlayerHealth.cs)** di proyek Anda. Script ini sudah dilengkapi dengan:
1. **Sistem HP & Slider Sync** (Otomatis memperbarui tampilan bar merah saat HP berkurang).
2. **Sistem iFrame (Invincibility Frame)** selama 1 detik setelah terkena hit (MC kebal sementara agar tidak mati instan dalam 1 frame).
3. **Efek Kedip Merah (Visual Feedback)** saat MC terkena damage.

Berikut adalah panduan merakit UI Health Bar ini di Unity Editor Anda:

---

## 🎨 LANGKAH 1: Membuat & Menghias UI Slider di Canvas

Buka scene gameplay utama Anda (misalnya scene `lobby` atau `Level1`):

### A. Membuat Objek Slider
1. Di Hierarchy, klik kanan pada **`Canvas`** -> **UI** -> **Slider**.
2. Beri nama objek baru ini: **`HealthSlider`**.

### B. Menghilangkan Bagian Slider yang Tidak Dibutuhkan
Karena bar darah MC ini bersifat *read-only* (hanya dibaca, tidak untuk digeser-geser oleh tangan pemain), kita harus membuang tombol pegangannya:
1. Klik tanda panah kecil di samping **`HealthSlider`** di Hierarchy untuk melihat anak-anak objeknya.
2. **Hapus (Delete)** objek bernama **`Handle Slide Area`**.

---

### C. Menghias Bar Darah Menggunakan Aset "Bars" Tiny Swords
Aset bar darah Anda memiliki masalah yang sama dengan kertas menu pilih level: **`BigBar_Base.png`** aslinya memiliki celah kosong transparan (whitespace) yang sangat lebar di antara bagian kiri, tengah, dan kanannya!

Jika langsung digunakan, bar kayu Anda akan terlihat terpotong dan bolong. Untuk menyelesaikannya secara instan, saya sudah menambahkan fitur penjahit otomatis baru di Unity Anda:

1. **Jahit Rapat Bingkai Bar Darah:**
   - Pergi ke menu atas Unity Editor Anda, klik **`Tools`** -> **`Stitch BigBar Base`**.
   - Klik **Mantap!** saat muncul jendela sukses.
   - Sebuah file gambar baru bernama **`BigBar_Base_Stitched.png`** akan otomatis terbuat di folder `Assets/Tiny Swords (Free Pack)/UI Elements/UI Elements/Bars/`.
   - File baru ini sudah **otomatis dijahit rapat tanpa celah, diset sebagai Sprite, dan garis pembatas 9-Slice (Border) kirinya: 24 dan kanannya: 24 sudah dikonfigurasi otomatis!**

2. **Mengatur Bingkai Bar (Background):**
   - Klik objek **`Background`** di bawah `HealthSlider` di Hierarchy.
   - Di Inspector pada komponen **Image**:
     - Seret sprite baru **`BigBar_Base_Stitched`** dari folder Project ke kolom **Source Image**.
     - Ubah **Image Type** menjadi **`Sliced`** agar bingkai kayu emas memanjang secara proporsional.
3. **Mengatur Isi Darah Merah (Fill):**
   - Klik objek **`Fill`** (yang ada di dalam `Fill Area`) di Hierarchy.
   - Di Inspector pada komponen **Image**:
     - Seret sprite **`BigBar_Fill`** (atau versi sliced-nya) ke kolom **Source Image**.
     - Ubah **Image Type** menjadi **`Tiled`** atau **`Sliced`** (Tiled sangat direkomendasikan untuk bar bertekstur agar tidak melar/distorsi saat darah berkurang).
4. **Merapikan Celah Agar Pas di Dalam Bingkai (Padding):**
   - Karena `BigBar_Base` memiliki bingkai kayu yang tebal, kita harus membuat isi darah merah berada sedikit masuk ke dalam bingkai agar tidak menimpa kayu emasnya:
     - Klik objek **`Fill Area`** di Hierarchy.
     - Di Inspector pada komponen **Rect Transform**, atur pembatas (*offsets*) agar isi darah masuk ke dalam bingkai:
       - **Left:** `16` (menggeser ujung kiri isi darah)
       - **Right:** `16` (menggeser ujung kanan isi darah)
       - **Top:** `6` (memberi celah atas)
       - **Bottom:** `6` (memberi celah bawah)
       *(Sesuaikan angka ini secara manual sampai bar merah Anda pas dan rapi berada di dalam bingkai kayu).*

---

### D. Posisikan Slider ke Pojok Kiri Atas Layar
1. Klik objek utama **`HealthSlider`** di Hierarchy.
2. Di Inspector, klik kotak jangkar (**Anchor Presets**) di pojok kiri atas komponen Rect Transform.
3. Sambil menahan tombol **`Alt`** di keyboard, pilih **Top-Left** (pojok kiri atas).
4. Atur posisinya agar berjarak rapi dari tepi layar, isi koordinat berikut:
   - **Pos X:** `160` (geser ke kanan sedikit)
   - **Pos Y:** `-70` (geser ke bawah sedikit)
   - **Width:** `280` (lebar bar, sesuaikan agar proporsional dengan bingkai kayu)
   - **Height:** `45` (tinggi bar, sesuaikan agar bingkai kayu terlihat jelas)

---

## 🔌 LANGKAH 2: Menghubungkan Script PlayerHealth ke Player

1. **Pasang Script ke Player:**
   - Pilih objek **`Player`** di Hierarchy.
   - Tarik/drag script **`PlayerHealth.cs`** dari folder `Assets/Scripts/` dan letakkan di Inspector objek `Player` tersebut.
2. **Hubungkan Referensi di Inspector:**
   - Klik objek **`Player`**. Pada komponen **Player Health (Script)** baru di Inspector:
     - **`Max Health`**: Isi `100`.
     - **`Invincibility Duration`**: Isi `1` (masa kebal 1 detik).
     - **`Health Slider`**: Seret objek **`HealthSlider`** dari Hierarchy ke kolom ini.
     - **`Sprite Renderer`**: Seret objek **`Player`** itu sendiri ke kolom ini (agar bisa memicu kedip merah).
     - **`Damage Color`**: Ubah warnanya menjadi merah terang atau biarkan default.

---

### 🎉 SELESAI & CARA MENGUJI!
Sekarang jalankan game Anda.
* Darah MC akan langsung terisi penuh (bar merah 100% penuh).
* Jika Anda ingin menguji fungsinya, Anda bisa membuat musuh menabrak player dan memanggil fungsi `TakeDamage` dari script:
  ```csharp
  // Contoh memanggil dari script musuh saat menyentuh Player:
  if (collision.TryGetComponent<PlayerHealth>(out var playerHealth))
  {
      playerHealth.TakeDamage(15); // Mengurangi HP sebanyak 15
  }
  ```
* Saat terkena hit, MC Anda akan berkedip merah secara dramatis selama 1 detik, kebal dari serangan lain selama durasi tersebut, dan bar darah merah di pojok kiri atas Anda akan otomatis berkurang secara sangat mulus!
