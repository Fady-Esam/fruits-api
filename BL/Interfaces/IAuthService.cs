using FruitsAppBackEnd.Models;

namespace FruitsAppBackEnd.BL.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterWithEmailAndPassword(RegisterModel RegisterModel);
        Task<ApiResponse> LoginWithEmailAndPassword(LogInModel LogInModel);
        Task<ApiResponse> LoginWithFacebook(string accessToken);
        Task<ApiResponse> LoginWithApple(string idToken);
        ApiResponse LoginWithGoogle(string idToken);
    }
}
