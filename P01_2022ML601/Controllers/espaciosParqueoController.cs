using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022ML601.Models;
using Microsoft.EntityFrameworkCore;

namespace P01_2022ML601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class espaciosParqueoController : ControllerBase
    {
        private readonly parqueoContext _context;
        public espaciosParqueoController(parqueoContext context)
        {
            _context = context;


        }

        //consultar todo
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            var espacios = _context.espacioParqueo.ToList();
            if (espacios.Count == 0)
            {
                return NotFound();
            }
            return Ok(espacios);
        }
        //consultar por sucursal
        [HttpGet]
        [Route("GetBySucursal/{sucursalId}")]
        public IActionResult GetBySucursal(int sucursalId)
        {
            var espacios = _context.espacioParqueo.Where(e => e.SucursalId == sucursalId).ToList();
            if (espacios.Count == 0)
            {
                return NotFound();
            }
            return Ok(espacios);
        }
        //Espacios Disponible Por Dia
        [HttpGet]
        [Route("GetAvailableSpacesForDay")]
        public IActionResult EspaciosDisponiblePorDia(DateTime fecha)
        {
            var espaciosDisponibles = (from e in _context.espacioParqueo
                                       .Include(e => e.Reservas) // Incluir las reservas asociadas
                                       where e.Estado == "Disponible"
                                             && !e.Reservas.Any(r => r.FechaReserva.Date == fecha.Date) // Filtrar si no tiene reservas para esa fecha
                                       select e).ToList();

            if (espaciosDisponibles.Count == 0)
            {
                return NotFound("No hay espacios disponibles para el día indicado.");
            }

            return Ok(espaciosDisponibles);
        }


        //espacios Reservados Por dia
        [HttpGet]
        [Route("GetReservedSpacesByDay")]
        public IActionResult espaciosReservadosPordia(DateTime fecha)
        {
            var espaciosReservados = (from e in _context.espacioParqueo
                                      join r in _context.reserva on e.Id equals r.EspacioId
                                      where r.FechaReserva.Date == fecha.Date
                                      select new
                                      {
                                          r.Id,
                                          r.FechaReserva,
                                          r.HoraInicio,
                                          e.NumeroEspacio,
                                          e.Ubicacion,
                                          e.SucursalId
                                      }).ToList();

            if (espaciosReservados.Count == 0)
            {
                return NotFound();
            }

            return Ok(espaciosReservados);
        }
        //espacios Resevados Por Fechas
        [HttpGet]
        [Route("GetReservedSpacesByDateRange/{sucursalId}")]
        public IActionResult espaciosResevadosPorFechas(int sucursalId, DateTime startDate, DateTime endDate)
        {
            var espaciosReservados = (from e in _context.espacioParqueo
                                      join r in _context.reserva on e.Id equals r.EspacioId
                                      where e.SucursalId == sucursalId && r.FechaReserva >= startDate && r.FechaReserva <= endDate
                                      select new
                                      {
                                          r.Id,
                                          r.FechaReserva,
                                          r.HoraInicio,
                                          e.NumeroEspacio,
                                          e.Ubicacion
                                      }).ToList();

            if (espaciosReservados.Count == 0)
            {
                return NotFound();
            }

            return Ok(espaciosReservados);
        }

        //agregar espacio parqueo
        [HttpPost]
        [Route("Add")]
        public IActionResult agregar([FromBody] espacioParqueo espacio)
        {
            try
            {
                _context.espacioParqueo.Add(espacio);
                _context.SaveChanges();
                return Ok(espacio);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //actualizar espacio parqueo
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult actualizar(int id, [FromBody] espacioParqueo espacioModificar)
        {
            var espacioActual = _context.espacioParqueo.FirstOrDefault(e => e.Id == id);
            if (espacioActual == null)
            {
                return NotFound();
            }

            espacioActual.NumeroEspacio = espacioModificar.NumeroEspacio;
            espacioActual.Ubicacion = espacioModificar.Ubicacion;
            espacioActual.CostoHora = espacioModificar.CostoHora;
            espacioActual.Estado = espacioModificar.Estado;

            _context.Entry(espacioActual).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(espacioModificar);
        }
        //eliminar espacio parqueo
        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            var espacio = _context.espacioParqueo.FirstOrDefault(e => e.Id == id);
            if (espacio == null)
            {
                return NotFound();
            }

            _context.espacioParqueo.Remove(espacio);
            _context.SaveChanges();

            return Ok(espacio);
        }
    }
}
