using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITareaRepository
    {
        Task<IEnumerable<TareaDto>> ObtenerTareasActivas();
        Task<IEnumerable<TareaDto>> ObtenerFullPayloadTareas(Dictionary<string, object?> parametros);
        Task CrearTareaAsync(TblTarea tarea);
        Task<bool> ExisteTareaAsync(int id);
        Task ActualizarTareaAsync(TblTarea tarea);
        Task ActualizarEstadoTareaAsync(int id, bool estado);
        Task EliminarTareaAsync(int id);
    }
}
