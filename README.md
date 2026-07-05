# CoreShop — Bilgisayar Bileşenleri E-Ticaret Uygulaması

ASP.NET Core 8.0 MVC ile geliştirilmiş, katmanlı mimariye sahip bir e-ticaret portfolyo projesidir.

Uygulama **veritabanı gerektirmeden** çalışır: tüm örnek veri, uygulama içindeki in-memory veri deposundan gelir. Depoyu klonlayıp tek komutla ayağa kaldırabilirsiniz — bağlantı dizesi, migration veya SQL Server kurulumu gerekmez.

> 📌 Bu proje aktif olarak geliştirilmektedir. Sürüm planı ve mühendislik standartları için: [ROADMAP.md](ROADMAP.md) · [AGENTS.md](AGENTS.md)

---

## Özellikler

- **Kullanıcı Sistemi** — Kayıt, giriş ve profil yönetimi (cookie tabanlı kimlik doğrulama)
- **Rol Tabanlı Yetkilendirme** — Kullanıcı / Admin rolleri
- **Ürün Kataloğu** — Kategori, marka, fiyat aralığı ve arama filtreleriyle listeleme; sıralama seçenekleri
- **Ürün Detayı** — Açıklama, stok durumu ve önerilen ürünler
- **Alışveriş Sepeti** — Session tabanlı sepet; adet artırma/azaltma, stok sınırı kontrolü
- **Sipariş Akışı** — Teslimat + kart formu, **simüle edilmiş ödeme adımı**, sipariş geçmişi ve durum takibi
- **Admin Paneli** — Dashboard istatistikleri, ürün/kategori/sipariş yönetimi, görsel yükleme, kritik stok uyarıları (≤ 5 adet)
- **Hazır Demo Verisi** — 10 kategori, 38 ürün (gerçek görsellerle), örnek siparişler ve admin hesabı

---

## Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|---|---|
| ASP.NET Core 8.0 MVC | Web uygulama çerçevesi |
| Katmanlı Mimari | CORE / MODEL / SERVICE / Web olmak üzere 4 proje |
| In-Memory Veri Deposu | Thread-safe, generic servis arkasında statik veri (bilinçli tasarım tercihi, aşağıya bakın) |
| Cookie Authentication | Rol tabanlı oturum yönetimi |
| Session | Sepet verisinin JSON olarak saklanması |
| Bootstrap 5 + Font Awesome | Responsive arayüz |
| LINQ | Filtreleme, arama ve sıralama |

---

## Mimari

```
CoreShop.CORE      ← Temel soyutlamalar: CoreEntity, ICoreService<T>
CoreShop.MODEL     ← Entity'ler: Product, Category, Order, OrderDetail, User
CoreShop.SERVICE   ← ICoreService<T> implementasyonu + in-memory veri deposu
CoreShop (Web)     ← MVC katmanı: Controller, View, ViewModel, Admin area
```

Veri erişimi `ICoreService<T>` soyutlaması üzerinden yapılır; web katmanı veri deposunun in-memory olduğunu bilmez. Bu sayede v2.0'da gerçek veritabanına (EF Core) geçiş, controller'lara dokunmadan yapılabilecektir.

---

## Kurulum

### Gereksinimler

- .NET 8.0 SDK — hepsi bu. Veritabanı gerekmez.

### Çalıştırma

```bash
git clone <repo-url>
cd CoreShop
dotnet run --project CoreShop
```

Uygulama açıldığında tüm demo verisi hazırdır.

### Demo Hesabı

| | |
|---|---|
| **E-posta** | `admin@coreshop.com` |
| **Şifre** | `Admin123` |

Admin paneline giriş yaptıktan sonra navbar'daki **Admin** butonundan ulaşabilirsiniz. Normal kullanıcı deneyimi için kayıt olup yeni hesap açabilirsiniz.

---

## Bilinçli Tasarım Tercihleri

Bu proje bir portfolyo çalışmasıdır; bazı kararlar bilinçli olarak sadelik ve "sıfır kurulum" deneyimi için verilmiştir:

- **In-memory veri deposu** — Projeyi inceleyen birinin SQL Server kurmadan, tek komutla çalışan bir uygulama görmesi hedeflenmiştir. Veriler uygulama yeniden başlatıldığında sıfırlanır. Gerçek veritabanı entegrasyonu (EF Core, config ile in-memory ↔ DB anahtarı) [ROADMAP.md](ROADMAP.md) v2.0 kapsamındadır.
- **Simüle edilmiş ödeme** — Gerçek bir ödeme sağlayıcısı entegre edilmemiştir; kart formu yalnızca format doğrulaması yapar ve kartın yalnızca son 4 hanesi saklanır.
- **Kademeli sertleştirme** — Güvenlik (modern parola hashleme, CSRF koruması), test kapsamı ve CI süreci roadmap'te ayrı milestone'lar olarak planlanmıştır ve sırayla uygulanmaktadır.

## Seed Verisi

| Kategori | Ürün | | Kategori | Ürün |
|---|---|---|---|---|
| İşlemci | 4 | | HDD | 3 |
| Ekran Kartı | 4 | | Güç Kaynağı | 4 |
| Anakart | 4 | | Kasa | 3 |
| RAM | 4 | | Soğutma | 4 |
| SSD | 4 | | Monitör | 4 |

Ek olarak: farklı durumlarda (Hazırlanıyor, Kargoda, Teslim Edildi, İptal) 8 örnek sipariş.

---

## Yol Haritası

Proje v1.0'dan v2.0'a doğru milestone'lar halinde geliştirilmektedir — sıra ve kapsam için [ROADMAP.md](ROADMAP.md).

**Mevcut sürüm: v1.1** (repo hijyeni ve adlandırma standartları)
