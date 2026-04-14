using E_Commerce_API.Models.DTO;
using E_Commerce_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authServer)
        {
            _authService = authServer;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            string token = await _authService.Register(dto);
            return Ok(new { token = token });
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            string token = await _authService.Login(dto);
            return Ok(new { token = token });
        }
    }
}
