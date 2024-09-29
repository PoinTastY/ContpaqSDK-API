using Application.DTOs;
using Domain.Interfaces.Repos;
using Domain.Interfaces.Services;

namespace Application.UseCases.SQL.Productos
{
    public class GetProductosByIdsSQLUseCase
    {
        private readonly IProductRepo _productRepo;
        private readonly ILogger _logger;

        public GetProductosByIdsSQLUseCase(IProductRepo productRepo, ILogger logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<List<ProductoDTO>> Execute(List<int> ids)
        {
            try
            {
                _logger.Log("Obteniendo productos por ids...");
                var productos = await _productRepo.GetProductsByMultipleIdsAsync(ids);

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
