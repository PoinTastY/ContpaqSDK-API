using Application.DTOs;
using Application.UseCases.SQL.ClienteProveedor;
using Core.Domain.Entities.SQL;
using Microsoft.AspNetCore.Mvc;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class ClienteProveedorController : Controller
    {
        readonly SearchClienteProveedorByNameSQLUseCase _searchClienteProveedorByNameSQL;
        public ClienteProveedorController(SearchClienteProveedorByNameSQLUseCase searchClienteProveedorByNameSQLUseCase)
        {
            _searchClienteProveedorByNameSQL = searchClienteProveedorByNameSQLUseCase;
        }

        [HttpGet]
        [Route("ClienteProveedor/ByNombre/")]
        public async Task<ActionResult<ApiResponse>> GetClienteProveedorByName([FromQuery] string nombre)
        {
            try
            {
                List<ClienteProveedorDto> matches = new(await _searchClienteProveedorByNameSQL.Execute(nombre));
                return Ok(new ApiResponse { Message = "ClienteProveedor encontrado", Data = matches, Success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse { Success = false, Message = $"No se encontraron resultados para: {nombre}", ErrorDetails = ex.Message });
            }
        }
    }
}
