namespace CoreShop.Models
{
    /// <summary>How a campaign composes the hero area.</summary>
    public enum HeroLayout
    {
        /// <summary>Dark stage with a transparent product render (needs an alpha-channel asset).</summary>
        DarkRender,

        /// <summary>Dark stage led by typography and a spec panel — no cut-out imagery.</summary>
        DarkTypography,

        /// <summary>Light stage where a white-background studio photo merges with the surface.</summary>
        LightStudio
    }

    /// <summary>Accent family used for the kicker, lighting and spec icons. All values map to
    /// design-system colors in site.css (hero-accent-* classes).</summary>
    public enum HeroAccent
    {
        Indigo,
        Ember,
        Cyan,
        Rose,
        Green
    }

    /// <summary>
    /// Everything the homepage needs to render one marketing campaign.
    /// Adding a campaign means adding one entry to <c>HeroCampaignProvider</c> — no view changes.
    /// </summary>
    public class HeroCampaign
    {
        /// <summary>Stable identifier, also usable as a preview query (<c>/?campaign=key</c>).</summary>
        public required string Key { get; init; }

        /// <summary>Small uppercase label above the headline.</summary>
        public required string Kicker { get; init; }

        /// <summary>First headline line.</summary>
        public required string HeadlineTop { get; init; }

        /// <summary>Second headline line.</summary>
        public required string HeadlineBottom { get; init; }

        public required string Subtitle { get; init; }

        public required string CtaText { get; init; }

        /// <summary>Category the CTA filters to; null links to the full catalog.</summary>
        public int? CtaCategoryId { get; init; }

        /// <summary>Reassuring microcopy under the buttons (shipping, returns, ...).</summary>
        public required string Note { get; init; }

        /// <summary>Product surfaced in the hero's commerce caption.</summary>
        public int FeaturedProductId { get; init; }

        public HeroLayout Layout { get; init; }

        public HeroAccent Accent { get; init; }

        /// <summary>Selling points rendered as a spec panel (DarkTypography layout).</summary>
        public IReadOnlyList<string> SpecChips { get; init; } = [];
    }
}
