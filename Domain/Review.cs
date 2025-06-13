using FruitsAppBackEnd.BL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FruitsAppBackEnd.Domain
{
    public class Review
    {
        public int Id { get; set; }
        [StringLength(1000), Required(ErrorMessage = "You must enter Review Comment")]
        public string ReviewComment { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public string AppUserForeignKey { get; set; }
        [ForeignKey("AppUserForeignKey")]
        public AppUser AppUser { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }

    }

    public class ReviewDto
    {
        public string ReviewComment { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public int ProductId { get; set; }
    }

}
