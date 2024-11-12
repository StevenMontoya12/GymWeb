using Microsoft.AspNetCore.Mvc;
using gymapiweb.Data;
using gymapiweb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace gymapiweb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembresiasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembresiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Membresia>>> GetMembresias()
        {
            var membresias = await _context.Membresias.ToListAsync();
            return Ok(membresias);
        }
    }
}
