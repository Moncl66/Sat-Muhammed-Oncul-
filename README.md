# 🏢 Apartman Aidat Takip Sistemi

Bu proje, **Sistem Analizi ve Tasarımı** dersi kapsamında hazırlanmış olup apartman yöneticilerinin daire sakinleriyle ilgili **aidat, borç, ödeme ve gecikme** bilgilerini dijital ortamda kolayca yönetebilmesini sağlamayı amaçlamaktadır.

---

## 📘 Proje Özeti

Geleneksel yöntemlerde aidat takibi genellikle defter veya Excel dosyalarıyla manuel olarak yapılmakta, bu da zaman kaybına ve hatalara neden olmaktadır.  
Bu sistem sayesinde her daire için aylık aidat bilgileri kaydedilmekte; ödemesini yapan, geciken veya borçlu olan sakinler otomatik olarak listelenmektedir.  

Sistemde iki temel kullanıcı rolü vardır:  
- 🧑‍💼 **Yönetici**: Daire bilgilerini ekler, aidat tutarını belirler, ödeme bilgilerini girer ve rapor oluşturur.  
- 🏠 **Daire Sakini**: Sisteme giriş yaparak kendi borç durumunu ve geçmiş ödemelerini görüntüler.  

Bu yapı apartman yönetiminde şeffaflığı sağlar, verileri düzenli tutar ve yöneticinin iş yükünü azaltır.

---

## 🎯 Amaç ve Hedefler

Projenin amacı; apartman yöneticilerinin aidat, borç ve ödeme işlemlerini dijital ortamda **hızlı, hatasız ve şeffaf** biçimde yönetebilmesini sağlamaktır.  
Hedef; manuel işlemleri ortadan kaldırarak zaman tasarrufu ve kolay takip imkânı sunmaktır.

---

## 🧩 Kullanılan Diyagramlar

- ✅ ![Use Case Diyagramı](Usecase_diyagrami.png)  
- 🔁 ![DFD Diyagramı](DFD_diyagrami.png)  
- 🧱 ![ER Diyagramı](Er_diyagrami.png)  

Tüm diyagramlar bu projeye dahil edilmiştir.

---

## 🗂️ Dosya Listesi

| Dosya Adı | Açıklama |
|------------|-----------|
| `Apartman_Aidat_Takip_Sistemi_Proje_Formu.docx` | Proje bilgi formu |
| `UseCase.png` | Kullanım senaryosu diyagramı |
| `DFD.png` | Veri akış diyagramı |
| `ER.png` | Veritabanı diyagramı |
---

## 🚀 Kullanılan Teknolojiler (Uygulama)

Projenin analiz ve tasarım aşaması tamamlandıktan sonra, uygulama (kodlama) aşamasında aşağıdaki teknolojiler kullanılmıştır:

* **Platform:** ASP.NET Core 8.0
* **Mimari:** MVC (Model-View-Controller)
* **Veritabanı:** SQLite (Geliştirme için)
* **Veri Erişimi (ORM):** Entity Framework Core 8.0 (EF Core)
* **Güvenlik:** ASP.NET Core Identity (Authentication ve Rol Bazlı Authorization)
* **Arayüz:** HTML5, CSS, Bootstrap (ve Razor Pages)

---

## 🏁 Projeyi Çalıştırma ve Test Etme

Proje, veritabanını otomatik olarak oluşturan ve test verilerini (roller ve ilk admin) ekleyen bir altyapıya sahiptir.

### 1. Kurulum

1.  Projeyi klonlayın veya ZIP olarak indirin.
2.  `[ProjeAdı].sln` dosyasını Visual Studio 2022 ile açın.
3.  Üst menüden **Araçlar (Tools)** > **NuGet Paket Yöneticisi (NuGet Package Manager)** > **Paket Yöneticisi Konsolu (Package Manager Console)**'nu açın.
4.  Fiziksel veritabanı dosyasını (`aidat.db`) oluşturmak ve tüm tabloları (Daire, Aidat, Kullanıcılar, Roller vb.) eklemek için `PM>` satırına şu komutu yazın:
    ```bash
    Update-Database
    ```
5.  Projeyi (Yeşil ▶ Oynat tuşu veya F5) çalıştırın.

### 2. Test Akışı ve Kullanıcı Rolleri

Proje, iki farklı kullanıcı rolünü test edecek şekilde ayarlanmıştır. Sistem, `DbInitializer` adlı bir "tohumlama" (seeding) sınıfı sayesinde, rollerin ve ilk yönetici hesabının veritabanında var olmasını garanti eder.

#### Test 1: 🧑‍💼 Yönetici (Admin) Akışı

1.  Proje açıldığında, üst menüden **"Giriş Yap"** (Login) linkine tıklayın.
2.  Aşağıdaki varsayılan yönetici hesabıyla giriş yapın:
    * **E-posta:** `admin@admin.com`
    * **Şifre:** `Admin123!`
3.  Giriş yaptığınızda, menüde **"Daire Yönetimi"**, **"Aidat Yönetimi"** ve **"Ödeme Yönetimi"** linklerinin göründüğünü doğrulayın.
4.  Bu panelleri kullanarak yeni bir daire oluşturabilir, o daireye aidat atayabilir ve ödeme alabilirsiniz.

#### Test 2: 🏠 Daire Sakini Akışı

1.  Sistemden "Çıkış Yap" (Logout) deyin.
2.  Üst menüden **"Kayıt Ol"** (Register) linkine tıklayın.
3.  `sakin@sakin.com` (Şifre: `Sakin123!`) gibi **yeni bir normal kullanıcı** kaydedin. (Bu kullanıcı otomatik olarak "Daire Sakini" rolüne atanır).
4.  Giriş yaptığınızda, menüde "Yönetici" panellerinin **görünmediğini** ve sadece **"Borçlarım"** linkinin göründüğünü doğrulayın.
5.  "Borçlarım" linkine tıkladığınızda, (henüz bir daireye atanmadığınız için) **"Daire Atanmamış"** uyarısını göreceksiniz.
6.  Şimdi, bu sakini bir daireye atamak için:
    * Çıkış yapın ve `admin@admin.com` ile **tekrar giriş yapın**.
    * "Daire Yönetimi"ne gidin ve bir dairenin "Düzenle" (Edit) linkine tıklayın.
    * "Atanmış Sakin (Kullanıcı)" açılır listesinden `sakin@sakin.com` kullanıcısını seçin ve "Kaydet"e basın.
7.  **SON TEST:** Tekrar çıkış yapın ve `sakin@sakin.com` ile **yeniden giriş yapın**.
8.  "Borçlarım" linkine tıkladığınızda, artık o daireye ait aidat ve borçların listelendiği paneli göreceksiniz.

---

## 💻 Uygulama Özellikleri

Proje, analiz dosyasındaki gereksinimlerin tamamını karşılamaktadır:

* **Güvenlik:** Sayfalar `[Authorize]` etiketi ile korunmaktadır. "Yönetici" panellerine (`[Authorize(Roles = "Yönetici")]`) "Daire Sakini" rolündeki kullanıcılar erişemez (Erişim Engellendi - Access Denied).
* **Akıllı İş Mantığı (Ödemeler):**
    * `OdemelerController`'da yeni bir **Ödeme** kaydı oluşturulduğunda, o ödemenin bağlı olduğu `Aidat` kaydı bulunur ve `OdendiMi` durumu otomatik olarak `true` yapılır.
    * Bir **Ödeme** kaydı silindiğinde, bağlı olduğu `Aidat` kaydı bulunur ve `OdendiMi` durumu `false` olarak geri alınır.
* **Akıllı Filtreleme:**
    * "Yeni Ödeme Ekle" sayfasındaki açılır liste, kullanıcıya sadece `OdendiMi == false` olan, yani **ödenmemiş** aidatları gösterir.

---

## 🚀 Yazar
👤 **Muhammed Öncül**  
📘 Bilgisayar Teknolojileri Bölümü
