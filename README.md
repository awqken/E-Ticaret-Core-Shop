# CoreShop — Bilgisayar Bileşenleri E-Ticaret Platformu

ASP.NET Core 8.0 MVC ile geliştirilmiş, katmanlı mimariye sahip tam özellikli bir e-ticaret uygulamasıdır.  
Proje, ilk çalıştırmada otomatik olarak örnek verilerle (seed data) dolarak hazır hale gelir.

---
## Demo
- **Demo Versiyon** - Demo versiyonu için Database baglantısı yapmanız gerekmemektedir önceden eklenmiş olan hazır görseller , acıklamalar bulunmaktadır
---
## Özellikler

- **Kullanıcı Sistemi** — Kayıt, giriş ve profil yönetimi (cookie tabanlı kimlik doğrulama)
- **Rol Tabanlı Yetkilendirme** — Kullanıcı / Admin rolleri
- **Ürün Kataloğu** — Kategori, marka, fiyat ve arama filtreleriylee listeleme
- **Alışveriş Sepeti** — Session tabanlı sepet yönetimi
- **Sipariş Yönetimi** — Ödeme formu, sipariş geçmişi ve durum takibi
- **Admin Paneli** — Ürün, kategori ve sipariş CRUD işlemleri; dashboard istatistikleri
- **Otomatik Seed Data** — 10 kategori, 36+ ürün ve admin hesabı ilk çalıştırmada oluşturulur
- **Stok Takibi** — Kritik stok uyarıları (≤ 5 adet)
- **Ödeme API Entegrasyonu** — Harici ödeme servisi ile entegrasyon

---

## Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|---|---|
| ASP.NET Core 8.0 MVC | Web uygulama çerçevesi |
| Entity Framework Core 8 | ORM ve veritabanı yönetimi |
| SQL Server | İlişkisel veritabanı |
| Bootstrap 5 | Responsive UI bileşenleri |
| Cookie Authentication | Güvenli oturum yönetimi |
| LINQ | Veri sorgulama |
| SHA-256 | Parola hashleme |

---

## Mimari

```
CoreShop.CORE        ← Temel entity ve servis arayüzleri
CoreShop.MODEL       ← Veritabanı modelleri ve DbContext
CoreShop.SERVICE     ← Generic CRUD servis implementasyonu
CoreShop (Web)       ← MVC katmanı (Controller, View, ViewModel)
CoreShop.API         ← Ödeme API projesi
```

---

## Kurulum

### Gereksinimler

- .NET 8.0 SDK
- SQL Server (LocalDB veya tam kurulum)
- Visual Studio 2022 / VS Code

### Adımlar

1. Depoyu klonlayın:
   ```bash
   git clone <repo-url>
   cd CoreShop
   ```

2. `CoreShop/appsettings.json` dosyasındaki bağlantı dizesini kontrol edin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=CoreShopDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. Uygulamayı çalıştırın:
   ```bash
   dotnet run --project CoreShop
   ```
   Uygulama ilk açılışta veritabanını oluşturur ve örnek verileri otomatik olarak yükler.

4. Admin paneline erişmek için:
   - **E-posta:** `admin@coreshop.com`
   - **Şifre:** `Admin123`

---

## Seed Data

İlk çalıştırmada otomatik olarak oluşturulan veriler:

| Kategori | Ürün Sayısı |
|---|---|
| İşlemci | 4 |
| Ekran Kartı | 4 |
| Anakart | 4 |
| RAM | 4 |
| SSD | 4 |
| HDD | 3 |
| Güç Kaynağı | 4 |
| Kasa | 3 |
| Soğutma | 4 |
| Monitör | 4 |

---
