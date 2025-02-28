
using System.ComponentModel.DataAnnotations;

namespace P01_2022ML601.Models
{
    public class sucursal
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int AdministradorId { get; set; }
        public usuario Administrador { get; set; }
        public int NumeroEspacios { get; set; }

    }
}
