using API.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Interfaces;
using Services;
using Services.Interfaces;
using System.Text;
//using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure services for the application.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// console.WriteLine("🟢 Swagger is running at: http://localhost:44357/swagger/index.html");

// Configuration to allow Authorizacion Header in Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tu API",
        Version = "v1"
    });

    // Security configuration for JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT en este formato: Bearer {token}"
    });

    // Configuration to filter [Authorize] decorator
    options.OperationFilter<AuthOperationFilter>();
});

// Configure the connection string for SQL Server
var connectionString = builder.Configuration.GetConnectionString("DockerConnection");
builder.Services.AddScoped<ISqlServerConnection>(provider =>
    new SqlServerConnection(connectionString!)
);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("TasksCorsPolicy", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add Authentication and Authorization with JWT Bearer
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
    };
});

// Services added to the container.
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEstadoTareaRepository, EstadoTareaRepository>();
builder.Services.AddScoped<IEstadoTareaService, EstadoTareaService>();

var app = builder.Build();

// Enable Swagger documentation in development mode.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("TasksCorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();