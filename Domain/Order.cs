using FruitsAppBackEnd.BL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FruitsAppBackEnd.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;

        [Required, Range(0.01, 9999999.99, ErrorMessage = "Price must be between 0.01 and 9999999.99.")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Required, Range(0.01, 9999999.99, ErrorMessage = "Price must be between 0.01 and 9999999.99.")]

        public string AppUserForeignKey { get; set; }
        [ForeignKey("AppUserForeignKey")]
        public AppUser AppUser { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        public List<Product> Products { get; set; } 
    }

    public class OrderDto
    {
        public DateTime Data { get; set; } = DateTime.Now;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
    }
}
 