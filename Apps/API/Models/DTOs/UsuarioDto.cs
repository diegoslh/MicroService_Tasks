using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string? Alias { get; set; }
        public string? Rol { get; set; }
        public string? Clave { get; set; }
    }

}
