using Domain.Entities;

namespace Domain.Interfaces.Repos
{
    public interface IProductRepo
    {
        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns></returns>
        Task<List<ProductoSQL>> GetAllProductsAsync();

        /// <summary>
        /// Gets a product by its id
        /// </summary>
        /// <param name="idProductos"></param>
        /// <returns>Matching obj instance, if not, exception is thrown</returns>
        Task<ProductoSQL> GetProductByIdAsync(int idProductos);

        /// <summary>
        /// Gets a list of products by their ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<ProductoSQL>> GetProductsByMultipleIdsAsync(List<int> ids);

        /// <summary>
        /// Gets a product by its code
        /// </summary>
        /// <param name="codigoProducto"></param>
        /// <returns>Matching obj instance, if not, exception is thrown</returns>
        Task<ProductoSQL> GetProductByCodigoAsync(string codigoProducto);

        /// <summary>
        /// Gets products, but also filtering the CIDVALORCLASIFICACIO6 field, it ignores the 0 value i this field
        /// </summary>
        /// <param name="idProducto"></param>
        /// <returns>PRODUCTS FILTERED BY CPE REQUIREMENTS</returns>
        Task<List<ProductoSQL>> GetProductByIdsCPEAsync(List<int> idsProductos);
    }
}
