
using System.ComponentModel.DataAnnotations;

namespace FruitsAppBackEnd.Models
{
    public class AuthModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool isAuthenticated { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }

    public class ApiResponse
    {
        public object data { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }
        public string StatusCode { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage = "UserName Field is Required")]
        [MaxLength(30, ErrorMessage = "Your UserName must not exceed 30 charcters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email Field is Required")]
        [EmailAddress]
        [MaxLength(40, ErrorMessage = "Your Email must not exceed 40 charcters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Field is Required")]
        [MaxLength(50, ErrorMessage = "Your Password must not exceed 50 charcters")]
        public string Password { get; set; }
    }
    public class LogInModel
    {
        [Required(ErrorMessage = "Email Field is Required")]
        [EmailAddress]
        [MaxLength(40, ErrorMessage = "Your Email must not exceed 40 charcters")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Field is Required")]
        [MaxLength(50, ErrorMessage = "Your Password must not exceed 50 charcters")]
        public string Password { get; set; }
    }
}
