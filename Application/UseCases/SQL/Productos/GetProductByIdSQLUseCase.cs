using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Productos
{
    public class GetProductByIdSQLUseCase
    {
        private readonly IProductoSQLRepo _productRepo;
        private readonly ILogger _logger;

        public GetProductByIdSQLUseCase(IProductoSQLRepo productRepo, ILogger logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<ProductoDTO> Execute(int idProducto)
        {
            _logger.Log($"Obteniendo solicitud de buscar el producto con id: {idProducto}");
            var productoSQL = await _productRepo.GetProductByIdAsync(idProducto);
            return new ProductoDTO(productoSQL);
        }
    }
}
