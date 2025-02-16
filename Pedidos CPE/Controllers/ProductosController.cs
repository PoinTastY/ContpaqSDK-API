using Core.Application.DTOs;
using Core.Application.UseCases.SQL.Productos;
using Microsoft.AspNetCore.Mvc;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class ProductosController : Controller
    {
        private readonly SearchProductosByNameSQLUseCase _searchProductosByNameSQLUseCase;
        //private readonly GetProductosByIdsSQLUseCase _getProductosByIdsSQLUseCase;
        //private readonly GetProductoByCodigoSQLUseCase _getProductoByCodigoSQL;
        public ProductosController(SearchProductosByNameSQLUseCase searchProductosByNameSQLUseCase, GetProductosByIdsSQLUseCase getProductosByIdsSQLUseCase,
            GetProductoByCodigoSQLUseCase getProductoByCodigoSQLUseCase)
        {
            _searchProductosByNameSQLUseCase = searchProductosByNameSQLUseCase;
            //_getProductosByIdsSQLUseCase = getProductosByIdsSQLUseCase;
            //_getProductoByCodigoSQL = getProductoByCodigoSQLUseCase;
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

        //[HttpGet]
        //[Route("Productos/ByCodigos")]
        //public async Task<ActionResult<ApiResponse>> GetProductosByCodigos([FromQuery] List<string> codigos)
        //{
        //    try
        //    {
        //        var productos = await _getProductoByCodigoSQL.Execute(codigos);
        //        return Ok(new ApiResponse { Message = "Productos encontrados", Data = productos, Success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}
    }
}
