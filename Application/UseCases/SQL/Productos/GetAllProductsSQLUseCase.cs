using Application.DTOs;
using Domain.Interfaces.Repos;
using Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Productos
{
    public class GetAllProductsSQLUseCase
    {
        private readonly IProductRepo _productRepo;
        private readonly ILogger _logger;

        public GetAllProductsSQLUseCase(IProductRepo productRepo, ILogger logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductoDTO>> Execute()
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
