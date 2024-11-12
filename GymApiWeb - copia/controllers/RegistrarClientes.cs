// ClienteController.cs
using gymapiweb.Data;
using gymapiweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace gymapiweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(ApplicationDbContext context, ILogger<ClienteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCliente([FromBody] Cliente nuevoCliente)
        {
            if (nuevoCliente == null)
            {
                _logger.LogError("Intento de registro con datos vacíos.");
                return BadRequest("Los datos del cliente no son válidos.");
            }

            try
            {
                // Registrar el nuevo cliente en la base de datos usando el procedimiento almacenado
                await _context.RegistrarClienteAsync(nuevoCliente);
                _logger.LogInformation("Cliente registrado con éxito");
                return Ok(nuevoCliente);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al registrar el cliente: {ex.Message}");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
