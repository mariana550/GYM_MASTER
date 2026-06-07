using System.ComponentModel.DataAnnotations;

namespace PROYECTO_GYM_MASTER.DTOs
{
    public class RutinaDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El objetivo es obligatorio")]
        public string Objetivo { get; set; } = string.Empty;

        [Range(1, 52, ErrorMessage = "La duración debe ser entre 1 y 52 semanas")]
        public int DuracionSemanas { get; set; }
    }
    public class EditarRutinaDTO
    {
        [MaxLength(150)]
        public string? Nombre { get; set; }

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        public string? Objetivo { get; set; }

        [Range(1, 52)]
        public int? DuracionSemanas { get; set; }
    }
    public class RutinaResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Objetivo { get; set; } = string.Empty;
        public int DuracionSemanas { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string NombreEntrenador { get; set; } = string.Empty;
        public int EntrenadorId { get; set; }
    }
    public class AsignarRutinaDTO
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int RutinaId { get; set; }
    }
}
