using Models;
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
        Task CrearTareaAsync(TblTarea nuevaTarea);
        Task<bool> ActualizarTareaAsync(int id, TblTarea tareaActualizada);
    }
}
