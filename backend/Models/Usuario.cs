// Models/Usuario.cs
using System.ComponentModel.DataAnnotations;

namespace PROYECTO_GYM_MASTER.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = "Cliente"; // "Admin", "Entrenador", "Cliente"

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        // Relaciones
        public ICollection<Progreso> Progresos { get; set; } = new List<Progreso>();
        public RutinaAsignada? RutinaAsignada { get; set; }
    }
}
