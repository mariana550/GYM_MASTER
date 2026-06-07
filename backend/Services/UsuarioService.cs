using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PROYECTO_GYM_MASTER.Data;
using PROYECTO_GYM_MASTER.DTOs;

namespace PROYECTO_GYM_MASTER.Services
{
    public class UsuarioService
    {
            private readonly AppDbContext _context;

            public UsuarioService(AppDbContext context)
            {
                _context = context;
            }

            // Obtener todos los usuarios (Admin)
            public async Task<List<UsuarioDTO>> ObtenerTodosAsync()
            {
                return await _context.Usuarios
                    .Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Email = u.Email,
                        Rol = u.Rol,
                        FechaRegistro = u.FechaRegistro,
                        Activo = u.Activo
                    }).ToListAsync();
            }

            // Obtener usuario por ID
            public async Task<UsuarioDTO?> ObtenerPorIdAsync(int id)
            {
                var u = await _context.Usuarios.FindAsync(id);
                if (u == null) return null;

                return new UsuarioDTO
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    Rol = u.Rol,
                    FechaRegistro = u.FechaRegistro,
                    Activo = u.Activo
                };
            }

            // Obtener solo clientes (Admin y Entrenador)
            public async Task<List<UsuarioDTO>> ObtenerClientesAsync()
            {
                return await _context.Usuarios
                    .Where(u => u.Rol == "Cliente" && u.Activo)
                    .Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Email = u.Email,
                        Rol = u.Rol,
                        FechaRegistro = u.FechaRegistro,
                        Activo = u.Activo
                    }).ToListAsync();
            }
            // Editar usuario (Admin)
            public async Task<UsuarioDTO?> EditarAsync(int id, EditarUsuarioDTO dto)
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null) return null;

                if (dto.Nombre != null) usuario.Nombre = dto.Nombre;
                if (dto.Email != null) usuario.Email = dto.Email;
                if (dto.Rol != null) usuario.Rol = dto.Rol;
                if (dto.Activo.HasValue) usuario.Activo = dto.Activo.Value;

                await _context.SaveChangesAsync();

                return new UsuarioDTO
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Rol = usuario.Rol,
                    FechaRegistro = usuario.FechaRegistro,
                    Activo = usuario.Activo
                };
            }
            // Editar perfil propio (Cliente o Entrenador)
            public async Task<UsuarioDTO?> EditarPerfilAsync(int id, EditarPerfilDTO dto)
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null) return null;

                if (dto.Nombre != null) usuario.Nombre = dto.Nombre;
                if (dto.Email != null) usuario.Email = dto.Email;
                if (dto.NuevaPassword != null)
                    usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);

                await _context.SaveChangesAsync();

                return new UsuarioDTO
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Rol = usuario.Rol,
                    FechaRegistro = usuario.FechaRegistro,
                    Activo = usuario.Activo
                };
            }
            // Eliminar usuario (Admin)
            public async Task<bool> EliminarAsync(int id)
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null) return false;

                // Soft delete — solo desactiva
                usuario.Activo = false;
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
