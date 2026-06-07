
// Models/Entrenador.cs
using System.ComponentModel.DataAnnotations;

namespace PROYECTO_GYM_MASTER.Models
{
    public class Entrenador
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        [MaxLength(200)]
        public string Especialidad { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        // Relaciones
        public ICollection<Rutina> Rutinas { get; set; } = new List<Rutina>();
    }
}