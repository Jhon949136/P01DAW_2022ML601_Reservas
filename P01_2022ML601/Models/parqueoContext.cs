
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace P01_2022ML601.Models
{
    public class parqueoContext : DbContext
    {
        public parqueoContext(DbContextOptions<parqueoContext> options) : base(options)
        {

        }

        public DbSet<usuario> usuario { get; set; }
        public DbSet<sucursal> sucursal { get; set; }
        public DbSet<espacioParqueo> espacioParqueo { get; set; }
        public DbSet<reserva> reserva { get; set; }
    }
}
