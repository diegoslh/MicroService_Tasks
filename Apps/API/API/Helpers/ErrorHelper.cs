using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace API.Helpers
{
    public static class ErrorHelper
    {
        public static IActionResult BuildInternalError(Exception ex, string userMessage, HttpContext httpContext)
        {
            var env = httpContext?.RequestServices.GetService(typeof(IHostEnvironment)) as IHostEnvironment;

            var message = env != null && env.IsDevelopment()
                ? ex.Message
                : userMessage;

            var response = new ObjectResult(message)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            return response;
        }
    }
}