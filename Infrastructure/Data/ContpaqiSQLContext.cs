using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ContpaqiSQLContext : DbContext
    {
        public DbSet<DocumentSQL> documents { get; set; } = null!;
        public DbSet<ProductoSQL> productos { get; set; } = null!;
        public DbSet<ConceptoSQL> conceptos { get; set; } = null!;
        public DbSet<MovimientoSQL> movimientos { get; set; } = null!;
        public ContpaqiSQLContext(DbContextOptions<ContpaqiSQLContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DocumentSQL>().ToTable("admDocumentos");
            modelBuilder.Entity<DocumentSQL>(entity =>
            {
                entity.HasKey(e => e.CIDDOCUMENTO);

                entity.HasIndex(e => new { e.CFECHA, e.CIDDOCUMENTO }, "CFECHA");

                entity.HasIndex(e => new { e.CFECHAVENCIMIENTO, e.CIDDOCUMENTO }, "CFECHAVENCIMIENTO");

                entity.HasIndex(e => new { e.CIDCOPIADE, e.CFECHA, e.CIDDOCUMENTO }, "CIDCOPIADE");

                entity.HasIndex(e => new { e.CIDCUENTA, e.CFECHA, e.CIDDOCUMENTO }, "CIDCUENTA");

                entity.HasIndex(e => new { e.CIDDOCUMENTOORIGEN, e.CIDDOCUMENTO }, "CIDDOCUMENTOORIGEN");

                entity.HasIndex(e => new { e.CIDMONEDA, e.CIDDOCUMENTO }, "CIDMONEDA");

                entity.HasIndex(e => new { e.CIDPREPOLIZA, e.CIDDOCUMENTO }, "CIDPREPOLIZA");

                entity.HasIndex(e => new { e.CIDPROYECTO, e.CFECHA, e.CIDDOCUMENTO }, "CIDPROYECTO");

                entity.HasIndex(e => new { e.CSISTORIG, e.CIDDOCUMENTO }, "CSISTORIG");

                entity.HasIndex(e => new { e.CIDAGENTE, e.CFECHA, e.CSERIEDOCUMENTO, e.CFOLIO, e.CIDDOCUMENTO }, "IAGENTEFECHASERIEFOLIO");

                entity.HasIndex(e => new { e.CNUMEROGUIA, e.CDESTINATARIO, e.CIDDOCUMENTO }, "IBANCOS");

                entity.HasIndex(e => new { e.CIDCLIENTEPROVEEDOR, e.CAFECTADO, e.CNATURALEZA, e.CFECHAVENCIMIENTO, e.CIDDOCUMENTO }, "ICLIENTEPROVAFECTANATVENC");

                entity.HasIndex(e => new { e.CIDCLIENTEPROVEEDOR, e.CIDCONCEPTODOCUMENTO, e.CFECHA, e.CSERIEDOCUMENTO, e.CFOLIO }, "ICLIENTEPROVCPTOFECHA");

                entity.HasIndex(e => new { e.CIDCLIENTEPROVEEDOR, e.CFECHA, e.CSERIEDOCUMENTO, e.CFOLIO, e.CIDDOCUMENTO }, "ICLIENTEPROVEEDORFECHA");

                entity.HasIndex(e => new { e.CIDCLIENTEPROVEEDOR, e.CPENDIENTE, e.CAFECTADO, e.CNATURALEZA, e.CFECHAVENCIMIENTO, e.CIDDOCUMENTO }, "ICLIPENFEC");

                entity.HasIndex(e => new { e.CIDCONCEPTODOCUMENTO, e.CFECHA, e.CSERIEDOCUMENTO, e.CFOLIO }, "ICONCEPTOFECHA");

                entity.HasIndex(e => new { e.CIDCONCEPTODOCUMENTO, e.CFOLIO, e.CIDDOCUMENTO }, "ICONCEPTOFOLIO");

                entity.HasIndex(e => new { e.CIDCLIENTEPROVEEDOR, e.CIDDOCUMENTODE, e.CFECHAVENCIMIENTO, e.CIDDOCUMENTO }, "ICTEDOCTODEFECVENCCHQW");

                entity.HasIndex(e => new { e.CIDCLIENTEPROVEEDOR, e.CNATURALEZA, e.CPENDIENTE, e.CIDDOCUMENTO }, "ICTEPROVNATPEN");

                entity.HasIndex(e => new { e.CIDDOCUMENTODE, e.CIDDOCUMENTOORIGEN, e.CIDDOCUMENTO }, "IDOCTODEDOCTOORIGEN");

                entity.HasIndex(e => new { e.CIDDOCUMENTODE, e.CSERIEDOCUMENTO, e.CFOLIO, e.CIDDOCUMENTO }, "IDOCTODESERIEFOLIO");

                entity.HasIndex(e => new { e.CIDDOCUMENTODE, e.CIDAGENTE, e.CFECHA, e.CSERIEDOCUMENTO, e.CFOLIO }, "IDOCUMENTODEAGENTEFECHA");

                entity.HasIndex(e => new { e.CIDDOCUMENTODE, e.CIDCLIENTEPROVEEDOR, e.CFECHA, e.CSERIEDOCUMENTO, e.CFOLIO }, "IDOCUMENTODECLIENTEFECHA");

                entity.HasIndex(e => new { e.CIDDOCUMENTODE, e.CFECHA, e.CSERIEDOCUMENTO, e.CFOLIO }, "IDOCUMENTODEFECHASERIEFOL");

                entity.HasIndex(e => e.CCANCELADO, "RCCANCELADO");

                entity.HasIndex(e => new { e.CIDCLIENTEPROVEEDOR, e.CNATURALEZA, e.CAFECTADO, e.CFECHAVENCIMIENTO, e.CPENDIENTE }, "RIDCLINATAFEC");

                entity.Property(e => e.CCONDIPAGO)
                    .HasMaxLength(253)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CCUENTAMENSAJERIA)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CDESTINATARIO)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CFECHA)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHAENTREGARECEPCION)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHAEXTRA)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHAPRONTOPAGO)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHAULTIMOINTERES)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHAVENCIMIENTO)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CGUIDDOCUMENTO)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CLUGAREXPE)
                    .HasMaxLength(380)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CMENSAJERIA)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CMETODOPAG)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CNUMCTAPAG)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CNUMEROGUIA)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.COBSERVACIONES).HasColumnType("text");
                entity.Property(e => e.CRAZONSOCIAL)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CREFERENCIA)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CRFC)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSERIEDOCUMENTO)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTEXTOEXTRA1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTEXTOEXTRA2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTEXTOEXTRA3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTIMESTAMP)
                    .HasMaxLength(23)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTRANSACTIONID)
                    .HasMaxLength(26)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CUSUARIO)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CVERESQUE)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasDefaultValue("");
            });



            modelBuilder.Entity<ConceptoSQL>().ToTable("admConceptos");
            modelBuilder.Entity<ConceptoSQL>(entity =>
            {

                entity.HasKey(e => e.CIDCONCEPTODOCUMENTO);

                entity.HasIndex(e => e.CCODIGOCONCEPTO, "CCODIGOCONCEPTO");

                entity.HasIndex(e => new { e.CIDCUENTA, e.CIDCONCEPTODOCUMENTO }, "CIDCUENTA");

                entity.HasIndex(e => new { e.CIDDOCUMENTODE, e.CIDCONCEPTODOCUMENTO }, "CIDDOCUMENTODE");

                entity.HasIndex(e => e.CIDCONCEPTODOCUMENTO, "DCIDCONCEPTODOCUMENTO");

                entity.HasIndex(e => new { e.CNOMBRECONCEPTO, e.CNATURALEZA, e.CIDCONCEPTODOCUMENTO }, "INOMBRENATURALEZA");

                entity.Property(e => e.CCLAVESAT)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CCODIGOCONCEPTO)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CESTATUS).HasDefaultValue(1);
                entity.Property(e => e.CFORMAPREIMPRESA)
                    .HasMaxLength(253)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CIDFIRMADSL)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CMETODOPAG)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CNOMBRECONCEPTO)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CORDENCAPTURA)
                    .HasMaxLength(52)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CPLAMIGCFD)
                    .HasMaxLength(253)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CPREFIJOCONCEPTO)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CREGIMFISC)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CREPIMPCFD)
                    .HasMaxLength(253)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CRUTAENTREGA)
                    .HasMaxLength(253)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSCCPTO2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSCCPTO3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSCMOVTO)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSEGCONTCONCEPTO)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSERIEPOROMISION)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTIMESTAMP)
                    .HasMaxLength(23)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CVERESQUE)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasDefaultValue("");
            });

            modelBuilder.Entity<ProductoSQL>().ToTable("admProductos");
            modelBuilder.Entity<ProductoSQL>(entity =>
            {
                entity.HasKey(e => e.CIDPRODUCTO);

                entity.HasIndex(e => new { e.CCLAVESAT, e.CIDPRODUCTO }, "CCLAVESAT");

                entity.HasIndex(e => e.CCODIGOPRODUCTO, "CCODIGOPRODUCTO").IsUnique();

                entity.HasIndex(e => new { e.CERRORCOSTO, e.CIDPRODUCTO }, "CERRORCO01");

                entity.HasIndex(e => new { e.CFECHAALTAPRODUCTO, e.CIDPRODUCTO }, "CFECHAALTAPRODUCTO");

                entity.HasIndex(e => new { e.CIDPADRECARACTERISTICA1, e.CIDPRODUCTO }, "CIDPADRECARACTERISTICA1");

                entity.HasIndex(e => new { e.CIDPADRECARACTERISTICA2, e.CIDPRODUCTO }, "CIDPADRECARACTERISTICA2");

                entity.HasIndex(e => new { e.CIDPADRECARACTERISTICA3, e.CIDPRODUCTO }, "CIDPADRECARACTERISTICA3");

                entity.HasIndex(e => new { e.CIDUNIDADBASE, e.CIDPRODUCTO }, "CIDUNIDADBASE");

                entity.HasIndex(e => new { e.CIDUNIDADNOCONVERTIBLE, e.CIDPRODUCTO }, "CIDUNIDADNOCONVERTIBLE");

                entity.HasIndex(e => new { e.CIDVALORCLASIFICACION1, e.CIDPRODUCTO }, "CIDVALORCLASIFICACION1");

                entity.HasIndex(e => new { e.CIDVALORCLASIFICACION2, e.CIDPRODUCTO }, "CIDVALORCLASIFICACION2");

                entity.HasIndex(e => new { e.CIDVALORCLASIFICACION3, e.CIDPRODUCTO }, "CIDVALORCLASIFICACION3");

                entity.HasIndex(e => new { e.CIDVALORCLASIFICACION4, e.CIDPRODUCTO }, "CIDVALORCLASIFICACION4");

                entity.HasIndex(e => new { e.CIDVALORCLASIFICACION5, e.CIDPRODUCTO }, "CIDVALORCLASIFICACION5");

                entity.HasIndex(e => new { e.CIDVALORCLASIFICACION6, e.CIDPRODUCTO }, "CIDVALORCLASIFICACION6");

                entity.HasIndex(e => new { e.CCODALTERN, e.CTIPOPRODUCTO, e.CIDPRODUCTO }, "ICODALTTIP");

                entity.HasIndex(e => new { e.CCODIGOPRODUCTO, e.CTIPOPRODUCTO }, "ICODIGOTIPO");

                entity.HasIndex(e => new { e.CMETODOCOSTEO, e.CIDPRODUCTO }, "IMETODOCOSTEO");

                entity.HasIndex(e => new { e.CNOMALTERN, e.CTIPOPRODUCTO, e.CIDPRODUCTO }, "INOMALTTIP");

                entity.HasIndex(e => new { e.CNOMBREPRODUCTO, e.CTIPOPRODUCTO, e.CIDPRODUCTO }, "INOMBRETIPO");

                entity.HasIndex(e => new { e.CSTATUSPRODUCTO, e.CIDPRODUCTO }, "ISTATUSPRODUCTO");

                entity.HasIndex(e => new { e.CTIPOPRODUCTO, e.CSUBTIPO, e.CCODIGOPRODUCTO }, "ITIPCODIGO");

                entity.HasIndex(e => new { e.CTIPOPRODUCTO, e.CSUBTIPO, e.CNOMBREPRODUCTO, e.CIDPRODUCTO }, "ITIPNOMBRE");

                entity.Property(e => e.CCLAVESAT)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CCODALTERN)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CCODIGOPRODUCTO)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CCTAPRED)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CDESCCORTA)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CDESCRIPCIONPRODUCTO).HasColumnType("text");
                entity.Property(e => e.CFECCOSEX1)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECCOSEX2)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECCOSEX3)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECCOSEX4)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECCOSEX5)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHAALTAPRODUCTO)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHABAJA)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHAERRORCOSTO)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHAEXTRA)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CNOMALTERN)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CNOMBREPRODUCTO)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSEGCONTPRODUCTO1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSEGCONTPRODUCTO2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSEGCONTPRODUCTO3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSEGCONTPRODUCTO4)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSEGCONTPRODUCTO5)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSEGCONTPRODUCTO6)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSEGCONTPRODUCTO7)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTEXTOEXTRA1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTEXTOEXTRA2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTEXTOEXTRA3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTIMESTAMP)
                    .HasMaxLength(23)
                    .IsUnicode(false)
                    .HasDefaultValue("");
            });

            modelBuilder.Entity<MovimientoSQL>().ToTable("admMovimientos");
            modelBuilder.Entity<MovimientoSQL>(entity =>
            {
                entity.HasKey(e => e.CIDMOVIMIENTO);

                entity.HasIndex(e => new { e.CIDALMACEN, e.CIDMOVIMIENTO }, "CIDALMACEN");

                entity.HasIndex(e => new { e.CIDMOVTODESTINO, e.CIDMOVIMIENTO }, "CIDMOVTODESTINO");

                entity.HasIndex(e => new { e.CIDMOVTOORIGEN, e.CIDMOVIMIENTO }, "CIDMOVTOORIGEN");

                entity.HasIndex(e => e.CIDMOVIMIENTO, "DCIDMOVIMIENTO");

                entity.HasIndex(e => new { e.CAFECTADOSALDOS, e.CIDMOVIMIENTO }, "IAFECTASALDOS");

                entity.HasIndex(e => new { e.CIDDOCUMENTO, e.CNUMEROMOVIMIENTO }, "IDOCTONUMEROMOVTO");

                entity.HasIndex(e => new { e.CIDDOCUMENTO, e.CIDPRODUCTO, e.CIDMOVIMIENTO }, "IDOCTOPROD");

                entity.HasIndex(e => new { e.CAFECTAEXISTENCIA, e.CAFECTADOINVENTARIO, e.CIDMOVIMIENTO }, "IEXISTENCIAAFECTADO");

                entity.HasIndex(e => new { e.CMOVTOOCULTO, e.CIDMOVTOOWNER, e.CAFECTAEXISTENCIA, e.CIDMOVIMIENTO }, "IMOVTOOCULTOOWNER");

                entity.HasIndex(e => new { e.CIDMOVTOOWNER, e.CAFECTAEXISTENCIA, e.CIDMOVIMIENTO }, "IMOVTOOWNERAFECTAEXIST");

                entity.HasIndex(e => new { e.CIDMOVTOOWNER, e.CIDMOVIMIENTO }, "IMOVTOOWNERMOVTO");

                entity.HasIndex(e => new { e.CIDPRODUCTO, e.CIDALMACEN, e.CIDDOCUMENTODE, e.CFECHA, e.CIDMOVIMIENTO }, "IPROALMD02");

                entity.HasIndex(e => new { e.CIDPRODUCTO, e.CIDALMACEN, e.CIDMOVTOOWNER, e.CTIPOTRASPASO, e.CIDMOVIMIENTO }, "IPRODALMACENOWNERTRASP");

                entity.HasIndex(e => new { e.CIDPRODUCTO, e.CIDALMACEN, e.CAFECTADOINVENTARIO, e.CFECHA, e.CIDMOVIMIENTO }, "IPRODUCTOALMACENAFECFECHA");

                entity.HasIndex(e => new { e.CIDPRODUCTO, e.CIDALMACEN, e.CFECHA, e.CAFECTAEXISTENCIA, e.CIDMOVIMIENTO }, "IPRODUCTOALMACENFECHAAFEC");

                entity.HasIndex(e => new { e.CIDPRODUCTO, e.CIDDOCUMENTODE, e.CAFECTADOINVENTARIO, e.CFECHA, e.CIDMOVIMIENTO }, "IPRODUCTODOCTODEAFECFECHA");

                entity.HasIndex(e => new { e.CIDPRODUCTO, e.CFECHA, e.CAFECTAEXISTENCIA, e.CIDMOVIMIENTO }, "IPRODUCTOFECHAAFECTA");

                entity.Property(e => e.CFECHA)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.CFECHAEXTRA)
                    .HasDefaultValueSql("('18991230')")
                    .HasColumnType("datetime");
                entity.Property(e => e.COBJIMPU01)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.COBSERVAMOV).HasColumnType("text");
                entity.Property(e => e.CREFERENCIA)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CSCMOVTO)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTEXTOEXTRA1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTEXTOEXTRA2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTEXTOEXTRA3)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValue("");
                entity.Property(e => e.CTIMESTAMP)
                    .HasMaxLength(23)
                    .IsUnicode(false)
                    .HasDefaultValue("");
            });

        }
    }
}
