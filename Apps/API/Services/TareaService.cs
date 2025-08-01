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
            // Validaciones adicionales
            ValidarCamposTblTarea(nuevaTarea, "crear");

            // Crear tarea
            await tareaRepository.CrearTareaAsync(nuevaTarea);
        }

        public async Task<bool> ActualizarTareaAsync(int id, TblTarea tareaActualizada)
        {
            // Validaciones adicionales
            ValidarCamposTblTarea(tareaActualizada, "actualizar");

            // Confirmar existencia
            var existe = await tareaRepository.ExisteTareaAsync(id);
            if (!existe)
                return false;

            // Asegurar ID correcto y procesar actualización
            tareaActualizada.IdTareaPk = id;
            await tareaRepository.ActualizarTareaAsync(tareaActualizada);

            return true;
        }

        public async Task<bool> CambiarEstadoTareaAsync(int id, bool nuevoEstado)
        {
            // Confirmar existencia
            var existe = await tareaRepository.ExisteTareaAsync(id);
            if (!existe)
                return false;

            // Actualizar estado de la tarea
            await tareaRepository.ActualizarEstadoTareaAsync(id, nuevoEstado);
            return true;
        }

        public async Task<bool> EliminarTareaAsync(int id)
        {
            var existe = await tareaRepository.ExisteTareaAsync(id);
            if (!existe)
                return false;

            await tareaRepository.EliminarTareaAsync(id);
            return true;
        }


        #region Private Methods
        private static void ValidarCamposTblTarea(TblTarea tarea, string accion)
        {
            // Validaciones adicionales a las que tiene el Modelo TblTarea
            var hoy = DateTime.Now.Date;
            var fechaCreacion = tarea.FechaCreacion.Date;
            var fechaLimite = tarea.FechaLimite.Date;

            if (string.IsNullOrWhiteSpace(tarea.Titulo))
                throw new ArgumentException("El título no puede estar vacío.");

            if (accion == "crear")
            {
                if (fechaCreacion < hoy)
                    throw new ArgumentException("La fecha de creación no puede ser anterior al día de hoy.");

                if (fechaLimite == fechaCreacion)
                    throw new ArgumentException("La fecha límite no puede ser el mismo día de la creación. Debe ser al menos al día siguiente.");

                if (fechaLimite < fechaCreacion)
                    throw new ArgumentException("La fecha límite debe ser posterior a la fecha de creación.");

                if ((fechaLimite - fechaCreacion).Days > 30)
                    throw new ArgumentException("La fecha límite no puede exceder los 30 días desde la fecha de creación.");
            }
            else if (accion == "actualizar")
            {
                if (fechaLimite < hoy)
                    throw new ArgumentException("La fecha límite no puede ser anterior al día de hoy.");

                if (fechaLimite <= fechaCreacion)
                    throw new ArgumentException("La fecha límite debe ser posterior a la fecha de creación.");

                if ((fechaLimite - hoy).Days > 30)
                    throw new ArgumentException("La fecha límite no puede exceder los 30 días desde la fecha actual.");
            }

            if (tarea.ColaboradorFk <= 0)
                throw new ArgumentException("ID de colaborador inválido.");

            if (tarea.EstadoTareaFk <= 0)
                throw new ArgumentException("ID de estado de tarea inválido.");
        }
        #endregion
    }
}
