using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022ML601.Models;
using Microsoft.EntityFrameworkCore;

namespace P01_2022ML601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly parqueoContext _context;
        public reservasController(parqueoContext context)
        {
            _context = context;


        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var reservas = (from r in _context.reserva
                            join u in _context.usuario on r.UsuarioId equals u.Id
                            join e in _context.espacioParqueo on r.EspacioId equals e.Id
                            where r.Estado == "Activa"
                            select new
                            {
                                r.Id,
                                r.FechaReserva,
                                r.HoraInicio,
                                r.Duracion,
                                Usuario = u.Nombre,
                                Espacio = e.NumeroEspacio,
                                e.Ubicacion,
                                e.SucursalId,
                                r.Estado
                            }).ToList();

            if (reservas.Count == 0)
            {
                return NotFound();
            }
            return Ok(reservas);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var reserva = _context.reserva.FirstOrDefault(r => r.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }
            return Ok(reserva);
        }

        [HttpGet]
        [Route("GetByUser/{usuarioId}")]
        public IActionResult GetByUser(int usuarioId)
        {
            var reservas = _context.reserva.Where(r => r.UsuarioId == usuarioId).ToList();
            if (reservas.Count == 0)
            {
                return NotFound();
            }
            return Ok(reservas);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult agregar([FromBody] reserva reserva)
        {
            try
            {
                var espacio = _context.espacioParqueo.FirstOrDefault(e => e.Id == reserva.EspacioId);

                if (espacio == null || espacio.Estado == "Ocupado")
                {
                    return BadRequest("Espacio no disponible...");
                }

                espacio.Estado = "Ocupado";
                reserva.Estado = "Activa";

                _context.reserva.Add(reserva);
                _context.SaveChanges();

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] reserva reservaModificar)
        {
            var reservaActual = _context.reserva.FirstOrDefault(r => r.Id == id);
            if (reservaActual == null)
            {
                return NotFound();
            }

            reservaActual.FechaReserva = reservaModificar.FechaReserva;
            reservaActual.HoraInicio = reservaModificar.HoraInicio;
            reservaActual.Duracion = reservaModificar.Duracion;

            _context.Entry(reservaActual).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(reservaModificar);
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            var reserva = _context.reserva.FirstOrDefault(r => r.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            var espacio = _context.espacioParqueo.FirstOrDefault(e => e.Id == reserva.EspacioId);
            if (espacio != null)
            {
                espacio.Estado = "Disponible";
            }

            _context.reserva.Remove(reserva);
            _context.SaveChanges();

            return Ok(reserva);
        }
    }
}
