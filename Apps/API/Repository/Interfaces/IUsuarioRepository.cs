using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<ColaboradorDto>> ObtenerColaboradoresActivosAsync();
        Task<IEnumerable<UsuarioDto>> ObtenerUsuarioPorAlias(string alias);
    }
}
