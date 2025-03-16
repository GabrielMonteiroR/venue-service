using venue_service.Src.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Pegando a connection string do appsettings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Injetando o DbContext com a connection string
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString)
);

// Restante das configurações
builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<venue_service.Src.Middlewares.ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
