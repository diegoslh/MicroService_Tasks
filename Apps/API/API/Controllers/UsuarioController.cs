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
        [HttpGet("Colaboradores")]
        public async Task<IActionResult> ObtenerColaboradores()
        {
            try
            {
                var colaboradores = await usuarioService.ObtenerColaboradoresAsync();
                return Ok(colaboradores);
            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Error al obtener los colaboradores.", HttpContext);
            }
        }

        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(string usuario, string clave)
        {
            try
            {
                var usuarioInfo = await usuarioService.VerificarCredencialesUsuario(usuario, clave);
                
                if (usuarioInfo == null)
                {
                    return Unauthorized(new { mensaje = "⛔ Credenciales inválidas" });
                }

                return Ok(new { mensaje = "✅ Usuario autenticado correctamente!" });
            }
            catch (Exception ex)
            {
                return ErrorHelper.BuildInternalError(ex, "✖️ Error al autenticar el usuario.", HttpContext);
            }
        }

    }
}
