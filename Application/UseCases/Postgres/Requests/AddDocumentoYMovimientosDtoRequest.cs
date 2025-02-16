using Core.Domain.Entities.DTOs;

namespace Core.Application.UseCases.Postgres.Requests
{
    public class AddDocumentoYMovimientosDtoRequest
    {
        public DocumentoDto Documento { get; set; } = new();
        public List<MovimientoDto> Movimientos { get; set; } = new();
    }
}
