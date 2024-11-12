using gymapiweb.Data;
using gymapiweb.Models;
using gymapiweb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace gymapiweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(AuthService authService, ILogger<LoginController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.CorreoElectronico) || string.IsNullOrEmpty(request.Password))
            {
                _logger.LogWarning("Correo electrónico o contraseña faltante en la solicitud");
                return BadRequest(new { message = "Correo electrónico y contraseña son requeridos" });
            }

            _logger.LogInformation("Intentando iniciar sesión con el correo: {Correo}", request.CorreoElectronico);

            try
            {
                // Intenta iniciar sesión como empleado
                var empleado = await _authService.LoginEmpleadoAsync(request.CorreoElectronico, request.Password);
                if (empleado != null)
                {
                    _logger.LogInformation("Inicio de sesión exitoso para empleado con el correo: {Correo}", request.CorreoElectronico);

                    // Genera el token JWT con el rol de empleado
                    var token = _authService.GenerateJwtToken(request.CorreoElectronico, "Empleado", empleado.EmpleadoID);

                    return Ok(new { Role = "Empleado", Token = token });
                }

                // Intenta iniciar sesión como cliente
                var cliente = await _authService.LoginClienteAsync(request.CorreoElectronico, request.Password);
                if (cliente != null)
                {
                    _logger.LogInformation("Inicio de sesión exitoso para cliente con el correo: {Correo}", request.CorreoElectronico);

                    // Genera el token JWT con el rol de cliente
                    var token = _authService.GenerateJwtToken(request.CorreoElectronico, "Cliente", cliente.ClienteID);

                    return Ok(new { Role = "Cliente", Token = token });
                }

                // Si ambas autenticaciones fallan
                _logger.LogError("Inicio de sesión fallido para todas las cuentas con el correo: {Correo}", request.CorreoElectronico);
                return Unauthorized(new { message = "Credenciales inválidas" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el proceso de inicio de sesión para el correo: {Correo}", request.CorreoElectronico);
                return StatusCode(500, new { message = "Ocurrió un error en el servidor. Por favor, inténtelo de nuevo más tarde." });
            }
        }
    }

    public class LoginRequest
    {
        public string? CorreoElectronico { get; set; }
        public string? Password { get; set; }
    }
}
