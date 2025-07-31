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
        Task<IEnumerable<TareaDto>> ObtenerFullPayloadTareas(Dictionary<string, object?> parameters);
        Task<bool> CrearTarea(TareaDto tarea);
        Task<bool> ActualizarTarea(TareaDto tarea);
        Task<bool> InhabilitarTarea(int id);
    }
}
