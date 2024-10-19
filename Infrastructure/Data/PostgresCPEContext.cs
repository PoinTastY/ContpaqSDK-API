using Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

namespace Infrastructure.Data
{
    public class PostgresCPEContext : DbContext
    {
        public DbSet<Movimiento> movimientos { get; set; } = null!;
        public DbSet<Pedido> pedidos { get; set; } = null!;
        public PostgresCPEContext(DbContextOptions<PostgresCPEContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movimiento>().ToTable("movimiento").HasKey(m => m.Id);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.IdPedido)
                .HasColumnName("id_pedido")
                .IsRequired(true);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.CodigoProducto)
                .HasColumnName("codigoproducto")
                .IsRequired(true);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.CodigoAlmacen)
                .HasColumnName("codigoalmacen")
                .IsRequired(true);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.CodigoClasificacion)
                .HasColumnName("codigoclasificacion")
                .IsRequired(false);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Unidades)
                .HasColumnName("unidades")
                .IsRequired(true);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Fecha)
                .HasColumnName("fecha")
                .IsRequired(true);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Referencia)
                .HasColumnName("referencia")
                .IsRequired(false);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.Surtidas)
                .HasColumnName("surtidas")
                .IsRequired(false);

            modelBuilder.Entity<Movimiento>()
                .Property(m => m.EsGranel)
                .HasColumnName("esgranel")
                .IsRequired(true);

            modelBuilder.Entity<Pedido>().ToTable("pedido").HasKey(p => p.IdInterfaz);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.IdInterfaz)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Folio)
                .HasColumnName("folio")
                .IsRequired(true);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.NumMoneda)
                .HasColumnName("anummoneda")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.TipoCambio)
                .HasColumnName("atipocambio")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Importe)
                .HasColumnName("aimporte")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.DescuentoDoc1)
                .HasColumnName("adescuentodoc1")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.DescuentoDoc2)
                .HasColumnName("adescuentodoc2")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.SistemaOrigen)
                .HasColumnName("asistemaorigen")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.CodConcepto)
                .HasColumnName("acodconcepto")
                .IsRequired(true);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Serie)
                .HasColumnName("aserie")
                .IsRequired(true);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Fecha)
                .HasColumnName("afecha")
                .IsRequired(true);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.CodigoCteProv)
                .HasColumnName("acodigocteprov")
                .IsRequired(true);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.CodigoAgente)
                .HasColumnName("acodigoagente")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Referencia)
                .HasColumnName("areferencia")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Afecta)
                .HasColumnName("aafecta")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Gasto1)
                .HasColumnName("agasto1")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>() 
                .Property(p => p.Gasto2)
                .HasColumnName("agasto2")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Gasto3)
                .HasColumnName("agasto3")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Surtido)
                .HasColumnName("surtido")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Observaciones)
                .HasColumnName("cobservaciones")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.TextoExtra1)
                .HasColumnName("ctextoextra1")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.TextoExtra2)
                .HasColumnName("ctextoextra2")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.TextoExtra3)
                .HasColumnName("ctextoextra3")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Impreso)
                .HasColumnName("impreso")
                .IsRequired(false);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.IdContpaqiSQL)
                .HasColumnName("id_contpaqi_sql")
                .IsRequired(false);


        }
    }
}
