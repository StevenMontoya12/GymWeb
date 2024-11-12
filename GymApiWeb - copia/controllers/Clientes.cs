using gymapiweb.Data;
using gymapiweb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClientesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/clientes
    [HttpGet]
    public IActionResult GetClientes()
    {
        // Excluir el campo Password en la respuesta del GET
        var clientes = _context.Clientes
            .Select(c => new
            {
                c.ClienteID,
                c.Nombre,
                c.Apellido,
                c.Sexo,
                c.FechaNacimiento,
                c.Telefono,
                c.CorreoElectronico,
                c.Direccion
            })
            .ToList();

        if (!clientes.Any())
        {
            return NotFound("No se encontraron clientes.");
        }

        return Ok(clientes);
    }

    // POST: api/clientes
    [HttpPost]
    public IActionResult CreateCliente([FromBody] Cliente cliente)
    {
        if (cliente == null || !ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            Console.WriteLine("Errores en el modelo de cliente: " + string.Join(", ", errors));
            return BadRequest(new { message = "Los datos del cliente no son válidos o están incompletos.", errors });
        }

        Console.WriteLine("Registrando nuevo cliente: " + cliente.Nombre);

        cliente.ClienteID = Guid.NewGuid();
        _context.Clientes.Add(cliente);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetClientes), new { id = cliente.ClienteID }, cliente);
    }

    // PUT: api/clientes/{id}
 // PUT: api/clientes/{id}
[HttpPut("{id}")]
public IActionResult UpdateCliente(Guid id, [FromBody] Cliente cliente)
{
    if (id != cliente.ClienteID)
    {
        return BadRequest("El ID del cliente no coincide.");
    }

    if (!ModelState.IsValid)
    {
        return BadRequest("Datos del cliente no válidos.");
    }

    var clienteExistente = _context.Clientes.Find(id);
    if (clienteExistente == null)
    {
        return NotFound("Cliente no encontrado.");
    }

    Console.WriteLine("Actualizando cliente: " + cliente.Nombre);

    // Actualizar los campos del cliente excepto Password si no está vacío
    clienteExistente.Nombre = cliente.Nombre;
    clienteExistente.Apellido = cliente.Apellido;
    clienteExistente.Sexo = cliente.Sexo;
    clienteExistente.FechaNacimiento = cliente.FechaNacimiento;
    clienteExistente.Telefono = cliente.Telefono;
    clienteExistente.CorreoElectronico = cliente.CorreoElectronico;
    clienteExistente.Direccion = cliente.Direccion;

    // Solo actualiza la contraseña si se ha ingresado una nueva
    if (!string.IsNullOrEmpty(cliente.Password))
    {
        clienteExistente.Password = cliente.Password;
    }

    _context.SaveChanges();

    return NoContent();
}


    // DELETE: api/clientes/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteCliente(Guid id)
    {
        var cliente = _context.Clientes.Find(id);
        if (cliente == null)
        {
            return NotFound("Cliente no encontrado.");
        }

        Console.WriteLine("Eliminando cliente con ID: " + id);

        _context.Clientes.Remove(cliente);
        _context.SaveChanges();

        return NoContent();
    }
}
