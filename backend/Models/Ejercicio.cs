// Models/Ejercicio.cs
using System.ComponentModel.DataAnnotations;

namespace PROYECTO_GYM_MASTER.Models
{
    public class Ejercicio
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [MaxLength(100)]
        public string GrupoMuscular { get; set; } = string.Empty; // "Pecho", "Pierna", etc.

        // Relaciones
        public ICollection<RutinaEjercicio> RutinaEjercicios { get; set; } = new List<RutinaEjercicio>();
    }
}
