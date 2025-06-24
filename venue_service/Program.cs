using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using venue_service.Src.Contexts;
using venue_service.Src.Middlewares;
using venue_service.Src.Config;
using venue_service.Src.Services.ImageStorageService;
using Microsoft.Extensions.Options;
using venue_service.Src.Services.Auth;
using venue_service.Src.Services.Venue;
using venue_service.Src.Services.User;
using venue_service.Src.Interfaces.VenueInterfaces;
using venue_service.Src.Interfaces.PaymentInterfaces;
using venue_service.Src.Interfaces.ImageStorageInterfaces;
using venue_service.Src.Interfaces.SportInterfaces;
using venue_service.Src.Services.Sport;
using venue_service.Src.Interfaces.AvailableTimesInterfaces;
using venue_service.Src.Services.Schedules;

var builder = WebApplication.CreateBuilder(args);

// Configuração dos arquivos JSON
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddHttpContextAccessor();

var accessToken = builder.Configuration["MercadoPago:AccessToken"];

// Ambiente e Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseApiKey = builder.Configuration["Supabase:ApiKey"];

Console.WriteLine($"🧪 ENVIRONMENT: {builder.Environment.EnvironmentName}");
Console.WriteLine($"📦 Connection string lida: {connectionString}");
Console.WriteLine($"🔗 Supabase URL: {supabaseUrl}");
Console.WriteLine($"🔑 Supabase API Key: {(string.IsNullOrEmpty(supabaseApiKey) ? "NÃO CONFIGURADA" : "***")}");

// Contexto do banco de dados
builder.Services.AddDbContext<VenueContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<ReservationContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddDbContext<SportContext>(options =>
    options.UseNpgsql(connectionString));

// Controllers
builder.Services.AddControllers();

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

// Configurações customizadas via IOptions<T>
builder.Services.Configure<SupabaseStorageOptions>(
    builder.Configuration.GetSection("Supabase")
);

// Injeção de Services (respeitando dependências)
//builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IVenueType, VenueTypeService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ISportInterface, SportService>();
builder.Services.AddScoped<IAvailableTimesService, VenueAvailableTimesService>();

// Storage de Imagens

// HttpClient (para Supabase)
builder.Services.AddHttpClient<IStorageService, SupabaseStorageService>((serviceProvider, client) =>
{
    var config = serviceProvider
        .GetRequiredService<IOptions<SupabaseStorageOptions>>().Value;

    if (string.IsNullOrWhiteSpace(config.Url))
        throw new InvalidOperationException("Supabase Url não configurada.");

    client.BaseAddress = new Uri(config.Url);
});


// Build da aplicação
var app = builder.Build();

// Migrações do banco
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<VenueContext>().Database.Migrate();
    scope.ServiceProvider.GetRequiredService<UserContext>().Database.Migrate();
    scope.ServiceProvider.GetRequiredService<ReservationContext>().Database.Migrate();
    scope.ServiceProvider.GetRequiredService<SportContext>().Database.Migrate();
}


// Middleware de erro global
app.UseMiddleware<ErrorHandlingMiddleware>();

// Removido Swagger
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// Segurança
app.UseAuthentication();
app.UseAuthorization();

// CORS liberado (ajuste no futuro para produção!)
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

// Mapear endpoints dos Controllers
app.MapControllers();

// Rodar aplicação
app.Run();
