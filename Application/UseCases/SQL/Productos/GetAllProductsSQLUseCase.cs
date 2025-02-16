using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Productos
{
    public class GetAllProductsSQLUseCase
    {
        private readonly IProductoSQLRepo _productoSQLRepo;
        private readonly ILogger _logger;

        public GetAllProductsSQLUseCase(IProductoSQLRepo productoSQLRepo, ILogger logger)
        {
            _productoSQLRepo = productoSQLRepo;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los productos diaplos
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductoDto>> Execute()
        {
            _logger.Log("Ejecutando el caso de uso GetAllProductsSQL...");

            var productos = await _productoSQLRepo.GetAllAsync();

            _logger.Log("Productos obtenidos exitosamente");

            return productos.Select(p => new ProductoDto(p));
        }
    }
}
