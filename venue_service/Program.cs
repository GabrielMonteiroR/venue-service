using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using venue_service.Src.Contexts;
using venue_service.Src.Middlewares;
using venue_service.Src.Services;
using Src.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


// Pegando a connection string do appsettings
var connectionString = builder.Configuration["DefaultConnection"];

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(connectionString)
);

// Injetando os Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();

// Injetando os Services
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IVenueAvaliabilityTime, VenueAvaliabilityTimeService>();
builder.Services.AddScoped<IVenueType, VenueTypeService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate(); 
}


// Middleware de tratamento de erros
app.UseMiddleware<ErrorHandlingMiddleware>();

// Swagger ativado somente em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ativando autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento de Controllers
app.MapControllers();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();
