using Domain.Entities;

namespace Domain.Interfaces.Repos
{
    public interface IDocumentRepo
    {
        /// <summary>
        /// Returns a list of documents by fecha, concepto and serie
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="codigoConcepto"></param>
        /// <param name="serie"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<List<DocumentSQL>> GetAllDocumentsByFechaConceptoSerieAsync(DateTime fechaInicio, DateTime fechaFin, string codigoConcepto, string serie);

        /// <summary>
        /// retorna una lista de documentos por fecha, concepto y serie, para la coonsulta de CPE (only TextoExtra3 empties)
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="codigoConcepto"></param>
        /// <param name="serie"></param>
        /// <returns></returns>
        Task<List<DocumentSQL>> GetAllDocumentsByFechaSerieCPEAsync(DateTime fechaInicio, DateTime fechaFin, string serie);

        /// <summary>
        /// Needs a date range and a serie to return a list of documents
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="serie"></param>
        /// <returns>List of documents matching the serie</returns>
        /// <exception cref="NotFoundArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        Task<List<DocumentSQL>> GetAllDocumentsByFechaAndSerieAsync(DateTime fechaInicio, DateTime fechaFin, string serie);

        /// <summary>
        /// Search documents by folio and serie
        /// </summary>
        /// <param name="folio"></param>
        /// <returns>Matching Folium</returns>
        /// <exception cref="Exception"></exception>
        Task<DocumentSQL> GetDocumentByFolioAndSerieAsync(string folio, string serie);

        /// <summary>
        /// Ask if a document exists by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if it does, false if not</returns>
        Task<bool> IdExistAsync(int id);

        Task<List<DocumentSQL>> GetDocumentosByIdClienteAndDateAsync(int idCliente, DateTime fechaInicio, DateTime fechaFin);
    }
}
