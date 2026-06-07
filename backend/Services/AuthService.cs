// Services/AuthService.cs
using Microsoft.EntityFrameworkCore;
using PROYECTO_GYM_MASTER.Data;
using PROYECTO_GYM_MASTER.DTOs;
using PROYECTO_GYM_MASTER.Helpers;
using PROYECTO_GYM_MASTER.Models;

namespace PROYECTO_GYM_MASTER.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthService(AppDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDTO?> RegistrarAsync(RegisterDTO dto)
        {
            // Verificar si el email ya existe
            var existe = await _context.Usuarios
                .AnyAsync(u => u.Email == dto.Email);

            if (existe) return null;

            // Capitalizar el rol correctamente
            var rolCapitalizado = dto.Rol switch
            {
                "admin" or "Admin" or "ADMIN" => "Admin",
                "entrenador" or "Entrenador" or "ENTRENADOR" => "Entrenador",
                _ => "Cliente"
            };

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Rol = dto.Rol,
                FechaRegistro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var token = _jwtHelper.GenerarToken(usuario);

            return new AuthResponseDTO
            {
                Token = token,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol,
                UsuarioId = usuario.Id
            };
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Activo);

            if (usuario == null) return null;

            // Verificar contraseña con BCrypt
            var passwordValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);

            if (!passwordValida) return null;

            var token = _jwtHelper.GenerarToken(usuario);

            return new AuthResponseDTO
            {
                Token = token,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol,
                UsuarioId = usuario.Id
            };
        }
    }
}
