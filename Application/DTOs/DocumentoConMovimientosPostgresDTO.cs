using Domain.Entities.Interfaces;

namespace Application.DTOs
{
    public class DocumentoConMovimientosPostgresDTO
    {
        public Documento Documento { get; set; } = new();
        public List<Movimiento> Movimientos { get; set; } = new();
    }
}
