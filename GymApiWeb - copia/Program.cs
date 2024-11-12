using gymapiweb.Data;
using gymapiweb.Services; // Asegúrate de incluir el espacio de nombres de AuthService
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

// Configura CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5067") // URL del frontend Blazor
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Permitir credenciales si las necesitas
    });
});

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// Registrar AuthService en el contenedor de dependencias
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Usa la política de CORS para permitir solicitudes desde el frontend Blazor
app.UseCors("AllowBlazorFrontend");

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usa autenticación en el pipeline
app.UseAuthentication();

// Middleware personalizado para verificar roles y restringir acceso
app.Use(async (context, next) =>
{
    var userRole = context.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
    if (userRole == "Cliente" && context.Request.Path.StartsWithSegments("/empleado"))
    {
        context.Response.Redirect("/acceso-denegado");
    }
    else
    {
        await next();
    }
});

// Usa autorización
app.UseAuthorization();

app.MapControllers();

app.Run();
