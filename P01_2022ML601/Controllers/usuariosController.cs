using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022ML601.Models;
using Microsoft.EntityFrameworkCore;

namespace P01_2022ML601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly parqueoContext _context;
        public usuariosController(parqueoContext context)
        {
            _context = context;


        }
        //consultar usuarios
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var usuarios = (from u in _context.usuario
                            select u).ToList();
            if (usuarios.Count == 0)
            {
                return NotFound();
            }
            return Ok(usuarios);
        }
        // consulatr por id
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var usuario = _context.usuario.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        //agregar usuario
        [HttpPost]
        [Route("Add")]
        public IActionResult agrega([FromBody] usuario usuario)
        {
            try
            {
                _context.usuario.Add(usuario);
                _context.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //actualizar usuario
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] usuario usuarioModificar)
        {
            var usuarioActual = _context.usuario.FirstOrDefault(u => u.Id == id);
            if (usuarioActual == null)
            {
                return NotFound();
            }

            usuarioActual.Nombre = usuarioModificar.Nombre;
            usuarioActual.Correo = usuarioModificar.Correo;
            usuarioActual.Telefono = usuarioModificar.Telefono;
            usuarioActual.Contrasena = usuarioModificar.Contrasena;
            usuarioActual.Rol = usuarioModificar.Rol;

            _context.Entry(usuarioActual).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(usuarioModificar);
        }
        //eliminar usuario
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            var usuario = _context.usuario.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.usuario.Remove(usuario);
            _context.SaveChanges();

            return Ok(usuario);
        }
    }
}
