using Domain.Entities;

namespace Domain.Interfaces.Repos
{
    public interface IMovimientoRepo
    {
        /// <summary>
        /// Ask for a list of movements by document id
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns>enumerable of MovimientoSQL intaces</returns>
        Task<List<MovimientoSQL>> GetMovimientosByDocumentId(int idDocumento);

        /// <summary>
        /// Ask for a list of movement ID only by document id
        /// </summary>
        /// <param name="idDocumento"></param>
        /// <returns>enumerable of int's that have the id of the provided documentid</returns>
        Task<List<int>> GetMovimientosIdsByDocumenId(int idDocumento);

        /// <summary>
        /// Updates the movement units by id, on SQL
        /// </summary>
        /// <param name="idMovimiento"></param>
        /// <param name="unidades"></param>
        /// <returns>nothing if good, error if bad</returns>
        Task UpdateUnidadesMovimientoById(int idMovimiento, double unidades);
    }
}
