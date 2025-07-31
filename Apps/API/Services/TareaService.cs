using Models;
using Models.DTOs;
using Repository;
using Repository.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TareaService(ITareaRepository tareaRepository) : ITareaService
    {
        public async Task<IEnumerable<TareaDto>> ObtenerTareas(bool fullPayload, bool? estado = null)
        {
            // Trae todas las tareas con el payload completo si fullPayload es true.
            if (fullPayload == true)
            {
                //Si @soloActivas = true → solo tareas activas.
                //Si @soloActivas = false → solo inactivas.
                //Si @soloActivas = NULL → trae todas.
                var parametros = new Dictionary<string, object?>
                {
                    { "@soloActivas", estado.HasValue ? estado : DBNull.Value }
                };

                return await tareaRepository.ObtenerFullPayloadTareas(parametros);
            }
            // Si fullPayload es false, solo trae las tareas activas con información muy básica.
            else
            {
                return await tareaRepository.ObtenerTareasActivas();
            }
        }

        public async Task CrearTareaAsync(TblTarea nuevaTarea)
        {
            // Validaciones lógicas adicionales a las que tiene el Modelo TblTarea
            if (string.IsNullOrWhiteSpace(nuevaTarea.Titulo))
                throw new ArgumentException("El título no puede estar vacío.");

            if (nuevaTarea.FechaCreacion < DateTime.Now)
                throw new ArgumentException("La fecha de creación no puede ser menor al día actual.");

            if (nuevaTarea.FechaLimite.Date <= nuevaTarea.FechaCreacion.Date)
                throw new ArgumentException("La fecha límite debe ser al menos un día después de la fecha de creación.");

            if (nuevaTarea.ColaboradorFk <= 0)
                throw new ArgumentException("ID de colaborador inválido.");

            if (nuevaTarea.EstadoTareaFk <= 0)
                throw new ArgumentException("ID de estado de tarea inválido.");

            await tareaRepository.CrearTareaAsync(nuevaTarea);
        }

    }
}
