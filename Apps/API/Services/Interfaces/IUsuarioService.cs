using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<ColaboradorDto>> ObtenerColaboradoresAsync();
        Task<UsuarioDto?> VerificarCredencialesUsuario(string alias, string clave);
    }
}
