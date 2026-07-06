using CoreShop.MODEL.Entities;

namespace CoreShop.Models
{
    public class ProfileVM
    {
        public required User User { get; set; }

        /// <summary>Orders newest-first, each with its own detail lines.</summary>
        public List<ProfileOrderVM> Orders { get; set; } = new List<ProfileOrderVM>();

        public ProfileUpdateVM AddressForm { get; set; } = new ProfileUpdateVM();

        /// <summary>True when the address form was submitted with errors and must stay open.</summary>
        public bool ShowAddressForm { get; set; }
    }
}
