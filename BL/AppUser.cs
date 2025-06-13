using FruitsAppBackEnd.Domain;
using Microsoft.AspNetCore.Identity;

namespace FruitsAppBackEnd.BL
{
    public class AppUser : IdentityUser
    {
        public List<Order> Orders { get; set; }
        public Rate Rete { get; set; }
        public Review Review { get; set; }
    }
}
