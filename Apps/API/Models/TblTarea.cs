using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TblTarea
    {
        public int IdTareaPk { get; set; }

        [MaxLength(100), Required]
        public string Titulo { get; set; }

        public string? Descripcion { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime FechaLimite { get; set; }

        [Required]
        public int ColaboradorFk { get; set; }

        [Required]
        public int EstadoTareaFk { get; set; }

        public bool Estado { get; set; } = true;
    }
}
