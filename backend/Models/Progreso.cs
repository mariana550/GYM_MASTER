// Models/Progreso.cs
using System.ComponentModel.DataAnnotations;

namespace PROYECTO_GYM_MASTER.Models
{
    public class Progreso
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public double Peso { get; set; }        // kg
        public double Altura { get; set; }      // cm
        public double PorcentajeGrasa { get; set; }

        [MaxLength(500)]
        public string Notas { get; set; } = string.Empty;
    }
}
