using Application.DTOs;
using Domain.Interfaces.Repos;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.SQL.Productos
{
    public class GetProductoByCodigoSQLUseCase
    {
        private readonly IProductRepo _productRepo;
        private readonly ILogger _logger;

        public GetProductoByCodigoSQLUseCase(IProductRepo productRepo, ILogger logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<ProductoDTO> Execute(string codigoProducto)
        {
            try
            {
                _logger.Log($"Obteniendo solicitud de buscar el producto con codigo: {codigoProducto}");
                var productoSQL = await _productRepo.GetProductByCodigoAsync(codigoProducto);
                return new ProductoDTO(productoSQL);
            }
            catch (Exception e)
            {
                _logger.Log($"Error al obtener el producto con codigo: {codigoProducto}, {e.Message}");
                throw;
            }
        }
    }
}
