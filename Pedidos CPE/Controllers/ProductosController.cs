using Application.DTOs;
using Application.UseCases.SQL.Productos;
using Microsoft.AspNetCore.Mvc;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class ProductosController : Controller
    {
        private readonly SearchProductosByNameSQLUseCase _searchProductosByNameSQLUseCase;
        public ProductosController(SearchProductosByNameSQLUseCase searchProductosByNameSQLUseCase)
        {
            _searchProductosByNameSQLUseCase = searchProductosByNameSQLUseCase;
        }

        [HttpGet]
        [Route("Productos/ByNombre")]
        public async Task<ActionResult<ApiResponse>> GetProductosByName(string nombre)
        {
            try
            {
                var productos = await _searchProductosByNameSQLUseCase.Execute(nombre);
                return Ok(new ApiResponse { Message = "Productos encontrados", Data = productos, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
