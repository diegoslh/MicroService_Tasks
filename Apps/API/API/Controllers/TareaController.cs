using API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Repository.Interfaces;
using Services;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TareaController(ITareaService tareaService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ObtenerPayloadTareas([FromQuery] bool fullPayload, [FromQuery] bool? estado)
        {
            try
            {
                var tareas = await tareaService.ObtenerTareas(fullPayload, estado);
                return Ok(tareas); // StatusCode 200
            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Error al obtener los tareas.", HttpContext);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CrearTarea([FromBody] TblTarea nuevaTarea)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await tareaService.CrearTareaAsync(nuevaTarea);
                return StatusCode(201, "Tarea creada exitosamente.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Ocurrió un error al crear la tarea.", HttpContext);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarTarea(int id, [FromBody] TblTarea tareaActualizada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var actualizada = await tareaService.ActualizarTareaAsync(id, tareaActualizada);

                if (!actualizada)
                    return NotFound(new { mensaje = $"No se encontró ninguna tarea con ID {id}." });

                return Ok(new { mensaje = "Tarea actualizada correctamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Error interno al actualizar la tarea.", HttpContext);
            }
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool nuevoEstado)
        {
            try
            {
                var actualizado = await tareaService.CambiarEstadoTareaAsync(id, nuevoEstado);

                if (!actualizado)
                    return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}." });

                return Ok(new { mensaje = $"El estado de la tarea fue actualizado a {(nuevoEstado ? "Activa" : "Inactiva")}." });
            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Ocurrió un error al actualizar el estado.", HttpContext);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            try
            {
                var eliminada = await tareaService.EliminarTareaAsync(id);

                if (!eliminada)
                    return NotFound(new { mensaje = $"No se encontró la tarea con ID {id}." });

                return Ok(new { mensaje = $"La tarea con ID {id} fue eliminada correctamente." });

            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Error interno al intentar eliminar la tarea.", HttpContext);
            }
        }
    }
}
