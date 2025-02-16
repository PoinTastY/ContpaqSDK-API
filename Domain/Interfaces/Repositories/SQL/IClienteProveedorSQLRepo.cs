using Core.Domain.Entities.SQL;

namespace Core.Domain.Interfaces.Repositories.SQL
{
    /// <summary>
    /// READONLY Interfaz para el repositorio de clientes y proveedores
    /// </summary>
    public interface IClienteProveedorSQLRepo
    {
        /// <summary>
        /// Busca todos los clientes y proveedores con el nombre proporcionado
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<IEnumerable<ClienteProveedorSQL>> SearchByName(string name, CancellationToken cancellationToken = default);
    }
}
