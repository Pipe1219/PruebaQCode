using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_QCode.Context;
using API_QCode.Models;
using API_QCode.Servicios;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Identity.Data;

namespace API_QCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly DbContext_QCode _context;

        public EmpleadosController(DbContext_QCode context)
        {
            _context = context;
        }

        // GET: api/Empleados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            return await _context.Empleados.ToListAsync();
        }

        // GET: api/Empleados/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(Guid id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
            {
                return NotFound();
            }

            return empleado;
        }

        // PUT: api/Empleados/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpleado(Guid id, Empleado empleado)
        {
            if (id != empleado.EmpleadoId)
            {
                return BadRequest();
            }

            var register_key = _context.Llave_Contrasenas.FirstOrDefault();

            if (register_key == null)
            {
                return NotFound();
            }


            empleado.Contra = Encript_Desencript.EncriptarContrasena(empleado.Contra, register_key.Llave);

            _context.Entry(empleado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(id))
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

        // POST: api/Empleados
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Empleado>> PostEmpleado(Empleado empleado)
        {
            var register_key = _context.Llave_Contrasenas.FirstOrDefault();

            if (register_key == null)
            {
                return NotFound();
            }

            empleado.Contra = Encript_Desencript.EncriptarContrasena(empleado.Contra, register_key.Llave);

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmpleado", new { id = empleado.EmpleadoId }, empleado);
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] Models.LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Usuario) || string.IsNullOrEmpty(request.Contra))
            {
                return BadRequest("Usuario y contraseña son requeridos.");
            }

            ValidacionEmpleado validadorEmp = new ValidacionEmpleado(_context);

            bool esValido = validadorEmp.ValidarEmpleado(request.Usuario, request.Contra);

            if (esValido)
            {
                return Ok("Login exitoso.");
            }
            else
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }
        }

        // DELETE: api/Empleados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(Guid id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpleadoExists(Guid id)
        {
            return _context.Empleados.Any(e => e.EmpleadoId == id);
        }
    }
}
