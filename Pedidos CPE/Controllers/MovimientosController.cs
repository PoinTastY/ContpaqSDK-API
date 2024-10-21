using Application.DTOs;
using Application.UseCases.Postgres.Movimientos;
using Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pedidos_CPE.Controllers
{
    [ApiController]
    public class MovimientosController : Controller
    {
        private readonly UpdateMovimientosPostgresUseCase _updateMovimientos;
        private readonly GetMovimientosByDocumentoIdPostgresUseCase _getMovimientosByDocumentoIdPostgresUseCase;

        public MovimientosController(UpdateMovimientosPostgresUseCase updateMovimientos, GetMovimientosByDocumentoIdPostgresUseCase getMovimientosByDocumentoIdPostgresUseCase)
        {
            _updateMovimientos = updateMovimientos;
            _getMovimientosByDocumentoIdPostgresUseCase = getMovimientosByDocumentoIdPostgresUseCase;
        }

        [HttpPatch]
        [Route("Movimientos")]
        public async Task<ActionResult<ApiResponse>> PatchMovimientosUnidades(List<Movimiento> movimientos)
        {
            try
            {
                await _updateMovimientos.Execute(movimientos);
                return CreatedAtAction("PatchMovimientos", new ApiResponse { Message = "Movimientos agregados con éxito", Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Movimientos/ByDocumentoId")]
        public async Task<ActionResult<ApiResponse>> GetMovimientosByDocumentoId(int documentoId)
        {
            try
            {
                var movimientos = await _getMovimientosByDocumentoIdPostgresUseCase.Execute(documentoId);
                return Ok(new ApiResponse { Message = "Movimientos encontrados", Data = movimientos, Success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
