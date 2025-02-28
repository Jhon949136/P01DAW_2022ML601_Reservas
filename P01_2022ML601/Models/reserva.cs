
using System.ComponentModel.DataAnnotations;

namespace P01_2022ML601.Models
{
    public class reserva
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public usuario Usuario { get; set; }
        public int EspacioId { get; set; }
        public espacioParqueo Espacio { get; set; }
        public DateTime FechaReserva { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public string Estado { get; set; } //activa o cancelada
    }
}
