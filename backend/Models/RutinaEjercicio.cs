// Models/RutinaEjercicio.cs
namespace PROYECTO_GYM_MASTER.Models
{
    public class RutinaEjercicio
    {
        public int Id { get; set; }

        public int RutinaId { get; set; }
        public Rutina Rutina { get; set; } = null!;

        public int EjercicioId { get; set; }
        public Ejercicio Ejercicio { get; set; } = null!;

        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public int DescansoSegundos { get; set; }
        public string Dia { get; set; } = string.Empty; // "Lunes", "Martes", etc.
    }
}
