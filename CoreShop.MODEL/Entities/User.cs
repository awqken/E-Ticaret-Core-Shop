using CoreShop.CORE.Entity;

namespace CoreShop.MODEL.Entities
{
    public class User : CoreEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // Address details are optional until the user fills in their profile.
        public string? City { get; set; }
        public string? District { get; set; }
        public string? FullAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
