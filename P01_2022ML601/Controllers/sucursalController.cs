using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022ML601.Models;
using Microsoft.EntityFrameworkCore;

namespace P01_2022ML601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sucursalController : ControllerBase
    {
        private readonly parqueoContext _context;
        public sucursalController(parqueoContext context)
        {
            _context = context;


        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var sucursales = _context.sucursal.ToList();
            if (sucursales.Count == 0)
            {
                return NotFound();
            }
            return Ok(sucursales);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var sucursal = _context.sucursal.FirstOrDefault(s => s.Id == id);
            if (sucursal == null)
            {
                return NotFound();
            }
            return Ok(sucursal);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult agregar([FromBody] sucursal sucursal)
        {
            try
            {
                _context.sucursal.Add(sucursal);
                _context.SaveChanges();
                return Ok(sucursal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] sucursal sucursalModificar)
        {
            var sucursalActual = _context.sucursal.FirstOrDefault(s => s.Id == id);
            if (sucursalActual == null)
            {
                return NotFound();
            }

            sucursalActual.Nombre = sucursalModificar.Nombre;
            sucursalActual.Direccion = sucursalModificar.Direccion;
            sucursalActual.Telefono = sucursalModificar.Telefono;
            sucursalActual.Administrador = sucursalModificar.Administrador;
            sucursalActual.NumeroEspacios = sucursalModificar.NumeroEspacios;

            _context.Entry(sucursalActual).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(sucursalModificar);
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            var sucursal = _context.sucursal.FirstOrDefault(s => s.Id == id);
            if (sucursal == null)
            {
                return NotFound();
            }

            _context.sucursal.Remove(sucursal);
            _context.SaveChanges();

            return Ok(sucursal);
        }
    }
}
