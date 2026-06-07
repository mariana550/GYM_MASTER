using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using PROYECTO_GYM_MASTER.Data;
using PROYECTO_GYM_MASTER.DTOs;
using PROYECTO_GYM_MASTER.Services;

namespace PROYECTO_GYM_MASTER.Controllers
{
    [ApiController]
    [Route("api/rutinas")]
    [Authorize]
    public class RutinaController : ControllerBase
    {
        private readonly RutinaService _rutinaService;
        private readonly AppDbContext _context;

        public RutinaController(RutinaService rutinaService, AppDbContext context)
        {
            _rutinaService = rutinaService;
            _context = context;
        }
        // GET api/rutinas — Admin ve todas, Entrenador solo las suyas
        [HttpGet]
        [Authorize(Roles = "Admin,Entrenador")]
        public async Task<IActionResult> GetAll()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);
            var rol = User.FindFirstValue(ClaimTypes.Role)!;

            var rutinas = await _rutinaService.ObtenerTodasAsync(usuarioId, rol);
            return Ok(rutinas);
        }
        // GET api/rutinas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rutina = await _rutinaService.ObtenerPorIdAsync(id);
            if (rutina == null) return NotFound(new { mensaje = "Rutina no encontrada" });
            return Ok(rutina);
        }
        // GET api/rutinas/mi-rutina — Cliente ve su rutina asignada
        [HttpGet("mi-rutina")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> GetMiRutina()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);

            var rutinaAsignada = await _context.RutinasAsignadas
                .Include(r => r.Rutina)
                    .ThenInclude(r => r.Entrenador)
                        .ThenInclude(e => e.Usuario)
                .FirstOrDefaultAsync(r => r.UsuarioId == usuarioId && r.Activa);

            if (rutinaAsignada == null)
                return NotFound(new { mensaje = "No tienes rutina asignada" });

            var rutina = rutinaAsignada.Rutina;

            return Ok(new RutinaResponseDTO
            {
                Id = rutina.Id,
                Nombre = rutina.Nombre,
                Descripcion = rutina.Descripcion,
                Objetivo = rutina.Objetivo,
                DuracionSemanas = rutina.DuracionSemanas,
                FechaCreacion = rutina.FechaCreacion,
                NombreEntrenador = rutina.Entrenador.Usuario.Nombre,
                EntrenadorId = rutina.EntrenadorId
            });
        }
        // POST api/rutinas — Solo Entrenador
        [HttpPost]
        [Authorize(Roles = "Entrenador")]
        public async Task<IActionResult> Create([FromBody] RutinaDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);

            var resultado = await _rutinaService.CrearAsync(dto, usuarioId);
            if (resultado == null)
                return BadRequest(new { mensaje = "No tienes un perfil de entrenador activo" });

            return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
        }
        // PUT api/rutinas/{id} — Solo Entrenador dueño
        [HttpPut("{id}")]
        [Authorize(Roles = "Entrenador")]
        public async Task<IActionResult> Update(int id, [FromBody] EditarRutinaDTO dto)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);

            var resultado = await _rutinaService.EditarAsync(id, dto, usuarioId);
            if (resultado == null)
                return NotFound(new { mensaje = "Rutina no encontrada o no tienes permiso" });

            return Ok(resultado);
        }
        // DELETE api/rutinas/{id} — Admin o Entrenador dueño
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Entrenador")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);
            var rol = User.FindFirstValue(ClaimTypes.Role)!;

            var resultado = await _rutinaService.EliminarAsync(id, usuarioId, rol);
            if (!resultado)
                return NotFound(new { mensaje = "Rutina no encontrada o no tienes permiso" });

            return Ok(new { mensaje = "Rutina eliminada correctamente" });
        }
        // POST api/rutinas/asignar — Solo Entrenador
        [HttpPost("asignar")]
        [Authorize(Roles = "Entrenador")]
        public async Task<IActionResult> Asignar([FromBody] AsignarRutinaDTO dto)
        {
            var resultado = await _rutinaService.AsignarAsync(dto);
            if (!resultado)
                return BadRequest(new { mensaje = "Usuario o rutina no encontrados" });

            return Ok(new { mensaje = "Rutina asignada correctamente" });
        }
    }
}
