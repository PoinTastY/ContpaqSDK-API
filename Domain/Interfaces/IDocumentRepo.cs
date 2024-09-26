using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDocumentRepo
    {
        Task<List<Document>> GetAllDocumentsByFechaConceptoSerieAsync(DateTime fechaInicio, DateTime fechaFin, string codigoConcepto, string serie);
        Task<List<Document>> GetAllDocumentsBySerieAsync(DateTime fechaInicio, DateTime fechaFin, string serie);
        Task<Document> GetDocumentByFolioAsync(string folio, string serie);
    }
}
