using Application.DTOs;
using Core.Domain.Interfaces.Repositories.SQL;
using Core.Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Productos
{
    public class GetProductosByIdsSQLUseCase
    {
        private readonly IProductoSQLRepo _productRepo;
        private readonly ILogger _logger;

        public GetProductosByIdsSQLUseCase(IProductoSQLRepo productRepo, ILogger logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<List<ProductoDTO>> Execute(List<string> codigos)
        {
            try
            {
                _logger.Log("Obteniendo productos por ids...");
                var productos = await _productRepo.GetProductsByMultipleCodigosAsync(codigos);

                var dTos = new List<ProductoDTO>();

                foreach (var producto in productos)
                {
                    dTos.Add(new ProductoDTO(producto));
                }

                return dTos;
            }
            catch (Exception ex)
            {
                _logger.Log("Error obteniendo productos: " + ex.Message);
                throw;
            }
        }
    }
}
