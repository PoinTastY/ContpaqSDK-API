using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Productos
{
    public class SearchProductosByNameSQLUseCase
    {
        private readonly IProductoSQLRepo _productoSQLRepo;
        private readonly ILogger _logger;

        public SearchProductosByNameSQLUseCase(IProductoSQLRepo productoSQLRepo, ILogger logger)
        {
            _productoSQLRepo = productoSQLRepo;
            _logger = logger;
        }

        /// <summary>
        /// Busca un producto por nombre
        /// </summary>
        /// <param name="name"></param>
        public async Task<IEnumerable<ProductoDto>> Execute(string name)
        {
            _logger.Log($"Obteniendo solicitud de buscar el producto con nombre: {name}");

            var productosSQL = await _productoSQLRepo.SearchByNameAsync(name);

            return productosSQL.Select(p => new ProductoDto(p));
        }
    }
}
