using Microsoft.AspNetCore.Mvc;
using gymapiweb.Data;
using gymapiweb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace gymapiweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntrenadoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EntrenadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Entrenadores


        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntrenadorDto>>> GetEntrenadores()
        {
            var entrenadores = await _context.Entrenadores.ToListAsync();

            var entrenadoresDto = entrenadores.Select(entrenador => new EntrenadorDto
            {
                EntrenadorID = entrenador.EntrenadorID,
                Nombre = entrenador.Nombre,
                Apellido = entrenador.Apellido,
                Sexo = entrenador.Sexo,
                Especialidad = entrenador.Especialidad,
                Telefono = entrenador.Telefono,
                CorreoElectronico = entrenador.CorreoElectronico,
                FechaContratacion = entrenador.FechaContratacion,
                // Convierte el campo Foto (binario) a FotoBase64 (cadena) si no es nulo
                FotoBase64 = entrenador.Foto != null ? Convert.ToBase64String(entrenador.Foto) : null
            }).ToList();

            return entrenadoresDto;
        }




        // GET: api/Entrenadores/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Entrenador>> GetEntrenador(Guid id)
        {
            var entrenador = await _context.Entrenadores.FindAsync(id);

            if (entrenador == null)
            {
                return NotFound();
            }

            return entrenador;
        }

        // POST: api/Entrenadores
// POST: api/Entrenadores
        [HttpPost]
        public async Task<ActionResult<Entrenador>> PostEntrenador([FromBody] Entrenador entrenador)
        {
            try
            {
                entrenador.EntrenadorID = Guid.NewGuid(); // Genera un nuevo ID único
                _context.Entrenadores.Add(entrenador);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEntrenador), new { id = entrenador.EntrenadorID }, entrenador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }



        // PUT: api/Entrenadores/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntrenador(Guid id, [FromBody] Entrenador entrenador)
        {
            if (id != entrenador.EntrenadorID)
            {
                return BadRequest();
            }

            _context.Entry(entrenador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntrenadorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Entrenadores/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntrenador(Guid id)
        {
            var entrenador = await _context.Entrenadores.FindAsync(id);
            if (entrenador == null)
            {
                return NotFound();
            }

            _context.Entrenadores.Remove(entrenador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntrenadorExists(Guid id)
        {
            return _context.Entrenadores.Any(e => e.EntrenadorID == id);
        }
    }
}
