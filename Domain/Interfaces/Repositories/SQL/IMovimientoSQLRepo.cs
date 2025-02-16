using Core.Domain.Entities.SQL;

namespace Core.Domain.Interfaces.Repositories.SQL
{
    /// <summary>
    /// READONLY Interfaz para el repositorio de movimientos
    /// </summary>
    public interface IMovimientoSQLRepo
    {
        /// <summary>
        /// Obtiene los movimientos por id de documento padre
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<IEnumerable<MovimientoSQL>> GetByDocumentId(int idDocumento, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene los ids de los movimientos por id de documento padre
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<IEnumerable<int>> GetMovimientosIdsByDocumenId(int idDocumento, CancellationToken cancellationToken = default);
    }
}
