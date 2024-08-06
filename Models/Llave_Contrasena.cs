using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_QCode.Models
{
    public class LlaveContrasena
    {
        public Guid LlaveContrasenaId { get; set; }
        public string Llave { get; set; }
    }
}
