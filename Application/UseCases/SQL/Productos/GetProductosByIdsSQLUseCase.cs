using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Productos
{
    public class GetProductosByIdsSQLUseCase
    {
        private readonly IProductoSQLRepo _productoSQLRepo;
        private readonly ILogger _logger;

        public GetProductosByIdsSQLUseCase(IProductoSQLRepo productoSQLRepo, ILogger logger)
        {
            _productoSQLRepo = productoSQLRepo;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista de productos por una lista de ids
        /// </summary>
        /// <param name="ids"></param>
        public async Task<IEnumerable<ProductoDto>> Execute(List<int> ids)
        {
            _logger.Log("Ejecutando caso de uso GetProductosByIdsCPESQL...");

            var productos = await _productoSQLRepo.GetByIdsAsync(ids);

            return productos.Select(p => new ProductoDto(p));
        }
    }
}
