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
            if (string.IsNullOrWhiteSpace(tarea.Titulo))
                throw new ArgumentException("El título no puede estar vacío.");

            var validarFechaCreacion = accion == "crear" ? (tarea.FechaCreacion < DateTime.Now) : false;
            if (validarFechaCreacion)
                throw new ArgumentException("La fecha de creación no puede ser menor al día actual.");

            var validacionFechaLimite = accion == "actualizar" ? (tarea.FechaLimite.Date < DateTime.Now) : (tarea.FechaLimite.Date <= tarea.FechaCreacion.Date);
            if (validacionFechaLimite)
                throw new ArgumentException("La fecha límite debe ser al menos un día después de la fecha de creación.");

            if (tarea.ColaboradorFk <= 0)
                throw new ArgumentException("ID de colaborador inválido.");

            if (tarea.EstadoTareaFk <= 0)
                throw new ArgumentException("ID de estado de tarea inválido.");
        }
        #endregion
    }
}
