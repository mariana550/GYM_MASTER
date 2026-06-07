using System.ComponentModel.DataAnnotations;

namespace PROYECTO_GYM_MASTER.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
    }
    public class EditarUsuarioDTO
    {
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Rol { get; set; }

        public bool? Activo { get; set; }
    }
    public class EditarPerfilDTO
    {
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        public string? NuevaPassword { get; set; }
    }
}
