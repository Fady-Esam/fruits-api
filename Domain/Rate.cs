using FruitsAppBackEnd.BL;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FruitsAppBackEnd.Domain
{
    public class Rate
    {
        public int Id { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public double RateValue { get; set; }
        public string AppUserForeignKey { get; set; }
        [ForeignKey("AppUserForeignKey")]
        public AppUser AppUser { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }


    public class RateDto
    {
        public double RateValue { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
    }
}
