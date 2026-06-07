using System.ComponentModel.DataAnnotations;

namespace PROYECTO_GYM_MASTER.DTOs
{
    public class CrearProgresoDTO
    {
        [Range(30, 300, ErrorMessage = "Peso debe estar entre 30 y 300 kg")]
        public double Peso { get; set; }

        [Range(100, 250, ErrorMessage = "Altura debe estar entre 100 y 250 cm")]
        public double Altura { get; set; }

        [Range(0, 100, ErrorMessage = "Porcentaje de grasa debe estar entre 0 y 100")]
        public double PorcentajeGrasa { get; set; }

        [MaxLength(500)]
        public string Notas { get; set; } = string.Empty;
    }
    public class ProgresoResponseDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public double Peso { get; set; }
        public double Altura { get; set; }
        public double PorcentajeGrasa { get; set; }
        public double IMC { get; set; }
        public string Notas { get; set; } = string.Empty;
    }
}
