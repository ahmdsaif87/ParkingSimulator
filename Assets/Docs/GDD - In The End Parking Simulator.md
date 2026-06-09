# **In The End: Parking Simulator \- Game Design Document**

## **Game Overview**

### **Spesifikasi Game**

* **Judul Game:** In The End: Parking Simulator  
* **Genre:** Simulation, Driving  
* **Tema:** Parkir Kendaraan, Tantangan Presisi  
* **Target Pengguna:** Usia 12-40 tahun, pecinta game simulasi dan driving  
* **Platform:** PC  
* **Waktu Pengembangan:** \-

### **Tim Pengembang:**

**Game Designer:**

1. Ahmad Saifi Khayatu Ulumuddin  

**Artist:**

1. Ahmad Saifi Khayatu Ulumuddin  

**Game Programmer:**

1. Ahmad Saifi Khayatu Ulumuddin  

**Sound Engineer:**

1. Ahmad Saifi Khayatu Ulumuddin  

**Produser/Manajer Produk:**

1. Ahmad Saifi Khayatu Ulumuddin  
1. 

---

## **Background**

**In The End: Parking Simulator** dibuat sebagai game simulasi parkir yang menguji kemampuan pemain dalam mengendalikan kendaraan untuk parkir di area yang ditentukan dengan presisi tinggi. Game ini terinspirasi dari game parkir simulator populer seperti **Dr. Driving** yang rilis pada tahun 2013 dan sangat eksis pada masanya. Melalui game ini, pemain dapat merasakan pengalaman dan tantangan memarkir mobil di berbagai situasi, mulai dari area parkir statis hingga area parkir dengan rintangan bergerak yang semakin menantang.

---

## **Pitch**

**In The End: Parking Simulator** adalah game simulasi parkir 3D yang menghadirkan pengalaman memarkir mobil dengan tingkat kesulitan bertahap. Pemain harus mengarahkan mobil ke zona parkir yang ditentukan tanpa menabrak rintangan, dengan Level 1 berupa area parkir statis dan Level 2 menambahkan tantangan mobil bot yang bergerak di area parkir.

---

## **Core Experience**

Pemain harus merasakan sensasi ketegangan dan kepuasan ketika berhasil memarkir mobil dengan presisi. Pemain akan mengendalikan mobil dari sudut pandang orang ketiga, menavigasi melalui rintangan-rintangan seperti pohon, tiang lampu, bangunan, dan di Level 2, mobil bot yang bergerak secara dinamis. Game ini menawarkan pengalaman yang menantang namun santai, di mana pemain harus fokus pada kontrol kendaraan dan timing yang tepat.

---

## **Game Core Direction**

Arah utama pengembangan game adalah menghadirkan pengalaman simulasi parkir yang realistis dan menantang. Setiap elemen game harus menyatu dengan tema simulasi mengemudi dan parkir. Fokus utama adalah pada mekanik kontrol kendaraan yang responsif, sistem deteksi tabrakan yang akurat, dan peningkatan kesulitan yang progresif antar level.

---

## 

## **Game Flow Summary**

Pemain memulai dari layar utama dengan opsi memulai game. Setelah memasuki level, pemain mengendalikan mobil dan harus menavigasi ke zona parkir yang ditandai. Pemain harus memarkir mobil di dalam zona parkir selama waktu yang ditentukan (3 detik) tanpa menabrak rintangan. Level 1 menampilkan area parkir dengan rintangan statis (pohon, bangunan, tiang lampu, dll). Level 2 memiliki mekanisme yang sama namun menambahkan mobil bot yang bergerak di area parkir sebagai tantangan tambahan. Setelah berhasil parkir, pemain dapat melanjutkan ke level berikutnya.

---

## **Look and Feel**

Game ini menggunakan gaya visual **3D realistis** dengan tampilan third-person camera di belakang mobil. Gaya visual dipilih untuk menciptakan nuansa simulasi mengemudi yang immersive dan memberikan pemain perspektif yang jelas untuk menilai jarak dan posisi mobil saat parkir.

---

## **Core Loop (Bisa dibuat Diagram)**

1. **Kendali Mobil:** Pemain mengendalikan mobil (maju, mundur, belok kiri/kanan, rem) untuk menavigasi menuju zona parkir.  
2. **Hindari Rintangan:** Pemain harus menghindari semua rintangan (Obstacle) di sepanjang jalan menuju zona parkir. Tabrakan akan menyebabkan kegagalan.  
3. **Parkir di Zona:** Pemain harus memposisikan mobil sepenuhnya di dalam ParkingZone dan tetap diam selama 3 detik untuk menyelesaikan level.

---

## **Mekanik Inti Game**

Mekanik inti dari **In The End: Parking Simulator** berfokus pada kontrol kendaraan, navigasi, dan presisi parkir. Aktivitas di dalam game memberikan tantangan berupa penghindaran rintangan dan akurasi parkir. Berikut adalah highlight mekanik utama:

### **Highlight Mekanik 1: Kontrol Kendaraan**

* **Player Controller:** Kontrol mobil dengan input akselerasi, rem, dan steering (belok kiri/kanan). Mobil dapat bergerak maju dan mundur.  
* **Camera System:** Kamera mengikuti mobil dari sudut pandang third-person untuk memberikan visibilitas yang baik saat parkir.  
* **Minimap:** Tampilan peta kecil di UI yang membantu pemain melihat posisi mobil, zona parkir, dan rintangan dari atas.

### **Highlight Mekanik 2: Sistem Parkir & Rintangan**

* **ParkingZone:** Area parkir yang ditandai dengan BoxCollider (trigger). Pemain harus memasukkan mobil ke dalam zona ini dan tetap diam selama **3 detik** (requiredStillTime) untuk menyelesaikan level.  
* **Obstacle System:** Semua objek rintangan (pohon, bangunan, tiang lampu, hydrant, billboard, traffic signal, windmill, pagar rumput, batu) memiliki tag "Obstacle" dan Collider. Tabrakan dengan objek ini akan menyebabkan kegagalan parkir.  
* **Collision Detection:** Sistem deteksi tabrakan yang memastikan mobil tidak boleh menyentuh objek bertag Obstacle selama proses parkir.

### **Highlight Mekanik 3: Level Progression**

* **Level 1 \- Static Parking:** Area parkir dengan rintangan statis. Pemain belajar kontrol dasar dan navigasi menuju zona parkir. Rintangan meliputi: Big Tree, Street Light, Building House, Rock, Grass Fence, Hydrant, BillBoard, Traffic Signal, Windmill, dan House Floor.  
* **Level 2 \- Dynamic Parking:** Mekanisme sama dengan Level 1, namun ditambahkan **mobil bot** yang bergerak/berjalan di area parkir. Mobil bot ini menjadi tantangan dinamis yang harus dihindari pemain, menambah kompleksitas dan membutuhkan timing yang lebih tepat untuk berhasil parkir.

---

## **Sistem Utama**

### **Sistem 1: Sistem Kontrol Kendaraan**

* Mobil dikendalikan menggunakan input keyboard (W/A/S/D atau Arrow Keys) untuk akselerasi, rem, dan steering.  
* Mobil memiliki fisika dasar yang realistis untuk simulasi parkir, termasuk kemampuan maju dan mundur.  
* Kamera mengikuti mobil secara dinamis untuk memberikan sudut pandang terbaik saat manuver parkir.

### **Sistem 2: Sistem Parkir (ParkingZone)**

* ParkingZone menggunakan BoxCollider sebagai trigger area.  
* Script `ParkingZone` mendeteksi ketika mobil masuk ke dalam zona dan menghitung waktu diam (still time).  
* Mobil harus tetap berada di dalam zona parkir selama **3 detik** tanpa bergerak untuk menyelesaikan level.  
* Jika mobil keluar dari zona sebelum waktu habis, timer akan reset.

### **Sistem 3: Sistem Rintangan (Obstacle)**

* Semua objek rintangan memiliki tag "Obstacle" dan Collider (MeshCollider/BoxCollider).  
* Tabrakan dengan objek ber-tag Obstacle akan menyebabkan kegagalan level.  
* Rintangan Level 1 bersifat statis (tidak bergerak).  
* Rintangan Level 2 mencakup semua rintangan statis Level 1 ditambah mobil bot yang bergerak dinamis.

### **Sistem 4: Level dan Progression**

* Game terdiri dari **2 level** dengan kesulitan bertahap.  
* Level 1: Area parkir statis \- fokus pada pembelajaran kontrol dan navigasi dasar.  
* Level 2: Area parkir dinamis \- menambahkan mobil bot yang bergerak sebagai tantangan ekstra.  
* Setelah berhasil menyelesaikan level, pemain dapat melanjutkan ke level berikutnya.

---

**REFERENSI GAME** 

1. **DR. DRIVING (2013)** \- Game parkir simulator populer yang menjadi inspirasi utama mekanik parkir dan tantangan mengemudi.

## **List Assets** 

### **3D Assets**

1. **Player Vehicle (Mobil Pemain)**  
   3D Model Mobil  
   Komponen: Rigidbody, Collider, Player Controller Script  
   Kontrol: Akselerasi, Rem, Steering (Kiri/Kanan), Mundur  
   Referensi: Dr. Driving vehicle style  

2. **Bot Vehicle (Mobil Bot \- Level 2)**  
   3D Model Mobil (berbeda dari mobil pemain)  
   Komponen: Rigidbody, Collider, AI Movement Script  
   Animasi: Idle, Moving (berjalan/bergerak di area parkir)  
   Fungsi: Rintangan dinamis di Level 2  

3. **Rintangan Statis (Obstacles)**  
   * **Natures\_Big Tree** \- Pohon besar dengan Collider  
   * **Natures\_Grass Fence** \- Pagar rumput  
   * **Natures\_Rock\_Big** \- Batu besar  
   * **Natures\_House Floor** \- Lantai bangunan  
   * **Props\_Street Light** \- Tiang lampu jalan  
   * **Props\_Hydrant** \- Hydrant  
   * **Props\_BillBoard\_small** \- Billboard kecil  
   * **Props\_Traffic Signal\_big** \- Traffic signal besar  
   * **Props\_Windmill** \- Kincir angin (dengan Animator)  
   * **Building\_House\_02\_color01** \- Bangunan rumah  

   Semua obstacle memiliki tag "Obstacle" dan Collider (MeshCollider/BoxCollider)

4. **Environment**  
   * Terrain/Ground plane  
   * Skybox Material  
   * Lighting (Directional Light, Ambient)  
   * Fog settings (opsional)  

5. **ParkingZone**  
   BoxCollider (Trigger) dengan script ParkingZone  
   Visual indicator untuk area parkir  
   requiredStillTime: 3 detik  

### **UI Assets**

1. **Minimap**  
   * MinimapRawImage \- RawImage yang menampilkan RenderTexture dari minimap camera  
   * Menampilkan posisi pemain, zona parkir, dan rintangan dari atas  

2. **HUD**  
   * Level indicator  
   * Status parkir (timer 3 detik)  
   * Pesan sukses/gagal  
   * Tombol navigasi menu (restart, next level, main menu)  

### **Scene**

1. **Level 1.unity** \- Scene level pertama dengan rintangan statis  
   Lokasi: `Assets/Scenes/Level/Level 1.unity`  
   Konten: Environment, obstacles, parking zone, player vehicle, minimap  

2. **Level 2.unity** \- Scene level kedua dengan rintangan dinamis (mobil bot)  
   Lokasi: `Assets/Scenes/Level/Level 2.unity` (to be created)  
   Konten: Sama seperti Level 1 + Bot Vehicle dengan AI movement  

### **Scripts**

1. **PlayerController** \- Kontrol mobil pemain (akselerasi, rem, steering)  
2. **ParkingZone** \- Deteksi area parkir dan timer still time (3 detik)  
3. **BotMovement** (Level 2) \- AI movement untuk mobil bot yang bergerak  
4. **GameManager** \- Manajemen level, status game, transisi scene  
5. **MinimapCamera** \- Kontrol kamera untuk minimap  
6. **CollisionHandler** \- Penanganan tabrakan dengan obstacle  

### **Audio Assets**

1. **Engine Sound** \- Suara mesin mobil (idle, accelerating)  
2. **Brake Sound** \- Suara rem  
3. **Collision Sound** \- Suara tabrakan  
4. **Parking Success Sound** \- Suara saat berhasil parkir  
5. **Background Music** \- Musik latar yang santai  

---

## **Level Design**

### **Level 1 \- Static Parking**

* **Tujuan:** Parkir mobil di ParkingZone tanpa menabrak obstacle  
* **Rintangan:** Semua obstacle statis (pohon, bangunan, tiang lampu, dll)  
* **ParkingZone Location:** Posisi di area yang membutuhkan manuver untuk mencapainya  
* **Kesulitan:** Mudah \- fokus pada pembelajaran kontrol dasar  
* **Kondisi Sukses:** Mobil berada di dalam ParkingZone selama 3 detik tanpa tabrakan  

### **Level 2 \- Dynamic Parking**

* **Tujuan:** Sama seperti Level 1  
* **Rintangan:** Semua obstacle statis Level 1 + **mobil bot yang bergerak**  
* **Bot Behavior:** Mobil bot bergerak mengikuti path/pola tertentu di area parkir  
* **Kesulitan:** Menengah \- pemain harus memperhatikan timing dan menghindari mobil bot  
* **Kondisi Sukses:** Mobil berada di dalam ParkingZone selama 3 detik tanpa tabrakan dengan obstacle apapun (termasuk mobil bot)  

---

## **Technical Specifications**

* **Game Engine:** Unity  
* **Bahasa Pemrograman:** C#  
* **Render Pipeline:** Built-in Render Pipeline / URP  
* **Target Platform:** PC (Windows)  
* **Input:** Keyboard (WASD / Arrow Keys)  
* **Physics:** Unity Physics (Rigidbody + Collider)  

---

## **Development Roadmap**

1. **Fase 1 \- Core Mechanics:** Player controller, camera system, basic physics  
2. **Fase 2 \- Level 1:** Environment setup, obstacle placement, parking zone system, minimap  
3. **Fase 3 \- Level 2:** Bot vehicle creation, AI movement system, integration with Level 2  
4. **Fase 4 \- UI/UX:** Main menu, HUD, minimap, success/fail screens  
5. **Fase 5 \- Audio & Polish:** Sound effects, background music, visual effects, bug fixing  
6. **Fase 6 \- Testing & Release:** Playtesting, optimization, final build
