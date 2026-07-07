using CoreShop.Models;

namespace CoreShop.Services
{
    /// <summary>
    /// Catalog of homepage hero campaigns. Registered as a singleton: one campaign is
    /// picked at startup and stays active for the lifetime of the running application,
    /// so every visitor of a given deployment sees the same campaign.
    /// </summary>
    public class HeroCampaignProvider : IHeroCampaignProvider
    {
        // Layout constraints (see docs on HeroLayout): DarkRender needs a transparent
        // asset — only the AOC render qualifies; opaque studio photos go to LightStudio,
        // everything else sells with typography and specs.
        private static readonly IReadOnlyList<HeroCampaign> Campaigns =
        [
            new()
            {
                Key = "monitors",
                Kicker = "Oyun monitörü kampanyası",
                HeadlineTop = "165 Hz akıcılık.",
                HeadlineBottom = "Her karede fark.",
                Subtitle = "Seçili oyun monitörlerinde sezon fiyatları başladı. IPS panel, " +
                           "FreeSync Premium ve 1 ms tepki süresi — rekabete hazır ekranlar stokta.",
                CtaText = "Monitörleri İncele",
                CtaCategoryId = 10,
                Note = "Stoktan aynı gün kargo · 14 gün koşulsuz iade",
                FeaturedProductId = 38, // AOC 27G2SPU — the transparent render
                Layout = HeroLayout.DarkRender,
                Accent = HeroAccent.Ember
            },
            new()
            {
                Key = "gpus",
                Kicker = "Ekran kartı sezonu",
                HeadlineTop = "Kare hızını",
                HeadlineBottom = "ikiye katla.",
                Subtitle = "RTX 40 serisiyle DLSS 3 ve Frame Generation, RX 7000 serisiyle FSR 3 — " +
                           "1440p ve 4K oyun için yeni nesil ekran kartları stokta.",
                CtaText = "Ekran Kartlarını Gör",
                CtaCategoryId = 2,
                Note = "Tüm kartlar orijinal distribütör garantili",
                FeaturedProductId = 6, // RTX 4070 Super
                Layout = HeroLayout.DarkTypography,
                Accent = HeroAccent.Green,
                SpecChips = ["DLSS 3 + Frame Generation", "Ray tracing donanım desteği", "1440p ve 4K oyun performansı"]
            },
            new()
            {
                Key = "cases",
                Kicker = "Kasa koleksiyonu",
                HeadlineTop = "Sadece güçlü değil.",
                HeadlineBottom = "Göz alıcı.",
                Subtitle = "Ahşap panelli İskandinav tasarımından mesh ön yüzlü hava akışı " +
                           "canavarlarına — masanın üstünü hak eden kasalar burada.",
                CtaText = "Kasaları Keşfet",
                CtaCategoryId = 8,
                Note = "Tüm kasalarda temperli cam panel · Ücretsiz iade",
                FeaturedProductId = 29, // Fractal Design North
                Layout = HeroLayout.LightStudio,
                Accent = HeroAccent.Indigo
            },
            new()
            {
                Key = "motherboards",
                Kicker = "Anakart yükseltmeleri",
                HeadlineTop = "Sisteminin",
                HeadlineBottom = "omurgasını seç.",
                Subtitle = "PCIe 5.0, DDR5 ve WiFi 6E — Ryzen 7000 ve Intel 14. nesil için " +
                           "güçlü VRM'li anakartlar uygun fiyatlarla.",
                CtaText = "Anakartları İncele",
                CtaCategoryId = 3,
                Note = "Uzman ekibimiz uyumluluk konusunda yanında",
                FeaturedProductId = 12, // ASUS ROG STRIX B650E-F
                Layout = HeroLayout.LightStudio,
                Accent = HeroAccent.Rose
            },
            new()
            {
                Key = "ssds",
                Kicker = "NVMe hız kampanyası",
                HeadlineTop = "Yüklenme ekranlarına",
                HeadlineBottom = "veda et.",
                Subtitle = "PCIe 4.0 NVMe sürücülerle oyunlar saniyeler içinde açılır, " +
                           "projeler beklemeden aktarılır. 1 TB'dan 2 TB'a stokta.",
                CtaText = "SSD'leri Gör",
                CtaCategoryId = 5,
                Note = "PlayStation 5 uyumlu modeller mevcut",
                FeaturedProductId = 17, // Samsung 990 Pro
                Layout = HeroLayout.DarkTypography,
                Accent = HeroAccent.Cyan,
                SpecChips = ["7.450 MB/s'ye varan okuma", "PCIe 4.0 NVMe arayüzü", "Yüksek dayanıklılık ve TBW"]
            },
            new()
            {
                Key = "cpus",
                Kicker = "İşlemci yükseltme zamanı",
                HeadlineTop = "Oyun performansında",
                HeadlineBottom = "son söz.",
                Subtitle = "3D V-Cache'li Ryzen'lardan 24 çekirdekli Intel amiral gemilerine — " +
                           "sisteminin kalbini bugün yükselt.",
                CtaText = "İşlemcileri İncele",
                CtaCategoryId = 1,
                Note = "AM5 ve LGA1700 platformları stokta",
                FeaturedProductId = 2, // Ryzen 7 7800X3D
                Layout = HeroLayout.DarkTypography,
                Accent = HeroAccent.Indigo,
                SpecChips = ["8 çekirdek / 16 iş parçacığı", "96 MB 3D V-Cache", "AM5 platform desteği"]
            },
            new()
            {
                Key = "ram",
                Kicker = "Bellek kampanyası",
                HeadlineTop = "Çoklu görevde",
                HeadlineBottom = "takılma yok.",
                Subtitle = "6000 MHz DDR5 kitlerinden ekonomik DDR4 modüllerine — oyun, " +
                           "yayın ve içerik üretimi için doğru bellek burada.",
                CtaText = "Bellekleri Gör",
                CtaCategoryId = 4,
                Note = "XMP 3.0 ve AMD EXPO profilleri desteklenir",
                FeaturedProductId = 14, // G.Skill Trident Z5 32GB
                Layout = HeroLayout.LightStudio,
                Accent = HeroAccent.Cyan
            },
            new()
            {
                Key = "cooling",
                Kicker = "Soğutma çözümleri",
                HeadlineTop = "Serin kal,",
                HeadlineBottom = "hızından ödün verme.",
                Subtitle = "Çift kule hava soğutuculardan 240 mm AIO sıvı soğutmalara — " +
                           "işlemcin yük altında bile sessiz ve serin kalsın.",
                CtaText = "Soğutucuları İncele",
                CtaCategoryId = 9,
                Note = "Montaj kitleri tüm güncel soketlerle uyumlu",
                FeaturedProductId = 32, // Arctic Liquid Freezer III 240
                Layout = HeroLayout.LightStudio,
                Accent = HeroAccent.Green
            },
            new()
            {
                Key = "psus",
                Kicker = "Güç kaynağı güvencesi",
                HeadlineTop = "Sessiz. Verimli.",
                HeadlineBottom = "Sarsılmaz.",
                Subtitle = "80 PLUS Gold sertifikalı, tam modüler güç kaynakları — sistemine " +
                           "on yıl garantiyle temiz ve kararlı güç ver.",
                CtaText = "Güç Kaynaklarını Gör",
                CtaCategoryId = 7,
                Note = "10 yıla varan üretici garantisi",
                FeaturedProductId = 24, // Corsair RM850x
                Layout = HeroLayout.DarkTypography,
                Accent = HeroAccent.Ember,
                SpecChips = ["80 PLUS Gold verimlilik", "Tam modüler kablo yönetimi", "10 yıla varan garanti"]
            },
            new()
            {
                Key = "setup",
                Kicker = "Sistem toplama sezonu",
                HeadlineTop = "Hayalindeki sistemi",
                HeadlineBottom = "bugün kur.",
                Subtitle = "İşlemciden kasaya 38'den fazla parça tek çatı altında. " +
                           "Karşılaştır, stok durumunu gör, güvenle sipariş ver.",
                CtaText = "Alışverişe Başla",
                CtaCategoryId = null, // full catalog
                Note = "10 kategori · Stoktan aynı gün kargo",
                FeaturedProductId = 5, // RTX 4060 — the store's best seller
                Layout = HeroLayout.DarkTypography,
                Accent = HeroAccent.Indigo,
                SpecChips = ["38+ donanım ürünü", "10 kategori tek adreste", "14 gün koşulsuz iade"]
            }
        ];

        public HeroCampaign Current { get; } = Campaigns[Random.Shared.Next(Campaigns.Count)];

        public HeroCampaign? Find(string? key) =>
            string.IsNullOrWhiteSpace(key)
                ? null
                : Campaigns.FirstOrDefault(c => c.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
    }
}
