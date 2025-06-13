using Azure.Core;
using FruitsAppBackEnd.BL;
using FruitsAppBackEnd.BL.Interfaces;
using FruitsAppBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FruitsAppBackEnd.BL.Classes
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly JWT _jwt;
        public AuthService(UserManager<AppUser> userManager, IOptions<JWT> jwt, IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResponse> RegisterWithEmailAndPassword(RegisterModel RegisterModel)
        {
            try
            {
                if (await _userManager.FindByNameAsync(RegisterModel.UserName) is not null)
                {
                    return new ApiResponse
                    {
                        Errors = new { Messages = new List<string> { "UserName already exists" } },
                        Message = "UserName already exists",
                        StatusCode = "400"
                    };
                }
                if (await _userManager.FindByEmailAsync(RegisterModel.Email) is not null)
                {
                    return new ApiResponse
                    {
                        Errors = new { Messages = new List<string> { "Email already exists" } },
                        Message = "Email already exists",
                        StatusCode = "400"
                    };
                }
                var user = new AppUser
                {
                    UserName = RegisterModel.UserName,
                    Email = RegisterModel.Email,
                };
                await _userManager.AddToRoleAsync(user, "User");
                var res = await _userManager.CreateAsync(user, RegisterModel.Password);
                if (!res.Succeeded)
                {
                    return new ApiResponse
                    {
                        Message = "Failed And Errors Found",
                        Errors = new { Messages = res.Errors.Select(i => i.Description).ToList() },
                        StatusCode = "400",
                    };
                }
                var token = await GetToken(user);

                var authModel = new AuthModel
                {
                    UserId = user.Id,
                    isAuthenticated = true,
                    Roles = new List<string> { "User" },
                    ExpiresOn = token.ValidTo,
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                };
                return new ApiResponse
                {
                    data = authModel,
                    Message = "User Created Successfully",
                    StatusCode = "200",
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
            
        }
        public async Task<ApiResponse> LoginWithEmailAndPassword(LogInModel LogInModel)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(LogInModel.Email);
                if (user is null || !await _userManager.CheckPasswordAsync(user, LogInModel.Password))
                {
                    return new ApiResponse
                    {
                        Errors = new { Messages = new List<string> { "Invalid Email Or Password" } },
                        Message = "Invalid Email Or Password",
                        StatusCode = "400"
                    };
                }
                var token = await GetToken(user);
                var authModel = new AuthModel
                {
                    UserId = user.Id,
                    isAuthenticated = true,
                    Roles = new List<string> { "User" },
                    ExpiresOn = token.ValidTo,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),

                };
                return new ApiResponse
                {
                    data = authModel,
                    Message = "Logged In Successfully",
                    StatusCode = "200",
                };

            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }
        public async Task<JwtSecurityToken> GetToken(AppUser appUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email ?? ""),
            };
            var roles = await _userManager.GetRolesAsync(appUser!);
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var credintial = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMonths(_jwt.DurationInMonths),
                    signingCredentials: credintial
                );
            return jwtToken;
        }
        public async Task<ApiResponse> LoginWithFacebook(string accessToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var appId = "OkBTRTHAg0jDAXH2M927hFrOZw329_aqBosq8Cr-7DHBzJHk";
                var appSecret = "kiopreqBTRTHAg0jDAXH2M927hFrOZw329_aqBosq8Cr-7DHBzJHk";

                // Validate Facebook token
                var response = await client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appId}|{appSecret}");
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

                if (!result?.data.is_valid)
                {
                    return new ApiResponse
                    {
                        Message = "Invalid Token",
                        Errors = new { Messages = new List<string> { "Invalid Token" } },
                        StatusCode = "400"
                    };
                }

                // Fetch user information
                var userResponse = await client.GetStringAsync($"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}");
                dynamic userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(userResponse);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userInfo.id.ToString()),
                    new Claim(ClaimTypes.Name, userInfo.name.ToString()),
                    new Claim(ClaimTypes.Email, userInfo.email?.ToString() ?? string.Empty)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _jwt.Issuer,
                    audience: _jwt.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMonths(_jwt.DurationInMonths), // Token expiration time
                    signingCredentials: creds
                );

                var authModel = new AuthModel
                {
                    UserId = userInfo.id.ToString(),
                    isAuthenticated = true,
                    Roles = new List<string> { "User" }, // You can adjust this based on your logic
                    ExpiresOn = token.ValidTo,
                    //UserName = userInfo.name,
                    Email = userInfo.email,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                };

                return new ApiResponse
                {
                    data = authModel,
                    Message = "Logged in with Facebook Successfully",
                    StatusCode = "200",
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

        public async Task<ApiResponse> LoginWithApple(string idToken)
        {
            try
            {
                // Decode and validate the Apple JWT
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(idToken);

                if (token.Issuer != "https://appleid.apple.com")
                {
                    return new ApiResponse
                    {
                        Message = "Invalid token",
                        Errors = new { Messages = new List<string> { "Invalid token" } },
                        StatusCode = "400"
                    };
                }

                // Fetch Apple's public keys
                var publicKeys = await GetApplePublicKeysAsync();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://appleid.apple.com",
                    ValidateAudience = true,
                    ValidAudience = "com.example.app",
                    ValidateLifetime = true,
                    IssuerSigningKeys = publicKeys
                };
                handler.ValidateToken(idToken, validationParameters, out _);
                // Extract user info from the token (e.g., email, user_id)
                var userId = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                var email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                var authModel = new AuthModel
                {
                    UserId = userId,
                    isAuthenticated = true,
                    Roles = new List<string> { "User" }, // You can adjust this based on your logic
                    ExpiresOn = token.ValidTo,
                    //UserName = "",
                    Email = email,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                };

                return new ApiResponse
                {
                    data = authModel,
                    Message = "Logged in with Apple Successfully",
                    StatusCode = "200",
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }
        public async Task<IEnumerable<SecurityKey>> GetApplePublicKeysAsync()
        {
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://appleid.apple.com/auth/keys");
            var jsonWebKeySet = new JsonWebKeySet(response);
            return jsonWebKeySet.Keys;
        }


        public ApiResponse LoginWithGoogle(string idToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(idToken);

                // Validate Google token issuer
                if (token.Issuer != "accounts.google.com" && token.Issuer != "https://accounts.google.com")
                {
                    return new ApiResponse
                    {
                        Message = "Invalid token issuer",
                        Errors = new { Messages = new List<string> { "Invalid token issuer" } },
                        StatusCode = "400"
                    };
                }

                var userId = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                var email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

                var authModel = new AuthModel
                {
                    UserId = userId,
                    isAuthenticated = true,
                    Roles = new List<string> { "User" },
                    ExpiresOn = token.ValidTo,
                    //UserName = "",
                    Email = email,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                };
                return new ApiResponse
                {
                    data = authModel,
                    Message = "Logged in with google Successfully",
                    StatusCode = "200",
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    Errors = new { Messages = new List<string> { ex.Message } },
                };
            }
        }

    }
}
