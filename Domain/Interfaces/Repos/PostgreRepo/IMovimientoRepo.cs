using Domain.Entities.Interfaces;

namespace Domain.Interfaces.Repos.PostgreRepo
{
    public interface IMovimientoRepo
    {
        /// <summary>
        /// Adds movimientos to the database
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        Task AddMovimientosAsync(List<Domain.Entities.Interfaces.Movimiento> movimientos);

        /// <summary>
        /// Gets all movimientos by documento id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Movimiento matches w provided document id</returns>
        Task<List<Domain.Entities.Interfaces.Movimiento>> GetMovimientosByDocumentoIdAsync(int id);

        /// <summary>
        /// Updates the unidades of a movimiento
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        Task UpdateMovimientos(List<Movimiento> movimientos);
    }
}
