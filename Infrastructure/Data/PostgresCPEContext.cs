using Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PostgresCPEContext : DbContext
    {
        public DbSet<Movimiento> movimientos { get; set; } = null!;
        public DbSet<Pedido> pedidos { get; set; } = null!;
        public PostgresCPEContext(DbContextOptions<PostgresCPEContext> options) : base(options)
        {
        }
    }
}
