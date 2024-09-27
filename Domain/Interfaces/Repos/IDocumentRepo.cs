using Domain.Entities;

namespace Domain.Interfaces.Repos
{
    public interface IDocumentRepo
    {
        Task<List<DocumentSQL>> GetAllDocumentsByFechaConceptoSerieAsync(DateTime fechaInicio, DateTime fechaFin, string codigoConcepto, string serie);
        Task<List<DocumentSQL>> GetAllDocumentsBySerieAsync(DateTime fechaInicio, DateTime fechaFin, string serie);
        Task<DocumentSQL> GetDocumentByFolioAsync(string folio, string serie);
    }
}
