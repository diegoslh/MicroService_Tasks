using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITareaService
    {
        Task<IEnumerable<TareaDto>> ObtenerTareas(bool fullPayload, bool? estado);
    }
}
