using Core.Domain.Entities.SQL;

namespace Core.Domain.Interfaces.Repositories.SQL
{
    /// <summary>
    /// READONLY Interfaz para el repositorio de documentos
    /// </summary>
    public interface IDocumentoSQLRepo
    {
        /// <summary>
        /// Obtiene una coleccion de documentos por fecha concepto y serie
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="codigoConcepto"></param>
        /// <param name="serie"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<IEnumerable<DocumentoSQL>> GetRangeByFechaConceptoSerieAsync(DateTime fechaInicio, DateTime fechaFin, string codigoConcepto, string serie, CancellationToken cancellationToken = default);

        /// <summary>
        /// retorna una COLECCION de documentos por fecha, concepto y serie
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="codigoConcepto"></param>
        /// <param name="serie"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<IEnumerable<DocumentoSQL>> GetRangeByFechaSerieAsync(DateTime fechaInicio, DateTime fechaFin, string serie, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca un documento por su folio y serie
        /// </summary>
        /// <param name="folio"></param>
        /// <param name="serie"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<DocumentoSQL> GetByFolioAndSerieAsync(string folio, string serie, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna un documento por su id
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<DocumentoSQL> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca documentos por id de cliente y rango de fecha
        /// </summary>
        /// <param name="idCliente"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<IEnumerable<DocumentoSQL>> GetByIdClienteAndDateAsync(int idCliente, DateTime fechaInicio, DateTime fechaFin, CancellationToken cancellationToken = default);
    }
}
