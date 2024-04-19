using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Change Customer_Id to string
        [ForeignKey("customer")]
        public string CustomerId { get; set; }

        public ApplicationUser Customer { get; set; }

        [ForeignKey("product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
