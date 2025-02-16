using Core.Domain.Entities.DTOs;

namespace Core.Application.UseCases.SDK.Requests
{
    public class AddDocumentoYMovimientosSDKRequest
    {
        public DocumentoDto DocumentoDto { get; set; } = new ();
        public List<MovimientoDto> MovimientoDtos { get; set; } = new();
        public string Empresa { get; set; } = string.Empty;
    }
}
