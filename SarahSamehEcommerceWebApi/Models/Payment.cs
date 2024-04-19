using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Method { get; set; }
        public Double Amount { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("customer")]
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

    }
}
