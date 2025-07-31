using API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    }
}
