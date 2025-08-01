using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;


namespace API.Filter
{
    public class AuthOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Revisa si el endpoint tiene [AllowAnonymous]
            var allowAnonymous = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AllowAnonymousAttribute>()
                .Any();

            // Revisa si el endpoint o controlador tiene [Authorize]
            var hasAuthorize = context.MethodInfo
                .DeclaringType!
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Any()
                || context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Any();

            // No requiere autenticación
            if (!hasAuthorize || allowAnonymous)
                return;

            // Si tiene [Authorize], añade el requisito
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
        }
    }
}
