using Core.Domain.Entities.DTOs;

namespace Application.DTOs
{
    public class DocumentoConMovimientosPostgresDTO
    {
        public DocumentoDto Documento { get; set; } = new();
        public List<MovimientoDto> Movimientos { get; set; } = new();
    }
}
