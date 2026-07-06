using CoreShop.CORE.Entity;
using CoreShop.MODEL.Entities;
using CoreShop.MODEL.Constants;
using CoreShop.MODEL.Enums;
using Microsoft.AspNetCore.Identity;

namespace CoreShop.SERVICE.Data
{
    public static class InMemoryStore
    {
        public static List<Category> Categories { get; } = CreateCategories();
        public static List<Product>  Products   { get; } = CreateProducts(Categories);
        public static List<User>     Users      { get; } = CreateUsers();
        public static List<Order>    Orders     { get; } = CreateOrders();
        public static List<OrderDetail> OrderDetails { get; } = CreateOrderDetails();

        public static List<T> GetList<T>() where T : CoreEntity
        {
            if (typeof(T) == typeof(Category))    return (List<T>)(object)Categories;
            if (typeof(T) == typeof(Product))     return (List<T>)(object)Products;
            if (typeof(T) == typeof(User))        return (List<T>)(object)Users;
            if (typeof(T) == typeof(Order))       return (List<T>)(object)Orders;
            if (typeof(T) == typeof(OrderDetail)) return (List<T>)(object)OrderDetails;
            throw new InvalidOperationException($"No in-memory store for {typeof(T).Name}");
        }

        // ── Categories ────────────────────────────────────────────────────────
        private static List<Category> CreateCategories() => new()
        {
            new() { ID = 1,  CategoryName = "İşlemci",     Description = "Yüksek performanslı masaüstü işlemciler" },
            new() { ID = 2,  CategoryName = "Ekran Kartı", Description = "Güçlü GPU'lar ve grafik kartları" },
            new() { ID = 3,  CategoryName = "Anakart",     Description = "Stabil ve uyumlu anakartlar" },
            new() { ID = 4,  CategoryName = "RAM",         Description = "Hızlı DDR4 ve DDR5 bellek modülleri" },
            new() { ID = 5,  CategoryName = "SSD",         Description = "NVMe ve SATA SSD depolama birimleri" },
            new() { ID = 6,  CategoryName = "HDD",         Description = "Geniş kapasiteli sabit diskler" },
            new() { ID = 7,  CategoryName = "Güç Kaynağı", Description = "Sertifikalı ve güvenilir PSU'lar" },
            new() { ID = 8,  CategoryName = "Kasa",        Description = "ATX ve mATX mid-tower kasalar" },
            new() { ID = 9,  CategoryName = "Soğutma",     Description = "Hava ve sıvı soğutma çözümleri" },
            new() { ID = 10, CategoryName = "Monitör",     Description = "Yüksek yenileme hızlı oyun monitörleri" },
        };

        // ── Products ──────────────────────────────────────────────────────────
        private static List<Product> CreateProducts(List<Category> categories)
        {
            var catMap = categories.ToDictionary(c => c.ID);

            var products = new List<Product>
            {
                // İşlemci ─────────────────────────────────────────────────────
                new() { ID=1,  ProductName="AMD Ryzen 5 7600",             ProductBrand="AMD",            ProductPrice=4999,  ProductStock=50, CategoryId=1, ProductImage="products/ryzen5-7600.jpg", Description="AMD Ryzen 5 7600, Zen 4 mimarisi üzerine kurulu 6 çekirdek ve 12 iş parçacığı ile günlük kullanım ve oyun için ideal performans sunar. 65W TDP ile enerji verimliliğiyle öne çıkar." },
                new() { ID=2,  ProductName="AMD Ryzen 7 7800X3D",          ProductBrand="AMD",            ProductPrice=8499,  ProductStock=30, CategoryId=1, ProductImage="products/ryzen7-7800x3d.jpg", Description="AMD Ryzen 7 7800X3D, 3D V-Cache teknolojisi ile oyun performansında zirveye taşınan 8 çekirdekli işlemcidir. Büyük önbelleği sayesinde oyun FPS'lerinde rakiplerini geride bırakır." },
                new() { ID=3,  ProductName="Intel Core i5-14600K",         ProductBrand="Intel",          ProductPrice=6299,  ProductStock=40, CategoryId=1, ProductImage="products/i5-14600k.jpg", Description="Intel Core i5-14600K, 14. nesil Raptor Lake mimarisine dayanan 14 çekirdekli yüksek performanslı bir işlemcidir. Hem oyun hem içerik üretimi için mükemmel bir denge sunar." },
                new() { ID=4,  ProductName="Intel Core i9-14900K",         ProductBrand="Intel",          ProductPrice=14999, ProductStock=15, CategoryId=1, ProductImage="products/i9-14900k.jpg", Description="Intel Core i9-14900K, 24 çekirdek ve 32 iş parçacığı ile üst düzey masaüstü performansı sunar. 3D render, video düzenleme ve ağır oyun iş yükleri için tasarlanmıştır." },

                // Ekran Kartı ─────────────────────────────────────────────────
                new() { ID=5,  ProductName="NVIDIA GeForce RTX 4060",      ProductBrand="NVIDIA",         ProductPrice=12499, ProductStock=25, CategoryId=2, ProductImage="products/rtx-4060.jpg", Description="NVIDIA GeForce RTX 4060, Ada Lovelace mimarisi ile 1080p oyunlarda mükemmel performans ve DLSS 3 desteği sunar. 8 GB GDDR6 belleği ile enerji dostu bir mid-range tercihtir." },
                new() { ID=6,  ProductName="NVIDIA GeForce RTX 4070 Super", ProductBrand="NVIDIA",        ProductPrice=24999, ProductStock=20, CategoryId=2, ProductImage="products/rtx-4070-super.jpg", Description="NVIDIA GeForce RTX 4070 Super, 1440p ve 4K oyunlarda üstün performans sunan güçlü bir ekran kartıdır. Ray tracing ve DLSS 3 teknolojileri ile tam donanımlıdır." },
                new() { ID=7,  ProductName="AMD Radeon RX 7800 XT",        ProductBrand="AMD",            ProductPrice=19999, ProductStock=18, CategoryId=2, ProductImage="products/rx-7800-xt.jpg", Description="AMD Radeon RX 7800 XT, RDNA 3 mimarisi ile 1440p oyunlarda rekabetçi performans sunar. 16 GB GDDR6 belleği ve FSR 3.0 desteğiyle gelecek oyunlara hazırdır." },
                new() { ID=8,  ProductName="NVIDIA GeForce RTX 4060 Ti",   ProductBrand="NVIDIA",         ProductPrice=15999, ProductStock=22, CategoryId=2, ProductImage="products/rtx-4060-ti.jpg", Description="NVIDIA GeForce RTX 4060 Ti, 1080p ve 1440p oyunlarda yüksek kare hızları ile akıcı oyun deneyimi sunar. DLSS 3 ve Frame Generation ile performansını ikiye katlar." },

                // Anakart ─────────────────────────────────────────────────────
                new() { ID=9,  ProductName="ASUS TUF Gaming B650-PLUS",    ProductBrand="ASUS",           ProductPrice=4999,  ProductStock=35, CategoryId=3, ProductImage="products/asus-tuf-b650-plus.png", Description="ASUS TUF Gaming B650-PLUS, AMD AM5 soketli Ryzen 7000 serisi işlemcilerle tam uyumlu, güçlü VRM katmanları ve DDR5 desteğiyle güvenilir bir ATX anakartdır." },
                new() { ID=10, ProductName="MSI MAG B650 TOMAHAWK WIFI",   ProductBrand="MSI",            ProductPrice=5499,  ProductStock=28, CategoryId=3, ProductImage="products/msi-mag-b650.jpg", Description="MSI MAG B650 TOMAHAWK WIFI, dahili WiFi 6E ve Bluetooth 5.2 ile AMD Ryzen 7000 serisi için eksiksiz bir anakartdır. Güçlü soğutma ve DDR5 desteği sunar." },
                new() { ID=11, ProductName="Gigabyte B760M DS3H AX",       ProductBrand="Gigabyte",       ProductPrice=3299,  ProductStock=45, CategoryId=3, ProductImage="products/gigabyte-b760m.jpg", Description="Gigabyte B760M DS3H AX, Intel LGA1700 soketli işlemcilerle uyumlu uygun fiyatlı bir mATX anakartdır. WiFi 6E ve DDR5 desteğiyle modern sistemler için idealdir." },
                new() { ID=12, ProductName="ASUS ROG STRIX B650E-F",       ProductBrand="ASUS",           ProductPrice=7999,  ProductStock=15, CategoryId=3, ProductImage="products/asus-rog-b650e.png", Description="ASUS ROG STRIX B650E-F, PCIe 5.0 ve DDR5 desteğiyle AMD platformu için premium bir anakartdır. Güçlü VRM'leri ve zengin bağlantı seçenekleriyle üst düzey kullanıcılara yöneliktir." },

                // RAM ─────────────────────────────────────────────────────────
                new() { ID=13, ProductName="Corsair Vengeance 16GB DDR5",  ProductBrand="Corsair",        ProductPrice=2499,  ProductStock=60, CategoryId=4, ProductImage="products/corsair-ddr5-16gb.jpg", Description="Corsair Vengeance 16GB DDR5-5200MHz, DDR5 platformları için yüksek bant genişliği ve düşük gecikme süresiyle üstün performans sunar. Intel XMP 3.0 desteklidir." },
                new() { ID=14, ProductName="G.Skill Trident Z5 32GB DDR5", ProductBrand="G.Skill",        ProductPrice=4999,  ProductStock=40, CategoryId=4, ProductImage="products/gskill-ddr5-32gb.png", Description="G.Skill Trident Z5 32GB DDR5-6000MHz, yüksek hız ve büyük kapasitesiyle çoklu görev, içerik üretimi ve oyun için mükemmel bir bellek kitidir." },
                new() { ID=15, ProductName="Kingston Fury Beast 16GB DDR4", ProductBrand="Kingston",      ProductPrice=1499,  ProductStock=80, CategoryId=4, ProductImage="products/kingston-ddr4-16gb.jpg", Description="Kingston Fury Beast 16GB DDR4-3200MHz, uygun fiyatıyla DDR4 platformları için güvenilir ve hızlı bir bellek seçeneğidir. Intel XMP ve AMD EXPO desteklidir." },
                new() { ID=16, ProductName="Team T-Force Vulcan 32GB DDR5", ProductBrand="Team",          ProductPrice=3999,  ProductStock=30, CategoryId=4, ProductImage="products/team-ddr5-32gb.jpg", Description="Team T-Force Vulcan 32GB DDR5-5600MHz, şık tasarımı ve güçlü performansıyla modern sistemler için büyük kapasite ve hız sunar." },

                // SSD ─────────────────────────────────────────────────────────
                new() { ID=17, ProductName="Samsung 990 Pro 1TB NVMe",     ProductBrand="Samsung",        ProductPrice=3499,  ProductStock=50, CategoryId=5, ProductImage="products/samsung-990pro.jpg", Description="Samsung 990 Pro 1TB NVMe SSD, PCIe 4.0 arayüzü ile 7.450 MB/s okuma hızı sunar. Yüksek güvenilirliği ve uzun ömrüyle hem oyuncular hem de profesyoneller için idealdir." },
                new() { ID=18, ProductName="Kingston NV2 1TB NVMe",        ProductBrand="Kingston",       ProductPrice=1999,  ProductStock=70, CategoryId=5, ProductImage="products/kingston-nv2.jpg", Description="Kingston NV2 1TB NVMe SSD, PCIe 4.0 ile 3.500 MB/s okuma hızında uygun fiyatlı ve güvenilir bir depolama çözümü sunar. Sistem sürücüsü ve oyun depolama için mükemmeldir." },
                new() { ID=19, ProductName="WD Black SN850X 1TB NVMe",     ProductBrand="Western Digital", ProductPrice=3999, ProductStock=35, CategoryId=5, ProductImage="products/wd-sn850x.jpg", Description="WD Black SN850X 1TB, PCIe 4.0 ile 7.300 MB/s okuma hızında oyun ve içerik üretimi için piyasanın en hızlı SSD'lerinden biridir. PlayStation 5 ile de tam uyumludur." },
                new() { ID=20, ProductName="Seagate FireCuda 530 2TB NVMe", ProductBrand="Seagate",       ProductPrice=5999,  ProductStock=20, CategoryId=5, ProductImage="products/seagate-firecuda.png", Description="Seagate FireCuda 530 2TB, PCIe 4.0 ile 7.300 MB/s hızında büyük kapasiteli depolama sunar. Dahili ısı yayıcısı ile uzun süre yoğun kullanımlarda sorunsuz çalışır." },

                // HDD ─────────────────────────────────────────────────────────
                new() { ID=21, ProductName="Seagate Barracuda 2TB",        ProductBrand="Seagate",        ProductPrice=1199,  ProductStock=90, CategoryId=6, ProductImage="products/seagate-barracuda.jpg", Description="Seagate Barracuda 2TB, 7200 RPM ve 256 MB önbellek ile güvenilir ve uygun fiyatlı bir depolama çözümü sunar. Yedekleme ve arşiv amaçlı kullanım için uygundur." },
                new() { ID=22, ProductName="WD Blue 4TB",                  ProductBrand="Western Digital", ProductPrice=2499, ProductStock=55, CategoryId=6, ProductImage="products/wd-blue-4tb.png", Description="WD Blue 4TB, 5400 RPM ve 256 MB önbellek ile büyük kapasiteli veri arşivleme ve yedekleme için ideal bir depolama çözümüdür. 2 yıl garanti ile birlikte gelir." },
                new() { ID=23, ProductName="Toshiba P300 2TB",             ProductBrand="Toshiba",        ProductPrice=1099,  ProductStock=75, CategoryId=6, ProductImage="products/toshiba-p300.jpg", Description="Toshiba P300 2TB, 7200 RPM hızı ve 64 MB önbelleğiyle masaüstü sistemler için güvenilir ve ekonomik bir depolama seçeneğidir. Sessiz çalışma profili ile dikkat çeker." },

                // Güç Kaynağı ─────────────────────────────────────────────────
                new() { ID=24, ProductName="Corsair RM850x 850W",          ProductBrand="Corsair",        ProductPrice=3799,  ProductStock=30, CategoryId=7, ProductImage="products/corsair-rm850x.png", Description="Corsair RM850x 850W, 80 PLUS Gold sertifikasıyla yüksek verimlilik ve tam modüler kablo yönetimi sunan güvenilir bir güç kaynağıdır. 10 yıl garanti ile gelmektedir." },
                new() { ID=25, ProductName="be quiet! Pure Power 11 750W", ProductBrand="be quiet!",      ProductPrice=2999,  ProductStock=25, CategoryId=7, ProductImage="products/bequiet-750w.jpg", Description="be quiet! Pure Power 11 750W, 80 PLUS Gold sertifikası ve sessiz çalışmasıyla ev ve ofis kullanımı için ideal uygun fiyatlı bir güç kaynağıdır." },
                new() { ID=26, ProductName="Seasonic Focus GX-650",         ProductBrand="Seasonic",      ProductPrice=2799,  ProductStock=35, CategoryId=7, ProductImage="products/seasonic-gx650.jpg", Description="Seasonic Focus GX-650, 80 PLUS Gold sertifikası ve 10 yıl garanti ile premium kalitede tam modüler bir güç kaynağıdır. Fanless modu ile düşük yüklerde sessiz çalışır." },
                new() { ID=27, ProductName="EVGA SuperNOVA 850 G6",        ProductBrand="EVGA",           ProductPrice=3499,  ProductStock=20, CategoryId=7, ProductImage="products/evga-850g6.jpg", Description="EVGA SuperNOVA 850 G6, 80 PLUS Gold onaylı ve tam modüler yapısıyla üst düzey sistemler için güvenilir güç kaynağı çözümü sunar. EcoMode özelliğiyle sessiz çalışma imkânı sağlar." },

                // Kasa ────────────────────────────────────────────────────────
                new() { ID=28, ProductName="Lian Li LANCOOL 216",          ProductBrand="Lian Li",        ProductPrice=2499,  ProductStock=20, CategoryId=8, ProductImage="products/lianli-lancool216.jpg", Description="Lian Li LANCOOL 216, mükemmel hava akışı tasarımı ve tempered glass panel ile estetik ve performansı bir arada sunan mid-tower bir kasadır. İki 160mm fan dahildir." },
                new() { ID=29, ProductName="Fractal Design North",         ProductBrand="Fractal Design",  ProductPrice=3499, ProductStock=15, CategoryId=8, ProductImage="products/fractal-north.jpg", Description="Fractal Design North, ahşap ön panel ve temiz İskandinav tasarımıyla odaya uyum sağlayan premium bir mid-tower kasadır. Mükemmel hava akışı ve modüler iç yapısıyla öne çıkar." },
                new() { ID=30, ProductName="NZXT H510 Flow",               ProductBrand="NZXT",           ProductPrice=2999,  ProductStock=18, CategoryId=8, ProductImage="products/nzxt-h510flow.jpg", Description="NZXT H510 Flow, delikli ön panel tasarımı sayesinde gelişmiş hava akışı ve kablo yönetimiyle şık görünümü buluşturan bir mid-tower kasadır." },

                // Soğutma ─────────────────────────────────────────────────────
                new() { ID=31, ProductName="Noctua NH-D15",                ProductBrand="Noctua",         ProductPrice=2199,  ProductStock=30, CategoryId=9, ProductImage="products/noctua-nhd15.jpg", Description="Noctua NH-D15, çift kule tasarımı ve iki NF-A15 fan ile yüksek TDP'li işlemciler için piyasanın en iyi hava soğutucularından biridir. Sessiz ve son derece etkin bir çözümdür." },
                new() { ID=32, ProductName="Arctic Liquid Freezer III 240", ProductBrand="Arctic",        ProductPrice=1999,  ProductStock=40, CategoryId=9, ProductImage="products/arctic-lf3-240.jpg", Description="Arctic Liquid Freezer III 240mm AIO, yüksek performanslı pompa ve geniş radyatör ile işlemcinizi etkin biçimde soğutur. Uygun fiyatıyla değer/performans kategorisinin lideridir." },
                new() { ID=33, ProductName="be quiet! Dark Rock Pro 4",    ProductBrand="be quiet!",      ProductPrice=2499,  ProductStock=25, CategoryId=9, ProductImage="products/bequiet-drp4.jpg", Description="be quiet! Dark Rock Pro 4, ikiz kule tasarımı ve iki sessiz fan ile üst düzey işlemcileri etkin biçimde soğutur. 250W TDP desteği ve şık mat siyah görünümüyle dikkat çeker." },
                new() { ID=34, ProductName="Corsair iCUE H100i Elite 240mm", ProductBrand="Corsair",     ProductPrice=2999,  ProductStock=22, CategoryId=9, ProductImage="products/corsair-h100i.jpg", Description="Corsair iCUE H100i Elite 240mm AIO, RGB aydınlatmalı pompa başlığı ve yüksek performanslı soğutma ile hem görsel hem de termal açıdan üst düzey bir deneyim sunar." },

                // Monitör ─────────────────────────────────────────────────────
                new() { ID=35, ProductName="LG 27GP850-B 27\" 180Hz",      ProductBrand="LG",             ProductPrice=7499,  ProductStock=20, CategoryId=10, ProductImage="products/lg-27gp850.jpg", Description="LG 27GP850-B, Nano IPS paneli ve 180Hz yenileme hızıyla rekabetçi oyunlar için mükemmel görüntü kalitesi ve akıcılık sunan 27 inç bir oyun monitörüdür. 1ms GtG tepki süresi vardır." },
                new() { ID=36, ProductName="Samsung Odyssey G5 27\" 165Hz", ProductBrand="Samsung",       ProductPrice=6999,  ProductStock=25, CategoryId=10, ProductImage="products/samsung-g5.jpg", Description="Samsung Odyssey G5, 1000R kavisli VA paneli ve 165Hz yenileme hızıyla sürükleyici oyun deneyimi sunar. HDR10 ve FreeSync Premium desteğiyle tam donanımlıdır." },
                new() { ID=37, ProductName="ASUS TUF Gaming VG27AQ 27\"",  ProductBrand="ASUS",           ProductPrice=7999,  ProductStock=18, CategoryId=10, ProductImage="products/asus-vg27aq.jpg", Description="ASUS TUF Gaming VG27AQ, WQHD IPS paneli ve 165Hz hızıyla keskin ve akıcı görüntü sunan premium bir oyun monitörüdür. G-Sync Compatible ve HDR10 desteklidir." },
                new() { ID=38, ProductName="AOC 27G2SPU 27\" 165Hz",       ProductBrand="AOC",            ProductPrice=5999,  ProductStock=30, CategoryId=10, ProductImage="products/aoc-27g2spu.png", Description="AOC 27G2SPU, IPS paneli ve 165Hz yenileme hızıyla uygun fiyatlı kategorisinde yüksek performanslı görüntü sunan 27 inç bir oyun monitörüdür. FreeSync Premium desteklidir." },
            };

            foreach (var p in products)
                if (catMap.TryGetValue(p.CategoryId, out var cat))
                    p.Category = cat;

            return products;
        }

        // ── Users ─────────────────────────────────────────────────────────────
        private static List<User> CreateUsers()
        {
            var admin = new User
            {
                ID          = 1,
                FullName    = "Admin",
                Email       = "admin@coreshop.com",
                Role        = UserRoles.Admin,
                City        = "İstanbul",
                District    = "Kadıköy",
                FullAddress = "CoreShop Merkez Ofis",
                PhoneNumber = "05001234567"
            };

            admin.Password = new PasswordHasher<User>().HashPassword(admin, "Admin123");

            return new List<User> { admin };
        }

        // ── Orders ────────────────────────────────────────────────────────────
        private static List<Order> CreateOrders() => new()
        {
            new() { ID=1, UserId=1, TotalPrice=21498, Status=OrderStatus.Delivered, OrderDate=DateTime.Now.AddDays(-30), FullName="Ahmet Yılmaz",  PhoneNumber="05321234567", City="İstanbul",  District="Kadıköy",    FullAddress="Moda Cad. No:12", CardName="Ahmet Yılmaz",  CardLast4="4242" },
            new() { ID=2, UserId=1, TotalPrice=8499,  Status=OrderStatus.Delivered, OrderDate=DateTime.Now.AddDays(-25), FullName="Ayşe Kaya",     PhoneNumber="05339876543", City="Ankara",    District="Çankaya",    FullAddress="Atatürk Blv. No:55", CardName="Ayşe Kaya",   CardLast4="1234" },
            new() { ID=3, UserId=1, TotalPrice=34999, Status=OrderStatus.Shipped,       OrderDate=DateTime.Now.AddDays(-10), FullName="Mehmet Demir",  PhoneNumber="05441112233", City="İzmir",     District="Konak",      FullAddress="Kemeraltı Cad. No:8", CardName="Mehmet Demir", CardLast4="5678" },
            new() { ID=4, UserId=1, TotalPrice=6299,  Status=OrderStatus.Preparing,  OrderDate=DateTime.Now.AddDays(-5),  FullName="Fatma Çelik",   PhoneNumber="05352223344", City="Bursa",     District="Osmangazi",  FullAddress="İnegöl Yolu No:3", CardName="Fatma Çelik",   CardLast4="9012" },
            new() { ID=5, UserId=1, TotalPrice=12499, Status=OrderStatus.Paid,          OrderDate=DateTime.Now.AddDays(-2),  FullName="Ali Şahin",     PhoneNumber="05467778899", City="Antalya",   District="Muratpaşa",  FullAddress="Lara Cad. No:77", CardName="Ali Şahin",      CardLast4="3456" },
            new() { ID=6, UserId=1, TotalPrice=4999,  Status=OrderStatus.Delivered, OrderDate=DateTime.Now.AddDays(-20), FullName="Zeynep Arslan", PhoneNumber="05321239876", City="İstanbul",  District="Beşiktaş",   FullAddress="Barbaros Blv. No:22", CardName="Zeynep Arslan", CardLast4="7890" },
            new() { ID=7, UserId=1, TotalPrice=19999, Status=OrderStatus.Delivered, OrderDate=DateTime.Now.AddDays(-15), FullName="Emre Yıldız",   PhoneNumber="05449990011", City="Eskişehir", District="Odunpazarı", FullAddress="Yunus Emre Mah. No:5", CardName="Emre Yıldız",  CardLast4="2345" },
            new() { ID=8, UserId=1, TotalPrice=24999, Status=OrderStatus.Cancelled,     OrderDate=DateTime.Now.AddDays(-8),  FullName="Selin Koç",     PhoneNumber="05363334455", City="Konya",     District="Selçuklu",   FullAddress="Mevlana Cad. No:9", CardName="Selin Koç",     CardLast4="6789" },
        };

        // ── Order Details ─────────────────────────────────────────────────────
        private static List<OrderDetail> CreateOrderDetails() => new()
        {
            // Sipariş 1: RTX 4060 + Ryzen 5 7600
            new() { ID=1,  OrderId=1, ProductId=5,  ProductName="NVIDIA GeForce RTX 4060",  Quantity=1, UnitPrice=12499, ProductImage="products/rtx-4060.jpg" },
            new() { ID=2,  OrderId=1, ProductId=1,  ProductName="AMD Ryzen 5 7600",          Quantity=1, UnitPrice=4999,  ProductImage="products/ryzen5-7600.jpg" },
            new() { ID=3,  OrderId=1, ProductId=13, ProductName="Corsair Vengeance DDR5",   Quantity=1, UnitPrice=4000,  ProductImage="products/corsair-ddr5-16gb.jpg" },
            // Sipariş 2: Ryzen 7 7800X3D
            new() { ID=4,  OrderId=2, ProductId=2,  ProductName="AMD Ryzen 7 7800X3D",       Quantity=1, UnitPrice=8499,  ProductImage="products/ryzen7-7800x3d.jpg" },
            // Sipariş 3: RTX 4070 Super + i9-14900K
            new() { ID=5,  OrderId=3, ProductId=6,  ProductName="NVIDIA GeForce RTX 4070 Super", Quantity=1, UnitPrice=24999, ProductImage="products/rtx-4070-super.jpg" },
            new() { ID=6,  OrderId=3, ProductId=4,  ProductName="Intel Core i9-14900K",      Quantity=1, UnitPrice=10000, ProductImage="products/i9-14900k.jpg" },
            // Sipariş 4: i5-14600K
            new() { ID=7,  OrderId=4, ProductId=3,  ProductName="Intel Core i5-14600K",      Quantity=1, UnitPrice=6299,  ProductImage="products/i5-14600k.jpg" },
            // Sipariş 5: RTX 4060
            new() { ID=8,  OrderId=5, ProductId=5,  ProductName="NVIDIA GeForce RTX 4060",   Quantity=1, UnitPrice=12499, ProductImage="products/rtx-4060.jpg" },
            // Sipariş 6: Ryzen 5 7600
            new() { ID=9,  OrderId=6, ProductId=1,  ProductName="AMD Ryzen 5 7600",          Quantity=1, UnitPrice=4999,  ProductImage="products/ryzen5-7600.jpg" },
            // Sipariş 7: RX 7800 XT
            new() { ID=10, OrderId=7, ProductId=7,  ProductName="AMD Radeon RX 7800 XT",     Quantity=1, UnitPrice=19999, ProductImage="products/rx-7800-xt.jpg" },
            // Sipariş 8: RTX 4070 Super (iptal)
            new() { ID=11, OrderId=8, ProductId=6,  ProductName="NVIDIA GeForce RTX 4070 Super", Quantity=1, UnitPrice=24999, ProductImage="products/rtx-4070-super.jpg" },
        };
    }
}
