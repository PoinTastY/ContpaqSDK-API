namespace Application.DTOs
{
    public class DocumentoConMovimientosDTO
    {
        public DocumentDTO Documento { get; set; } = new DocumentDTO();
        public List<MovimientoDTO> Movimientos { get; set; } = new();
    }
}
