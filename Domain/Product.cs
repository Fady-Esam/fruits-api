using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FruitsAppBackEnd.Domain
{
    public class Product
    {
        public int Id { get; set; }
        [StringLength(100), Required(ErrorMessage = "Title Field is Required")]
        public string Title { get; set; }
        [StringLength(10000)]
        public string? Description { get; set; }
        [Required, Range(0.01, 9999999.99, ErrorMessage = "Price must be between 0.01 and 9999999.99.")]
        public decimal Price { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Quantity must be one or a positive number.")]
        public int Quantity { get; set; } = 1;
        [Range(0, 100, ErrorMessage = "Discount Percentage must be between 0 and 100")]
        public decimal Discount { get; set; } = 0;
        public byte[]? Image { get; set; }
        public string PriceUnit { get; set; } = "Kilo";
        public int SellingCount { get; set; } = 0;
        public bool IsFeatured { get; set; }
        public string ProductCode { get; set; }
        public int CategoryId { get; set; } = 1;
        public List<Rate> Rates { get; set; }
        public List<Review> Reviews { get; set; }
        public Category Category { get; set; }
        public List<OrderProduct> OrderProducts { get; set; } 
        public List<Order> Orders { get; set; }
    }
    public class ProductDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Discount { get; set; } = 0;
        public bool IsFeatured { get; set; } 
        public IFormFile? Image { get; set; }
        public int SellingCount { get; set; } = 0;
        public string ProductCode { get; set; }
        public string PriceUnit { get; set; } = "Kilo";
        public int CategoryId { get; set; } = 1;
    }
}
