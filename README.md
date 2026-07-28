<div align="center">
  
  # CoreShop E-Ticaret Uygulaması
  
  [![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
  [![Mimari](https://img.shields.io/badge/Mimari-N--Tier-blue)](#)
  [![Veritabanı](https://img.shields.io/badge/Veritabanı-Yok%20(In--Memory)-orange)](#)
</div>

<br/>

ASP.NET Core 8.0 MVC ile geliştirilmiş, çok katmanlı (N-Tier) mimariye sahip bir bilgisayar bileşenleri e-ticaret projesidir. 

**Bu proje bir portfolyo çalışması olarak özel dizayn edilmiştir.** Kodu inceleyen veya projeyi deneyimlemek isteyen kişileri yormamak adına "Sıfır Kurulum" mantığıyla tasarlanmıştır:
- **Veritabanı Kurulumu Yok:** Tüm veriler uygulama çalıştığı anda In-Memory (Bellek İçi) olarak yüklenir. SQL Server vb. kurmanıza gerek yoktur.
- **Hazır Veriler ve Görseller:** Proje; arka planı temizlenmiş yüksek kaliteli ürün görselleri, hazır kategoriler, test ürünleri ve örnek sipariş verileriyle birlikte gelir. Çalıştırdığınız an mağaza dolu ve teste hazırdır.

## 🛠️ Neler Kullanıldı?
- ASP.NET Core 8.0 MVC
- N-Katmanlı Mimari (Core, Model, Service, Web)
- In-Memory Veri Deposu
- Cookie Kimlik Doğrulama (Authentication)
- Session (Sepet işlemleri için)
- Bootstrap 5 & CSS3
- LINQ

## 📌 Projede Neler Var?
- **Kullanıcı İşlemleri:** Yeni hesap oluşturma, giriş yapma ve profil yönetimi.
- **Admin Paneli:** Ürün ekleme/düzenleme, kategorileri yönetme ve sipariş durumlarını (Hazırlanıyor, Kargoda vb.) güncelleme.
- **Alışveriş Sepeti:** Ürünleri sepete ekleme, adet değiştirme ve sipariş tamamlama (simüle edilmiş ödeme).
- **Stok Takibi:** Satın alınan ürünlerin stoktan düşmesi ve kritik stok uyarıları.
- **Katalog:** Kategorilere göre filtreleme ve arama.

## 🚀 Nasıl Kullanabilirsiniz?

Sadece .NET 8 SDK yüklü olması yeterlidir.

1. Projeyi indirin:
```bash
git clone https://github.com/awqken/E-Ticaret-Core-Shop.git
