using e_commerce.Controllers.Authorization.Requests;
using e_commerce.service.Services.AuthorizationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.Authorization
{
    public class AuthorizationController : BaseController
    {
        private readonly AuthorizationService _authorizationService;

        public AuthorizationController(AuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token = _authorizationService.Login(request.Account, request.Password);
            return Ok(token);
        }
    }
}
