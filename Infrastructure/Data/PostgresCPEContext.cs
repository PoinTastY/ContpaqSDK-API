using Core.Domain.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class PostgresCPEContext : DbContext
    {
        public DbSet<MovimientoDto> movimientos { get; set; } = null!;
        public DbSet<DocumentoDto> documentos { get; set; } = null!;
        public PostgresCPEContext(DbContextOptions<PostgresCPEContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovimientoDto>().ToTable("movimiento").HasKey(m => m.Id);

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.IdDocumento)
                .HasColumnName("id_pedido")
                .IsRequired(true);

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.CodigoProducto)
                .HasColumnName("codigoproducto")
                .IsRequired(true);

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.CodigoAlmacen)
                .HasColumnName("codigoalmacen")
                .IsRequired(true);

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.CodigoClasificacion)
                .HasColumnName("codigoclasificacion")
                .IsRequired(false);

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.Unidades)
                .HasColumnName("unidades")
                .IsRequired(true);

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.Fecha)
                .HasColumnName("fecha")
                .IsRequired(true);

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.Referencia)
                .HasColumnName("referencia")
                .IsRequired(false);

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.Surtidas)
                .HasColumnName("surtidas")
                .HasDefaultValue(0);

            modelBuilder.Entity<MovimientoDto>()
                .Property(m => m.EsGranel)
                .HasColumnName("es_granel")
                .IsRequired(true);

            modelBuilder.Entity<DocumentoDto>().ToTable("documento").HasKey(p => p.IdPostgres);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.IdPostgres)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Folio)
                .HasColumnName("folio")
                .IsRequired(true);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.NumMoneda)
                .HasColumnName("anummoneda")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.TipoCambio)
                .HasColumnName("atipocambio")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Importe)
                .HasColumnName("aimporte")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.DescuentoDoc1)
                .HasColumnName("adescuentodoc1")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.DescuentoDoc2)
                .HasColumnName("adescuentodoc2")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.SistemaOrigen)
                .HasColumnName("asistemaorigen")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.CodConcepto)
                .HasColumnName("acodconcepto")
                .IsRequired(true);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Serie)
                .HasColumnName("aserie")
                .IsRequired(true);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Fecha)
                .HasColumnName("afecha")
                .IsRequired(true);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.CodigoCteProv)
                .HasColumnName("acodigocteprov")
                .IsRequired(true);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.RazonSocial)
                .HasColumnName("razonsocial")
                .IsRequired(true);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.CodigoAgente)
                .HasColumnName("acodigoagente")
                .IsRequired(false);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Referencia)
                .HasColumnName("areferencia")
                .IsRequired(false);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Afecta)
                .HasColumnName("aafecta")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Gasto1)
                .HasColumnName("agasto1")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>() 
                .Property(p => p.Gasto2)
                .HasColumnName("agasto2")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Gasto3)
                .HasColumnName("agasto3")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Surtido)
                .HasColumnName("surtido")
                .HasDefaultValue(0);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Observaciones)
                .HasColumnName("cobservaciones")
                .IsRequired(false);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.TextoExtra1)
                .HasColumnName("ctextoextra1")
                .IsRequired(false);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.TextoExtra2)
                .HasColumnName("ctextoextra2")
                .IsRequired(false);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.TextoExtra3)
                .HasColumnName("ctextoextra3")
                .IsRequired(false);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.Impreso)
                .HasColumnName("impreso")
                .HasDefaultValue(false);

            modelBuilder.Entity<DocumentoDto>()
                .Property(p => p.IdContpaqiSQL)
                .HasColumnName("id_sql_documento")
                .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>().ToTable("pedidos_cancelados");

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.IdInterfaz)
            //    .HasColumnName("id");

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Folio)
            //    .HasColumnName("folio")
            //    .IsRequired(true);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.NumMoneda)
            //    .HasColumnName("anummoneda")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.TipoCambio)
            //    .HasColumnName("atipocambio")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Importe)
            //    .HasColumnName("aimporte")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.DescuentoDoc1)
            //    .HasColumnName("adescuentodoc1")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.DescuentoDoc2)
            //    .HasColumnName("adescuentodoc2")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.SistemaOrigen)
            //    .HasColumnName("asistemaorigen")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.CodConcepto)
            //    .HasColumnName("acodconcepto")
            //    .IsRequired(true);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Serie)
            //    .HasColumnName("aserie")
            //    .IsRequired(true);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Fecha)
            //    .HasColumnName("afecha")
            //    .IsRequired(true);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.CodigoCteProv)
            //    .HasColumnName("acodigocteprov")
            //    .IsRequired(true);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.CodigoAgente)
            //    .HasColumnName("acodigoagente")
            //    .IsRequired(false);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Referencia)
            //    .HasColumnName("areferencia")
            //    .IsRequired(false);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Afecta)
            //    .HasColumnName("aafecta")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Gasto1)
            //    .HasColumnName("agasto1")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Gasto2)
            //    .HasColumnName("agasto2")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Gasto3)
            //    .HasColumnName("agasto3")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Surtido)
            //    .HasColumnName("surtido")
            //    .HasDefaultValue(0);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Observaciones)
            //    .HasColumnName("cobservaciones")
            //    .IsRequired(false);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.TextoExtra1)
            //    .HasColumnName("ctextoextra1")
            //    .IsRequired(false);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.TextoExtra2)
            //    .HasColumnName("ctextoextra2")
            //    .IsRequired(false);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.TextoExtra3)
            //    .HasColumnName("ctextoextra3")
            //    .IsRequired(false);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.Impreso)
            //    .HasColumnName("impreso")
            //    .HasDefaultValue(false);

            //modelBuilder.Entity<Documento>()
            //    .Property(p => p.IdContpaqiSQL)
            //    .HasColumnName("id_sql_documento")
            //    .HasDefaultValue(0);
        }
    }
}
