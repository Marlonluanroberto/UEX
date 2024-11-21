using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteUEXAPI.Data;
using TesteUEXAPI.Models;

namespace TesteUEXAPI.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly teste_uexContext _context;

        public EstadosController(teste_uexContext context)
        {
            _context = context;
        }

        // GET: api/Estados
        [HttpGet]
        public JsonResult GetEstados()
        {
            var t =   _context.Estados.ToList();
                return new JsonResult(t);

        }

        // GET: api/Estados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estados>> GetEstados(int id)
        {
            var estados = await _context.Estados.FindAsync(id);

            if (estados == null)
            {
                return NotFound();
            }

            return estados;
        }

        // PUT: api/Estados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstados(int id, Estados estados)
        {
            if (id != estados.id_estado)
            {
                return BadRequest();
            }

            _context.Entry(estados).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadosExists(id))
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

        // POST: api/Estados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estados>> PostEstados(Estados estados)
        {
            _context.Estados.Add(estados);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstados", new { id = estados.id_estado }, estados);
        }

        // DELETE: api/Estados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstados(int id)
        {
            var estados = await _context.Estados.FindAsync(id);
            if (estados == null)
            {
                return NotFound();
            }

            _context.Estados.Remove(estados);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadosExists(int id)
        {
            return _context.Estados.Any(e => e.id_estado == id);
        }
    }
}
