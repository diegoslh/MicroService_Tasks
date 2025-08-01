using Repository;
using Repository.Interfaces;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure services for the application.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the connection string for SQL Server
var connectionString = builder.Configuration.GetConnectionString("LocalConnection");
builder.Services.AddScoped<ISqlServerConnection>(provider =>
    new SqlServerConnection(connectionString!)
);

// Services added to the container.
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEstadoTareaRepository, EstadoTareaRepository>();
builder.Services.AddScoped<IEstadoTareaService, EstadoTareaService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("TasksCorsPolicy", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Enable Swagger documentation in development mode.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("TasksCorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
