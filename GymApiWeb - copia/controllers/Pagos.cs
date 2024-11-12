using gymapiweb.Data;
using gymapiweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gymapiweb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PagosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método GET para obtener todos los pagos
    [   HttpGet]
        public async Task<ActionResult<IEnumerable<PagoDTO>>> GetPagos()
        {
            try
            {
                var pagos = await _context.Pagos
                    .Include(p => p.Cliente)
                    .Include(p => p.Membresia)
                    .Include(p => p.Empleado)
                    .Select(p => new PagoDTO
                    {
                        PagoID = p.PagoID,
                        ClienteID = p.ClienteID,
                        MembresiaID = p.MembresiaID,
                        EmpleadoID = p.EmpleadoID,
                        MontoPagado = p.MontoPagado,
                        FechaPago = p.FechaPago,
                        ClienteNombreCompleto = p.Cliente != null ? $"{p.Cliente.Nombre} {p.Cliente.Apellido}" : "N/A",
                        MembresiaTipo = p.Membresia != null ? p.Membresia.TipoMembresia : "N/A",
                        EmpleadoNombre = p.Empleado != null ? p.Empleado.Nombre : "N/A"
                    })
                    .ToListAsync();

                return Ok(pagos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }


        // Método POST para crear un nuevo pago
        [HttpPost]
        public async Task<IActionResult> CrearPago([FromBody] Pago pago)
        {
            if (pago == null)
            {
                return BadRequest("Datos de pago inválidos");
            }

            try
            {
                var membresia = await _context.Membresias.FindAsync(pago.MembresiaID);
                if (membresia == null)
                {
                    return NotFound("La membresía especificada no existe");
                }

                pago.MontoPagado = membresia.Costo; // Establece el costo correcto
                pago.FechaPago = DateTime.Now;

                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Pago registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }

        // Método DELETE para eliminar un pago
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(Guid id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound("El pago especificado no existe");
            }

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Pago eliminado exitosamente" });
        }
    }

    // Clase DTO para enviar solo los datos necesarios al frontend
public class PagoDTO
{
    public Guid PagoID { get; set; }
    public Guid ClienteID { get; set; }
    public Guid MembresiaID { get; set; }
    public Guid? EmpleadoID { get; set; }
    public decimal MontoPagado { get; set; }
    public DateTime FechaPago { get; set; }
    public string ClienteNombreCompleto { get; set; } // Nuevo campo para el nombre completo del cliente
    public string MembresiaTipo { get; set; }
    public string? EmpleadoNombre { get; set; }
}

}
