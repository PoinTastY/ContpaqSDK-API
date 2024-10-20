using Application.DTOs;
using Application.UseCases.SQL.ClienteProveedor;
using Microsoft.AspNetCore.Mvc;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class ClienteProveedorController : Controller
    {
        readonly SearchClienteProveedorByNameSQLUseCase _searchClienteProveedorByNameSQLUseCase;
        public ClienteProveedorController(SearchClienteProveedorByNameSQLUseCase searchClienteProveedorByNameSQLUseCase)
        {
            _searchClienteProveedorByNameSQLUseCase = searchClienteProveedorByNameSQLUseCase;
        }

        [HttpGet]
        [Route("ClienteProveedor/ByNombre")]
        public async Task<ActionResult<ApiResponse>> GetClienteProveedorByName(string nombre)
        {
            try
            {
                var matches = await _searchClienteProveedorByNameSQLUseCase.Execute(nombre);
                return Ok(new ApiResponse { Message = "ClienteProveedor encontrado", Data = matches, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
