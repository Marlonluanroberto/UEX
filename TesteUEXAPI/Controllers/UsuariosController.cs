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
    public class UsuariosController : ControllerBase
    {
        private readonly teste_uexContext _context;

        public UsuariosController(teste_uexContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios() => await _context.Usuarios.ToListAsync();

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuarios>> GetUsuarios(int id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);

            if (usuarios == null)
                return NotFound();

            return usuarios;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuarios(int id, Usuarios usuarios)
        {
            if (id != usuarios.id_usuario)
                return BadRequest();

            _context.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<string> PostUsuarios(Usuarios usuarios)
        {
            var existeLogin = _context.Usuarios.Where(x => x.CPF.Equals(usuarios.CPF)).ToList();

            if (existeLogin.Count > 0)
                return "Usuario já cadastrado";
            else
            {
                _context.Usuarios.Add(usuarios);
                var t = await _context.SaveChangesAsync();

                if (t.Equals(1))
                    return "Cadastro feito com o sucesso";
                else
                    return "Ocorreu algum problema";
            }

        }

        [HttpPost]
        [Route(nameof(RealizarLogin))]
        public async Task<string> RealizarLogin(Usuarios usuarios)
        {
            var existeCPF = _context.Usuarios.Where(x => x.nome.Equals(usuarios.nome) && x.senha.Equals(usuarios.senha)).ToList();
            if (existeCPF.Count > 0)
            {
                return "Logado Com Sucesso";
            }
            else
                return "Necessario fazer o cadastro do seu usuario ";

        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteUsuarios(int id)
        {
            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
                return "Usuario Não encontrado";

            _context.Usuarios.Remove(usuarios);
           var excluido =  await _context.SaveChangesAsync();
            if (excluido > 0)
            {
                return "Excluido Com Sucesso";
            }
            else
                return "Não foi possivel excluir o usuario ";
        }

        private bool UsuariosExists(int id)
        {
            return _context.Usuarios.Any(e => e.id_usuario == id);
        }
    }
}
