// Models/RutinaAsignada.cs
namespace PROYECTO_GYM_MASTER.Models
{
    public class RutinaAsignada
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int RutinaId { get; set; }
        public Rutina Rutina { get; set; } = null!;

        public DateTime FechaAsignacion { get; set; } = DateTime.UtcNow;
        public bool Activa { get; set; } = true;
    }
}
