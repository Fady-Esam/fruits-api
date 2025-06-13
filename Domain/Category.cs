using System.ComponentModel.DataAnnotations;

namespace FruitsAppBackEnd.Domain
{
    public class Category
    { 
        public int Id { get; set; }
        [StringLength(100), Required(ErrorMessage = "Name Field is Required")]
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
    public class CategoryDto
    {
        public string Name { get; set; }
    }
}
