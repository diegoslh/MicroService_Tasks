using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EstadosTareaController(IEstadoTareaService estadoTareaService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ObtenerEstados()
        {
            try
            {
                var estados = await estadoTareaService.ListarEstadosActivosAsync();
                return Ok(estados);
            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Ocurrió un error al obtener los estados de tarea.", HttpContext);
            }
        }
    }
}
