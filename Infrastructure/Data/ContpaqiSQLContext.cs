using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ContpaqiSQLContext : DbContext
    {
        public DbSet<Document> documents { get; set; } = null!;
        public DbSet<Concept> concepts { get; set; } = null!;
        public ContpaqiSQLContext(DbContextOptions<ContpaqiSQLContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Document>().ToTable("admDocumentos");
            modelBuilder.Entity<Document>(entity =>
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

            modelBuilder.Entity<Concept>().ToTable("admConceptos");
            modelBuilder.Entity<Concept>(entity =>
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

        }
    }
}
