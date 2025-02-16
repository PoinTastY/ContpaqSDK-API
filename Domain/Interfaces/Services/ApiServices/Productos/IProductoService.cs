namespace Core.Domain.Interfaces.Services.ApiServices.Productos
{
    public interface IProductoService
    {
        /// <summary>
        /// Gets the products by their ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<List<T>> GetProductosByIdsSQLAsync<T>(List<int> ids);

        /// <summary>
        /// Ask for te products by id filterig CPE requirements
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<List<T>> GetProductosByIdListCPESQLAsync<T>(List<int> ids);
    }
}
