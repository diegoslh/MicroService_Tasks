using API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
    {
        [HttpGet("ObtenerColaboradores")]
        public async Task<IActionResult> ObtenerColaboradores()
        {
            try
            {
                var colaboradores = await usuarioService.ObtenerColaboradoresAsync();
                return Ok(colaboradores); // StatusCode 200
            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Error al obtener los colaboradores.", HttpContext);
            }
        }

        [HttpGet("AutenticarUsuario")]
        public async Task<IActionResult> AutenticarUsuario(string usuario, string clave)
        {
            try
            {
                var usuarioInfo = await usuarioService.VerificarCredencialesUsuario(usuario, clave);
                
                if (usuarioInfo == null)
                {
                    return Unauthorized(new { message = "⛔ Credenciales inválidas" }); // StatusCode 401
                }

                return Ok(new { message = "✅ Usuario autenticado correctamente!" }); // StatusCode 200
            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Error al autenticar el usuario.", HttpContext);
            }
        }

    }
}
