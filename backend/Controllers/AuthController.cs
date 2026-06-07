// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using PROYECTO_GYM_MASTER.DTOs;
using PROYECTO_GYM_MASTER.Services;

namespace PROYECTO_GYM_MASTER.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.RegistrarAsync(dto);

            if (resultado == null)
                return BadRequest(new { mensaje = "El email ya está registrado" });

            return Ok(resultado);
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _authService.LoginAsync(dto);

            if (resultado == null)
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });

            return Ok(resultado);
        }
    }
}