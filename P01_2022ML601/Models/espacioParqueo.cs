
using System.ComponentModel.DataAnnotations;

namespace P01_2022ML601.Models
{
    public class espacioParqueo
    {
        [Key]
        public int Id { get; set; }
        public ICollection<reserva> Reservas { get; set; }
        public int SucursalId { get; set; }
        public int NumeroEspacio { get; set; }
        public string Ubicacion { get; set; }
        public decimal CostoHora { get; set; }
        public string Estado { get; set; } //disponible o ocupado
    }
}
