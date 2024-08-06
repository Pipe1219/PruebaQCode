using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_QCode.Models
{
    public class Empleado
    {
        public Guid EmpleadoId { get; set; }

        [StringLength(20)]
        public required string Usuario { get; set; }

        [StringLength(250)]
        public required string Contra { get; set; }

    }
}
