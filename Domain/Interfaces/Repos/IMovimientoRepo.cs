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
    }
}
