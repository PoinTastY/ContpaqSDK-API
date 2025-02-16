using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.SQL.Productos
{
    public class GetProductoByCodigoSQLUseCase
    {
        private readonly IProductoSQLRepo _productRepo;
        private readonly ILogger _logger;

        public GetProductoByCodigoSQLUseCase(IProductoSQLRepo productRepo, ILogger logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene un producto por su codigo
        /// </summary>
        /// <param name="codigoProducto"></param>
        public async Task<ProductoDto> Execute(string codigoProducto)
        {
            _logger.Log($"Obteniendo solicitud de buscar el producto con codigo: {codigoProducto}");
            return new ProductoDto(await _productRepo.GetProductByCodigoAsync(codigoProducto));
        }
    }
}
