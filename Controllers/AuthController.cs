using FruitsAppBackEnd.BL.Interfaces;
using FruitsAppBackEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace FruitsAppBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccountWithEmailAndPassword(RegisterModel RegisterModel)
        {
            if (!ModelState.IsValid)
            {
                // handle this request
                return BadRequest("Something Went Wrong Please Try Again");
            }
            var apiResponse = await _authService.RegisterWithEmailAndPassword(RegisterModel);
            var authModel = (AuthModel)apiResponse.data;
            if (authModel is null || !authModel.isAuthenticated)
            {
                return BadRequest(new { apiResponse.Message, apiResponse.Errors, apiResponse.StatusCode } );
            }
            return Ok(apiResponse);
        }
        [HttpPost("log-in")]
        public async Task<IActionResult> LoginAccountWithEmailAndPassword(LogInModel LogInModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something Went Wrong Please Try Again");
            }
            var apiResponse = await _authService.LoginWithEmailAndPassword(LogInModel);
            var authModel = (AuthModel)apiResponse.data;
            if (authModel is null || !authModel.isAuthenticated)
            {
                return BadRequest(new { apiResponse.Message, apiResponse.Errors, apiResponse.StatusCode });
            }
            return Ok(apiResponse);
        }

        [HttpPost("log-in-with-google")]
        public  IActionResult LoginAccountWithGoogle(string idToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something Went Wrong Please Try Again");
            }
            var apiResponse =  _authService.LoginWithGoogle(idToken);
            var authModel = (AuthModel)apiResponse.data;
            if (authModel is null || !authModel.isAuthenticated)
            {
                return BadRequest(new { apiResponse.Message, apiResponse.Errors, apiResponse.StatusCode });
            }
            return Ok(apiResponse);
        }
        [HttpPost("log-in-with-Facebook")]
        public async Task<IActionResult> LoginWithFacebook(string idToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something Went Wrong Please Try Again");
            }
            var apiResponse = await _authService.LoginWithFacebook(idToken);
            var authModel = (AuthModel)apiResponse.data;
            if (authModel is null || !authModel.isAuthenticated)
            {
                return BadRequest(new { apiResponse.Message, apiResponse.Errors, apiResponse.StatusCode });
            }
            return Ok(apiResponse);
        }
        [HttpPost("log-in-with-apple")]
        public async Task<IActionResult> LoginWithApple(string idToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Something Went Wrong Please Try Again");
            }
            var apiResponse = await _authService.LoginWithApple(idToken);
            var authModel = (AuthModel)apiResponse.data;
            if (authModel is null || !authModel.isAuthenticated)
            {
                return BadRequest(new { apiResponse.Message, apiResponse.Errors, apiResponse.StatusCode });
            }
            return Ok(apiResponse);
        }


    }
}
