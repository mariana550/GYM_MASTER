using Microsoft.EntityFrameworkCore;
using PROYECTO_GYM_MASTER.Data;
using PROYECTO_GYM_MASTER.DTOs;
using PROYECTO_GYM_MASTER.Models;

namespace PROYECTO_GYM_MASTER.Services
{
    public class RutinaService
    {
        private readonly AppDbContext _context;
        public RutinaService(AppDbContext context)
        {
            _context = context;
        }
        // Obtener todas las rutinas (Admin ve todas, Entrenador solo las suyas)
        public async Task<List<RutinaResponseDTO>> ObtenerTodasAsync(int usuarioId, string rol)
        {
            var query = _context.Rutinas
                .Include(r => r.Entrenador)
                    .ThenInclude(e => e.Usuario)
                .AsQueryable();

            if (rol == "Entrenador")
            {
                var entrenador = await _context.Entrenadores
                    .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

                if (entrenador == null) return new List<RutinaResponseDTO>();

                query = query.Where(r => r.EntrenadorId == entrenador.Id);
            }

            return await query.Select(r => new RutinaResponseDTO
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion,
                Objetivo = r.Objetivo,
                DuracionSemanas = r.DuracionSemanas,
                FechaCreacion = r.FechaCreacion,
                NombreEntrenador = r.Entrenador.Usuario.Nombre,
                EntrenadorId = r.EntrenadorId
            }).ToListAsync();
        }

        // Obtener rutina por ID
        public async Task<RutinaResponseDTO?> ObtenerPorIdAsync(int id)
        {
            return await _context.Rutinas
                .Include(r => r.Entrenador)
                    .ThenInclude(e => e.Usuario)
                .Where(r => r.Id == id)
                .Select(r => new RutinaResponseDTO
                {
                    Id = r.Id,
                    Nombre = r.Nombre,
                    Descripcion = r.Descripcion,
                    Objetivo = r.Objetivo,
                    DuracionSemanas = r.DuracionSemanas,
                    FechaCreacion = r.FechaCreacion,
                    NombreEntrenador = r.Entrenador.Usuario.Nombre,
                    EntrenadorId = r.EntrenadorId
                }).FirstOrDefaultAsync();
        }
            // Crear rutina (solo Entrenador)
        public async Task<RutinaResponseDTO?> CrearAsync(RutinaDTO dto, int usuarioId)
        {
            var entrenador = await _context.Entrenadores
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

            if (entrenador == null) return null;

            var rutina = new Rutina
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Objetivo = dto.Objetivo,
                DuracionSemanas = dto.DuracionSemanas,
                EntrenadorId = entrenador.Id,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Rutinas.Add(rutina);
            await _context.SaveChangesAsync();

            return new RutinaResponseDTO
            {
                Id = rutina.Id,
                Nombre = rutina.Nombre,
                Descripcion = rutina.Descripcion,
                Objetivo = rutina.Objetivo,
                DuracionSemanas = rutina.DuracionSemanas,
                FechaCreacion = rutina.FechaCreacion,
                NombreEntrenador = entrenador.Usuario.Nombre,
                EntrenadorId = entrenador.Id
            };
        }
        // Editar rutina (solo el Entrenador dueño)
        public async Task<RutinaResponseDTO?> EditarAsync(int id, EditarRutinaDTO dto, int usuarioId)
        {
            var entrenador = await _context.Entrenadores
                .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

            if (entrenador == null) return null;

            var rutina = await _context.Rutinas
                .Include(r => r.Entrenador)
                    .ThenInclude(e => e.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id && r.EntrenadorId == entrenador.Id);

            if (rutina == null) return null;

            if (dto.Nombre != null) rutina.Nombre = dto.Nombre;
            if (dto.Descripcion != null) rutina.Descripcion = dto.Descripcion;
            if (dto.Objetivo != null) rutina.Objetivo = dto.Objetivo;
            if (dto.DuracionSemanas.HasValue) rutina.DuracionSemanas = dto.DuracionSemanas.Value;

            await _context.SaveChangesAsync();

            return new RutinaResponseDTO
            {
                Id = rutina.Id,
                Nombre = rutina.Nombre,
                Descripcion = rutina.Descripcion,
                Objetivo = rutina.Objetivo,
                DuracionSemanas = rutina.DuracionSemanas,
                FechaCreacion = rutina.FechaCreacion,
                NombreEntrenador = rutina.Entrenador.Usuario.Nombre,
                EntrenadorId = rutina.EntrenadorId
            };
        }
        // Eliminar rutina (Admin o Entrenador dueño)
        public async Task<bool> EliminarAsync(int id, int usuarioId, string rol)
        {
            Rutina? rutina;

            if (rol == "Admin")
            {
                rutina = await _context.Rutinas.FindAsync(id);
            }
            else
            {
                var entrenador = await _context.Entrenadores
                    .FirstOrDefaultAsync(e => e.UsuarioId == usuarioId);

                if (entrenador == null) return false;

                rutina = await _context.Rutinas
                    .FirstOrDefaultAsync(r => r.Id == id && r.EntrenadorId == entrenador.Id);
            }

            if (rutina == null) return false;

            _context.Rutinas.Remove(rutina);
            await _context.SaveChangesAsync();
            return true;
        }

        // Asignar rutina a cliente (solo Entrenador)
        public async Task<bool> AsignarAsync(AsignarRutinaDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            var rutina = await _context.Rutinas.FindAsync(dto.RutinaId);

            if (usuario == null || rutina == null) return false;

            // Desactivar rutina anterior si existe
            var rutinaAnterior = await _context.RutinasAsignadas
                .FirstOrDefaultAsync(r => r.UsuarioId == dto.UsuarioId && r.Activa);

            if (rutinaAnterior != null)
                rutinaAnterior.Activa = false;

            _context.RutinasAsignadas.Add(new RutinaAsignada
            {
                UsuarioId = dto.UsuarioId,
                RutinaId = dto.RutinaId,
                FechaAsignacion = DateTime.UtcNow,
                Activa = true
            });

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
