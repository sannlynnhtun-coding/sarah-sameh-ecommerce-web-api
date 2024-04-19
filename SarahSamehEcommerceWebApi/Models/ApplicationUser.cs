using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Shipment>? Shipments { get; set; }
        public List<Payment>? Payments { get; set; }
        public List<Cart>? Carts { get; set; }
        public List<WishList>? WishLists { get; set; }
    }
}
