using Repository;
using Repository.Interfaces;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure services for the application.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get the connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("LocalConnection");

// Services added to the container.
//builder.Services.AddScoped<ISqlServerConnection, SqlServerConnection>();
builder.Services.AddScoped<ISqlServerConnection>(provider =>
    new SqlServerConnection(connectionString!)
);
builder.Services.AddScoped<ITasksService, TasksService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
