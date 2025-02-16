using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;
using Domain.Entities;

namespace Application.UseCases.SQL.Productos
{
    public class GetProductosByIdsCPESQLUseCase
    {
        private readonly IProductoSQLRepo _productRepo;
        private readonly ILogger _logger;

        public GetProductosByIdsCPESQLUseCase(IProductoSQLRepo productRepo, ILogger logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        /// <summary>
        /// Gets the products by their ids, but also filtering the CIDVALORCLASIFICACIO6 field, it ignores the 0 value i this field
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>CPE filtered ProductDTO list of istances</returns>
        public async Task<List<ProductoDTO>> Execute(List<int> ids)
        {
            var productos = await _productRepo.GetProductByIdsCPEAsync(ids);

            var dTOs = new List<ProductoDTO>();

            foreach (var producto in productos)
            {
                dTOs.Add(new ProductoDTO(producto));
            }

            return dTOs;
        }
    }
}
