using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_QCode.Models
{
    public class Vehiculo
    {
        public Guid VehiculoId { get; set; }

        [StringLength(20)]
        public required string Marca { get; set; }

        [StringLength(20)]
        public required string Modelo { get; set; }
        public required int AnoFabricacion { get; set; }

        [StringLength(10)]
        public required string Matricula { get; set; }

        [StringLength(100)]
        public required string Problema { get; set; }

        [StringLength(200)]
        public required string DescripcionProblema { get; set; }

        public double Valor { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int Estado { get; set; }
    }
}
