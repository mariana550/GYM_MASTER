using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PROYECTO_GYM_MASTER.DTOs;
using PROYECTO_GYM_MASTER.Services;

namespace PROYECTO_GYM_MASTER.Controllers
{
    [ApiController]
    [Route("api/progreso")]
    [Authorize]
    public class ProgresoApiController : ControllerBase
    {
        private readonly ProgresoService _progresoService;

        public ProgresoApiController(ProgresoService progresoService)
        {
            _progresoService = progresoService;
        }

        [HttpGet("mio")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> GetMiProgreso()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);
            var progreso = await _progresoService.ObtenerPorUsuarioAsync(usuarioId);
            return Ok(progreso);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Entrenador")]
        public async Task<IActionResult> GetAll()
        {
            var progreso = await _progresoService.ObtenerTodoAsync();
            return Ok(progreso);
        }

        [HttpGet("usuario/{id}")]
        [Authorize(Roles = "Admin,Entrenador")]
        public async Task<IActionResult> GetByUsuario(int id)
        {
            var progreso = await _progresoService.ObtenerPorUsuarioAsync(id);
            return Ok(progreso);
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Create([FromBody] CrearProgresoDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);

            var resultado = await _progresoService.RegistrarAsync(dto, usuarioId);
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);
            var rol = User.FindFirstValue(ClaimTypes.Role)!;

            var resultado = await _progresoService.EliminarAsync(id, usuarioId, rol);
            if (!resultado)
                return NotFound(new { mensaje = "Registro no encontrado o sin permiso" });

            return Ok(new { mensaje = "Registro eliminado correctamente" });
        }
    }
}