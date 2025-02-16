using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Productos
{
    public class GetAllProductsSQLUseCase
    {
        private readonly IProductoSQLRepo _productRepo;
        private readonly ILogger _logger;

        public GetAllProductsSQLUseCase(IProductoSQLRepo productRepo, ILogger logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<List<ProductoDTO>> Execute()
        {
            var products = await _productRepo.GetAllProductsAsync();
            _logger.Log("Products retrieved successfully");

            var dTOs = new List<ProductoDTO>();

            foreach (var product in products)
            {
                dTOs.Add(new ProductoDTO(product));
            }

            return dTOs;
        }
    }
}
