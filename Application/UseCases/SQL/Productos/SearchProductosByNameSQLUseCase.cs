using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Productos
{
    public class SearchProductosByNameSQLUseCase
    {
        private readonly IProductoSQLRepo _productRepo;
        private readonly ILogger _logger;

        public SearchProductosByNameSQLUseCase(IProductoSQLRepo productRepo, ILogger logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<List<ProductoDTO>> Execute(string name)
        {
            _logger.Log($"Obteniendo solicitud de buscar el producto con nombre: {name}");
            var productosSQL = await _productRepo.SearchProductosByNameAsync(name);
            return productosSQL.Select(p => new ProductoDTO(p)).ToList();
        }
    }
}
