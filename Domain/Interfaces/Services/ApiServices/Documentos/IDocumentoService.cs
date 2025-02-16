using Core.Domain.Interfaces.Repositories.SQL;

namespace Core.Domain.Interfaces.Services.ApiServices.Documentos
{
    public interface IDocumentoService : IDocumentoSQLRepo
    {
        /// <summary>
        /// Get a document by its concepto, serie and folio
        /// </summary>
        /// <param name="concepto"></param>
        /// <param name="serie"></param>
        /// <param name="folio"></param>
        /// <returns>DocumentDTO, parsed from the data from the apiresponse</returns>
        /// <exception cref="Exception"></exception>
        Task<T> GetDocumentoByConceptoSerieAndFolioSDKAsync<T>(string codConcepto, string serie, string folio);

        /// <summary>
        /// Get a document by its id, asking the SDK
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns>DocumentDTO parsed form the ApiResponse Data atrbute</returns>
        /// <exception cref="Exception"></exception>
        Task<T> GetDocumentByIdSDKAsync<T>(int idDocumento);

        /// <summary>
        /// Get a document by its id, filtering date, filtering by CPE requirements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="serie"></param>
        /// <returns>List of documents</returns>
        Task<List<T>> GetPedidosByFechaSerieCPESQL<T>(DateTime fechaInicio, DateTime fechaFin, string serie);
    }
}