
using System.ComponentModel.DataAnnotations;

namespace P01_2022ML601.Models
{
    public class usuario
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Contrasena { get; set; }

        public string Rol { get; set; } //cliente o empleado

    }
}
