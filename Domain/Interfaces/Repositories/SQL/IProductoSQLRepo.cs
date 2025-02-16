using Core.Domain.Entities.SQL;

namespace Core.Domain.Interfaces.Repositories.SQL
{
    public interface IProductoSQLRepo
    {
        /// <summary>
        /// Obtiene todos los productos diablos
        /// </summary>
        Task<IEnumerable<ProductoSQL>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene un producto por su id
        /// </summary>
        /// <param name="idProductos"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<ProductoSQL> GetByIdAsync(int idProducto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene una COLECCION de productos por sus CCODIGOPRODUCTO
        /// </summary>
        /// <param name="codigos"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<IEnumerable<ProductoSQL>> GetByMultipleCodigosAsync(IEnumerable<string> codigos, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene un producto por su codigo, CCODIGOPRODUCTO
        /// </summary>
        /// <param name="codigoProducto"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<ProductoSQL> GetByCodigoAsync(string codigoProducto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene una COLECCION de productos por sus CIDPRODUCTO
        /// </summary>
        /// <param name="idProducto"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<IEnumerable<ProductoSQL>> GetByIdsAsync(IEnumerable<int> idsProductos, CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca productos por nombre
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<IEnumerable<ProductoSQL>> SearchByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
