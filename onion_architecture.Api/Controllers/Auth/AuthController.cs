using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using onion_architecture.Application.Features.Auth;
using onion_architecture.Application.IService;

namespace onion_architecture.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(LoginDto dto)
        {
            return Ok(_authService.Login(dto));
        }
    }
}
