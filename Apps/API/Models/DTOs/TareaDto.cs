using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class TareaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime FechaLimite { get; set; }
        public int? DiasRestantes { get; set; }
        public int ColaboradorFk { get; set; }
        public string Colaborador { get; set; }
        public string? Prioridad { get; set; }
        public int EstadoTareaFk { get; set; }
        public string EstadoTarea { get; set; }
        public string? EstadoRegistro { get; set; }
    }
}
