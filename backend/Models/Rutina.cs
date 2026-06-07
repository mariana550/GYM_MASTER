// Models/Rutina.cs
using System.ComponentModel.DataAnnotations;

namespace PROYECTO_GYM_MASTER.Models
{
    public class Rutina
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public string Objetivo { get; set; } = string.Empty; // "Fuerza", "Cardio", "Pérdida de peso"

        public int DuracionSemanas { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        public int EntrenadorId { get; set; }
        public Entrenador Entrenador { get; set; } = null!;

        public ICollection<RutinaEjercicio> RutinaEjercicios { get; set; } = new List<RutinaEjercicio>();
        public ICollection<RutinaAsignada> RutinasAsignadas { get; set; } = new List<RutinaAsignada>();
    }
}
