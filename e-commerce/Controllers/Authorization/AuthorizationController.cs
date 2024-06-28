using e_commerce.Controllers.Authorization.Requests;
using e_commerce.service.Services.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.Authorization
{
    public class AuthorizationController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            var result = await _authorizationService.CheckEmailAsync(email);
            return Ok(result);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var auth = await _authorizationService.Login(request.Email!, request.Password!);
            return Ok(auth);
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var auth = await _authorizationService.Register(request.Email!, request.Password!, request.Name!);
            return Ok(auth);
        }
    }
}
