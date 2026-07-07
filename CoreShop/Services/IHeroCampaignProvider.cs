using CoreShop.Models;

namespace CoreShop.Services
{
    public interface IHeroCampaignProvider
    {
        /// <summary>The campaign chosen for this application run.</summary>
        HeroCampaign Current { get; }

        /// <summary>Looks a campaign up by key (used to preview a specific campaign).</summary>
        HeroCampaign? Find(string? key);
    }
}
