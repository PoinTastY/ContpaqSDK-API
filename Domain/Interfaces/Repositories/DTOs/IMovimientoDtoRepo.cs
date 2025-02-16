using Core.Domain.Entities.DTOs;

namespace Core.Domain.Interfaces.Repositories.DTOs
{
    public interface IMovimientoDtoRepo
    {
        /// <summary>
        /// Agrega una COLECCION de movimientos a la base de datos
        /// </summary>
        /// <param name="movimientos"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<MovimientoDto> movimientos, CancellationToken cancellationToken = default);

        /// <summary>
        /// Agrega UN movimiento a la base de datos
        /// </summary>
        /// <param name="movimiento"></param>
        Task<MovimientoDto> AddAsync(MovimientoDto movimiento, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene una COLECCION de movimientos por su id de documento padre
        /// </summary>
        /// <param name="id"></param>   
        Task<IEnumerable<MovimientoDto>> GetByDocumentoDtoIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza una COLECCION de movimientos en la base de datos
        /// </summary>
        /// <param name="movimientos"></param>
        Task UpdateRangeAsync(IEnumerable<MovimientoDto> movimientos, CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza un movimiento en la base de datos
        /// </summary>
        /// <param name="movimiento"></param>
        /// <param name="cancellationToken"></param>
        Task UpdateAsync(MovimientoDto movimiento, CancellationToken cancellationToken = default);
    }
}
