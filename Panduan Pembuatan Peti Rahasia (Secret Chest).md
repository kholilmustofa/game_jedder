# Panduan Pembuatan Peti Rahasia (Secret Chest)

Panduan ini menjelaskan langkah demi langkah untuk mengkonfigurasi objek **Peti Rahasia (Secret Chest)** di Unity Editor agar dapat dibuka saat didekati dan diklik oleh Player, serta memberikan peningkatan damage peluru.

---

## 🛠️ Langkah 1: Menyiapkan Komponen Fisika pada Peti (`chest`)
Peti rahasia membutuhkan batas fisik (agar tidak ditembus Player) dan area deteksi jangkauan:

1. Pilih objek **`chest`** di Hierarchy Anda.
2. **Tambahkan Collider 1 (Batas Fisik/Dinding):**
   * Klik **Add Component** -> pasang **`Box Collider 2D`**.
   * Jangan centang *Is Trigger*.
   * Sesuaikan ukurannya agar pas mengelilingi kotak peti di kaki (sebagai tembok agar player tidak menembus peti).
3. **Tambahkan Collider 2 (Area Jangkauan Klik):**
   * Klik **Add Component** -> pasang **`Circle Collider 2D`** (atau Box Collider kedua).
   * **Centang kotak `Is Trigger`** pada collider kedua ini.
   * Atur nilai **Radius** menjadi sekitar **`1.5`** atau **`2`** (area lingkaran hijau ini adalah jarak minimal Player harus mendekat agar peti bisa diklik/dibuka).

---

## 🧠 Langkah 2: Memasang Script `SecretChest`
1. Pilih objek **`chest`** di Hierarchy.
2. Klik **Add Component** -> pasang script **`SecretChest`**.
3. Di panel Inspector pada script **`Secret Chest`**, isi kolom-kolomnya sebagai berikut:
   * **Damage Upgrade Amount:** Jumlah peningkatan damage (contoh: **`5`**).
   * **Animator:** Tarik komponen **Animator** dari objek chest itu sendiri ke kolom ini.
   * **Open Sound Audio:** (Opsional) Pasang komponen **Audio Source** pada chest, isi dengan file suara peti terbuka, lalu matikan *Play On Awake*, dan tarik Audio Source tersebut ke kolom ini.
   * **VFX Settings:**
     * **Upgrade Effect Prefab:** Tarik prefab efek visual upgrade pedang merah yang sudah Anda buat di Langkah 4 ke kolom ini.
     * **Spawn Effect On Player:** Centang pilihan ini agar efek visual upgrade muncul tepat mengelilingi tubuh karakter Player (jika tidak dicentang, efek akan muncul di peti).
     * **Effect Destroy Delay:** Waktu tunggu sebelum objek efek dihancurkan (misal: **`1.5`** detik).

---

## ⚔️ Langkah 3: Menghubungkan Tag Player
Agar sensor deteksi peti mengenali karakter Anda:
1. Pilih objek **`Player`** Anda di Hierarchy.
2. Di bagian atas panel Inspector, pastikan **Tag**-nya sudah diset ke **`Player`**.

---

## ✨ Langkah 4: Membuat Prefab Efek Visual Upgrade (VFX)
Karena folder `spell_attack_up_001_large_red` saat ini berisi kumpulan potongan sprite mentah, kita perlu menjadikannya **Prefab Animasi** terlebih dahulu:

1. Di folder **`Assets/Prefab/spell_attack_up_001_large_red/`**, blokir semua file sprite pedang merah `+` yang merupakan bagian dari gerakan animasi tersebut.
2. Seret (drag & drop) kumpulan sprite yang diblokir tersebut secara bersamaan ke dalam jendela **Scene view**.
3. Unity akan memunculkan jendela pop-up untuk membuat file animasi baru. Simpan file animasi tersebut dengan nama **`Anim_AttackUp`**.
4. Secara otomatis di Hierarchy akan muncul objek baru (biasanya bernama sesuai nama file sprite pertama). Ganti nama objek di Hierarchy tersebut menjadi **`VFX_AttackUp`**.
5. Tarik objek **`VFX_AttackUp`** dari Hierarchy ke dalam folder Project Anda (misalnya ke dalam folder `Assets/Prefab/`) untuk menjadikannya **Prefab**.
6. Setelah berhasil menjadi file Prefab di Project view, **hapus** objek `VFX_AttackUp` yang ada di Hierarchy.
7. Sekarang, tarik file Prefab `VFX_AttackUp` tersebut ke kolom **`Upgrade Effect Prefab`** pada script `SecretChest` di peti Anda.

---

## 🎮 Cara Kerja Fitur di Game:
1. Saat Player berjalan mendekati peti dan masuk ke dalam lingkaran hijau (Trigger Collider):
   * Peti mendeteksi kehadiran Player.
2. Player mengarahkan kursor mouse ke peti dan **mengklik kiri** peti tersebut:
   * Peti memutar animasi terbuka (`open`).
   * Damage peluru Player naik secara permanen (misal dari default `10` menjadi `15`).
   * Suara peti terbuka berputar.
   * Peti mengunci dirinya sendiri agar tidak bisa diklik berulang kali.

Jika Anda menembak musuh setelah membuka peti, musuh akan kalah lebih cepat karena damage tembakan Anda sudah ditingkatkan secara permanen!
