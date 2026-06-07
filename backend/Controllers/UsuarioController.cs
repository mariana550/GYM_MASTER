using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PROYECTO_GYM_MASTER.DTOs;
using PROYECTO_GYM_MASTER.Services;

namespace PROYECTO_GYM_MASTER.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("clientes")]
        [Authorize(Roles = "Admin,Entrenador")]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _usuarioService.ObtenerClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);
            var rol = User.FindFirstValue(ClaimTypes.Role)!;

            if (rol != "Admin" && usuarioId != id)
                return Forbid();

            var usuario = await _usuarioService.ObtenerPorIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            return Ok(usuario);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] EditarUsuarioDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _usuarioService.EditarAsync(id, dto);
            if (resultado == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            return Ok(resultado);
        }

        [HttpPut("perfil")]
        public async Task<IActionResult> UpdatePerfil([FromBody] EditarPerfilDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);

            var resultado = await _usuarioService.EditarPerfilAsync(usuarioId, dto);
            if (resultado == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _usuarioService.EliminarAsync(id);
            if (!resultado)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            return Ok(new { mensaje = "Usuario desactivado correctamente" });
        }
    }
}