using Core.Domain.Entities.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Core.Application.UseCases.SQL.Productos
{
    public class GetProductByIdSQLUseCase
    {
        private readonly IProductoSQLRepo _productoSQLRepo;
        private readonly ILogger _logger;

        public GetProductByIdSQLUseCase(IProductoSQLRepo productoSQLRepo, ILogger logger)
        {
            _productoSQLRepo = productoSQLRepo;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene un producto por su id
        /// </summary>
        /// <param name="idProducto"></param>
        public async Task<ProductoDto> Execute(int idProducto)
        {
            _logger.Log($"Obteniendo solicitud de buscar el producto con id: {idProducto}");
            return new ProductoDto(await _productoSQLRepo.GetByIdAsync(idProducto));
        }
    }
}
