using venue_service.Src.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Pegando a connection string do appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Injetando o DbContext com a connection string
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString)
);

// Controller
builder.Services.AddControllers();

// Swagger → antes do Build
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build do app
var app = builder.Build();

// Middleware de erro
app.UseMiddleware<venue_service.Src.Middlewares.ErrorHandlingMiddleware>();

// Swagger ativado
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Mapeamento de Controllers
app.MapControllers();

app.Run();
