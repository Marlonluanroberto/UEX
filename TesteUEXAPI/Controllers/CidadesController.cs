using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteUEXAPI.Data;
using TesteUEXAPI.Models;

namespace TesteUEXAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadesController : ControllerBase
    {
        private readonly teste_uexContext _context;

        public CidadesController(teste_uexContext context)
        {
            _context = context;
        }

        // GET: api/Cidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cidades>>> GetCidades()
        {
            return await _context.Cidades.ToListAsync();
        }

        [HttpPost]
        [Route("CarregarCidadePorEstado")]
        public async Task<ActionResult<IEnumerable<Cidades>>> CarregarCidadePorEstado(int id_estado)
        {
            return await _context.Cidades.Where(x => x.id_estado.Equals(id_estado)).ToListAsync();
        }
        // GET: api/Cidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cidades>> GetCidades(int id)
        {
            var cidades = await _context.Cidades.FindAsync(id);

            if (cidades == null)
                return NotFound();

            return cidades;
        }

        // PUT: api/Cidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCidades(int id, Cidades cidades)
        {
            if (id != cidades.id_cidade)
                return BadRequest();

            _context.Entry(cidades).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CidadesExists(id))
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

        // POST: api/Cidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Cidades>> PostCidades(Cidades cidades)
        //{
        //    _context.Cidades.Add(cidades);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCidades", new { id = cidades.id_cidade }, cidades);
        //}

        // DELETE: api/Cidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCidades(int id)
        {
            var cidades = await _context.Cidades.FindAsync(id);
            if (cidades == null)
            {
                return NotFound();
            }

            _context.Cidades.Remove(cidades);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CidadesExists(int id)
        {
            return _context.Cidades.Any(e => e.id_cidade == id);
        }
    }
}
