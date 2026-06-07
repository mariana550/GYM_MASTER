using Microsoft.EntityFrameworkCore;
using PROYECTO_GYM_MASTER.Data;
using PROYECTO_GYM_MASTER.DTOs;
using PROYECTO_GYM_MASTER.Models;
using Progreso = PROYECTO_GYM_MASTER.Models.Progreso;

namespace PROYECTO_GYM_MASTER.Services
{
    public class ProgresoService
    {
        private readonly AppDbContext _context;

        public ProgresoService(AppDbContext context)
        {
            _context = context;
        }
        // Cliente ve su propio progreso
        public async Task<List<ProgresoResponseDTO>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Progresos
                .Include(p => p.Usuario)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.Fecha)
                .Select(p => new ProgresoResponseDTO
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    NombreUsuario = p.Usuario.Nombre,
                    Fecha = p.Fecha,
                    Peso = p.Peso,
                    Altura = p.Altura,
                    PorcentajeGrasa = p.PorcentajeGrasa,
                    IMC = Math.Round(p.Peso / Math.Pow(p.Altura / 100, 2), 2),
                    Notas = p.Notas
                }).ToListAsync();
        }

        // Entrenador ve el progreso de un cliente específico
        public async Task<List<ProgresoResponseDTO>> ObtenerTodoAsync()
        {
            return await _context.Progresos
                .Include(p => p.Usuario)
                .OrderByDescending(p => p.Fecha)
                .Select(p => new ProgresoResponseDTO
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    NombreUsuario = p.Usuario.Nombre,
                    Fecha = p.Fecha,
                    Peso = p.Peso,
                    Altura = p.Altura,
                    PorcentajeGrasa = p.PorcentajeGrasa,
                    IMC = Math.Round(p.Peso / Math.Pow(p.Altura / 100, 2), 2),
                    Notas = p.Notas
                }).ToListAsync();
        }

        // Registrar nuevo progreso
        public async Task<ProgresoResponseDTO> RegistrarAsync(CrearProgresoDTO dto, int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            var progreso = new Progreso
            {
                UsuarioId = usuarioId,
                Fecha = DateTime.UtcNow,
                Peso = dto.Peso,
                Altura = dto.Altura,
                PorcentajeGrasa = dto.PorcentajeGrasa,
                Notas = dto.Notas
            };

            _context.Progresos.Add(progreso);
            await _context.SaveChangesAsync();

            return new ProgresoResponseDTO
            {
                Id = progreso.Id,
                UsuarioId = progreso.UsuarioId,
                NombreUsuario = usuario!.Nombre,
                Fecha = progreso.Fecha,
                Peso = progreso.Peso,
                Altura = progreso.Altura,
                PorcentajeGrasa = progreso.PorcentajeGrasa,
                IMC = Math.Round(progreso.Peso / Math.Pow(progreso.Altura / 100, 2), 2),
                Notas = progreso.Notas
            };
        }

        // Eliminar registro de progreso (Admin o el propio usuario)
        public async Task<bool> EliminarAsync(int id, int usuarioId, string rol)
        {
            Progreso? progreso;

            if (rol == "Admin")
                progreso = await _context.Progresos.FindAsync(id);
            else
                progreso = await _context.Progresos.FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);

            if (progreso == null) return false;

            _context.Progresos.Remove(progreso);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
